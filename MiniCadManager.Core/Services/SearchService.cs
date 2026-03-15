using MiniCadManager.Core.Models;

namespace MiniCadManager.Core.Services
{
    public class SearchService
    {
        public IEnumerable<CadObject> FilterAndSearch(
            IEnumerable<CadObject> source, 
            string searchText, 
            string objectType)
        {
            var result = source;

            // Filter by type
            if (!string.IsNullOrWhiteSpace(objectType) && objectType != "All")
            {
                result = result.Where(o => o.Type.Equals(objectType, StringComparison.OrdinalIgnoreCase));
            }

            // Search by name
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                result = result.Where(o => o.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public IEnumerable<CadObject> RemoveDuplicates(IEnumerable<CadObject> source)
        {
            // Simple deduplication based on X and Y within a small tolerance
            double tolerance = 0.001;
            var distinctList = new List<CadObject>();

            foreach (var item in source)
            {
                if (!distinctList.Any(d => Math.Abs(d.X - item.X) < tolerance && Math.Abs(d.Y - item.Y) < tolerance && d.Layer == item.Layer && d.Type == item.Type))
                {
                    distinctList.Add(item);
                }
            }

            return distinctList;
        }
    }
}
