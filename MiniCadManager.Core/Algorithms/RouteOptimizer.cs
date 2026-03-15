using MiniCadManager.Core.Models;

namespace MiniCadManager.Core.Algorithms
{
    public class OptimizationResult
    {
        public List<CadObject> OptimizedRoute { get; set; } = new();
        public double OriginalDistance { get; set; }
        public double OptimizedDistance { get; set; }
        public int ObjectCount => OptimizedRoute.Count;
    }

    public class RouteOptimizer
    {
        public OptimizationResult OptimizeRoute(IEnumerable<CadObject> objects)
        {
            var originalList = objects.ToList();
            if (originalList.Count <= 1)
            {
                return new OptimizationResult
                {
                    OptimizedRoute = originalList,
                    OriginalDistance = 0,
                    OptimizedDistance = 0
                };
            }

            double originalDist = CalculateTotalDistance(originalList);

            // Nearest Neighbor TSP
            var unvisited = new List<CadObject>(originalList);
            var optimized = new List<CadObject>(originalList.Count);

            // Start from the first object
            var current = unvisited[0];
            optimized.Add(current);
            unvisited.RemoveAt(0);

            while (unvisited.Count > 0)
            {
                // Find nearest
                double minDist = double.MaxValue;
                CadObject nearest = null!;

                foreach (var candidate in unvisited)
                {
                    double d = current.DistanceTo(candidate);
                    if (d < minDist)
                    {
                        minDist = d;
                        nearest = candidate;
                    }
                }

                optimized.Add(nearest);
                unvisited.Remove(nearest);
                current = nearest;
            }

            double optimizedDist = CalculateTotalDistance(optimized);

            return new OptimizationResult
            {
                OptimizedRoute = optimized,
                OriginalDistance = originalDist,
                OptimizedDistance = optimizedDist
            };
        }

        public double CalculateTotalDistance(List<CadObject> route)
        {
            double total = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                total += route[i].DistanceTo(route[i + 1]);
            }
            return total;
        }
    }
}
