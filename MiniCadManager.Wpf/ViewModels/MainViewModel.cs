using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MiniCadManager.Core.Algorithms;
using MiniCadManager.Core.Models;
using MiniCadManager.Core.Services;
using MiniCadManager.Wpf.Commands;

namespace MiniCadManager.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly FileLoaderService _fileLoaderService;
        private readonly SearchService _searchService;
        private readonly RouteOptimizer _routeOptimizer;

        private List<CadObject> _allObjects = new();

        public MainViewModel()
        {
            _fileLoaderService = new FileLoaderService();
            _searchService = new SearchService();
            _routeOptimizer = new RouteOptimizer();

            LoadDataCommand = new RelayCommand(_ => ExecuteLoadData());
            OptimizeRouteCommand = new RelayCommand(_ => ExecuteOptimizeRoute(), _ => CadObjects.Any());
            RemoveDuplicatesCommand = new RelayCommand(_ => ExecuteRemoveDuplicates(), _ => CadObjects.Any());
            ExportCommand = new RelayCommand(_ => ExecuteExport(), _ => CadObjects.Any());
            ToggleThemeCommand = new RelayCommand(_ => ExecuteToggleTheme());
        }

        // Properties

        private ObservableCollection<CadObject> _cadObjects = new();
        public ObservableCollection<CadObject> CadObjects
        {
            get => _cadObjects;
            set => SetProperty(ref _cadObjects, value);
        }

        public int TotalObjects => CadObjects.Count;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilters();
                }
            }
        }

        private string _selectedType = "All";
        public string SelectedType
        {
            get => _selectedType;
            set
            {
                if (SetProperty(ref _selectedType, value))
                {
                    ApplyFilters();
                }
            }
        }

        public ObservableCollection<string> AvailableTypes { get; } = new ObservableCollection<string> { "All", "Line", "Circle", "Text" };

        private string _optimizationResultText = string.Empty;
        public string OptimizationResultText
        {
            get => _optimizationResultText;
            set => SetProperty(ref _optimizationResultText, value);
        }

        // Commands

        public ICommand LoadDataCommand { get; }
        public ICommand OptimizeRouteCommand { get; }
        public ICommand RemoveDuplicatesCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        // Methods

        private void ExecuteLoadData()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (openFileDialog.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        _allObjects = _fileLoaderService.LoadFromCsv(openFileDialog.FileName);
                    }
                    else
                    {
                        _allObjects = _fileLoaderService.LoadFromJson(openFileDialog.FileName);
                    }
                    ApplyFilters();
                    OptimizationResultText = "Data loaded successfully.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ApplyFilters()
        {
            var filtered = _searchService.FilterAndSearch(_allObjects, SearchText, SelectedType).ToList();
            CadObjects = new ObservableCollection<CadObject>(filtered);
            OnPropertyChanged(nameof(TotalObjects));
        }

        private void ExecuteOptimizeRoute()
        {
            if (!CadObjects.Any()) return;

            var result = _routeOptimizer.OptimizeRoute(CadObjects);

            CadObjects = new ObservableCollection<CadObject>(result.OptimizedRoute);
            
            OptimizationResultText = $"Total Objects: {result.ObjectCount}\n" +
                                     $"Original Distance: {result.OriginalDistance:F2}\n" +
                                     $"Optimized Distance: {result.OptimizedDistance:F2}";
        }

        private void ExecuteRemoveDuplicates()
        {
            if (!CadObjects.Any()) return;

            var result = _searchService.RemoveDuplicates(CadObjects).ToList();
            int removed = CadObjects.Count - result.Count;

            CadObjects = new ObservableCollection<CadObject>(result);
            _allObjects = _allObjects.Where(o => result.Any(r => r.Id == o.Id)).ToList(); // update base list

            OnPropertyChanged(nameof(TotalObjects));
            OptimizationResultText = $"Removed {removed} duplicates.";
        }

        private void ExecuteExport()
        {
            if (!CadObjects.Any()) return;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|Text lines (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (saveFileDialog.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        _fileLoaderService.ExportToJson(CadObjects, saveFileDialog.FileName);
                    }
                    else
                    {
                        string content = string.Join(Environment.NewLine, CadObjects.Select(o => o.ToString()));
                        _fileLoaderService.ExportToTxt(content, saveFileDialog.FileName);
                    }
                    MessageBox.Show("Export successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool _isDarkTheme = false;
        private void ExecuteToggleTheme()
        {
            _isDarkTheme = !_isDarkTheme;
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri(
                _isDarkTheme ? "pack://application:,,,/MiniCadManager.Wpf;component/Themes/DarkTheme.xaml" 
                             : "pack://application:,,,/MiniCadManager.Wpf;component/Themes/LightTheme.xaml");
        }
    }
}
