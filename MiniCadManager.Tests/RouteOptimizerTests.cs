using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCadManager.Core.Algorithms;
using MiniCadManager.Core.Models;
using System.Collections.Generic;

namespace MiniCadManager.Tests
{
    [TestClass]
    public class RouteOptimizerTests
    {
        private RouteOptimizer _optimizer = null!;

        [TestInitialize]
        public void Setup()
        {
            _optimizer = new RouteOptimizer();
        }

        [TestMethod]
        public void CalculateTotalDistance_ReturnsCorrectDistance()
        {
            var route = new List<CadObject>
            {
                new CadObject { X = 0, Y = 0 },
                new CadObject { X = 3, Y = 4 }
            };

            double dist = _optimizer.CalculateTotalDistance(route);
            Assert.AreEqual(5.0, dist, 0.001);
        }

        [TestMethod]
        public void OptimizeRoute_OrdersByNearestNeighbor()
        {
            // P1(0,0), P2(100,100), P3(3,4)
            // Original distance 0->100 is ~141, 100->3 is ~137, total ~278
            // Optimized distance 0->3 is 5, 3->100 is ~137, total ~142
            var objects = new List<CadObject>
            {
                new CadObject { Id = 1, X = 0, Y = 0 },
                new CadObject { Id = 2, X = 100, Y = 100 },
                new CadObject { Id = 3, X = 3, Y = 4 }
            };

            var result = _optimizer.OptimizeRoute(objects);

            Assert.AreEqual(3, result.OptimizedRoute.Count);
            Assert.AreEqual(1, result.OptimizedRoute[0].Id);
            Assert.AreEqual(3, result.OptimizedRoute[1].Id);
            Assert.AreEqual(2, result.OptimizedRoute[2].Id);
            
            Assert.IsTrue(result.OptimizedDistance < result.OriginalDistance);
        }
    }
}
