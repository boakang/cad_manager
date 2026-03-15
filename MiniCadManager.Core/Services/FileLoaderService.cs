using System.Text.Json;
using MiniCadManager.Core.Models;

namespace MiniCadManager.Core.Services
{
    public class FileLoaderService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>Loads CadObjects from a JSON file.</summary>
        public List<CadObject> LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"JSON file not found: {filePath}", filePath);

            string json = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<List<CadObject>>(json, _jsonOptions);
            return result ?? new List<CadObject>();
        }

        /// <summary>Loads CadObjects from a CSV file. Expected header: Id,Name,Type,Layer,X,Y</summary>
        public List<CadObject> LoadFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"CSV file not found: {filePath}", filePath);

            var lines = File.ReadAllLines(filePath);
            var objects = new List<CadObject>();

            // skip header
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 6) continue;

                if (int.TryParse(parts[0].Trim(), out int id) &&
                    double.TryParse(parts[4].Trim(), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out double x) &&
                    double.TryParse(parts[5].Trim(), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out double y))
                {
                    objects.Add(new CadObject
                    {
                        Id = id,
                        Name = parts[1].Trim(),
                        Type = parts[2].Trim(),
                        Layer = parts[3].Trim(),
                        X = x,
                        Y = y
                    });
                }
            }
            return objects;
        }

        /// <summary>Serializes results to a JSON file.</summary>
        public void ExportToJson(IEnumerable<CadObject> objects, string filePath)
        {
            var json = JsonSerializer.Serialize(objects.ToList(),
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        /// <summary>Exports optimized route summary to a TXT file.</summary>
        public void ExportToTxt(string content, string filePath)
        {
            File.WriteAllText(filePath, content);
        }
    }
}
