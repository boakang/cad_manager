using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCadManager.Core.Models;
using MiniCadManager.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniCadManager.Tests
{
    [TestClass]
    public class SearchServiceTests
    {
        private SearchService _service = null!;
        private List<CadObject> _objects = null!;

        [TestInitialize]
        public void Setup()
        {
            _service = new SearchService();
            _objects = new List<CadObject>
            {
                new CadObject { Id = 1, Name = "Line1", Type = "Line", X = 0, Y = 0 },
                new CadObject { Id = 2, Name = "Circle1", Type = "Circle", X = 10, Y = 10 },
                new CadObject { Id = 3, Name = "Line2", Type = "Line", X = 0, Y = 0 } // Duplicate coords
            };
        }

        [TestMethod]
        public void FilterAndSearch_ByType_ReturnsOnlyType()
        {
            var result = _service.FilterAndSearch(_objects, "", "Line").ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(r => r.Type == "Line"));
        }

        [TestMethod]
        public void FilterAndSearch_ByName_ReturnsMatches()
        {
            var result = _service.FilterAndSearch(_objects, "1", "All").ToList();
            Assert.AreEqual(2, result.Count); // Line1, Circle1
        }

        [TestMethod]
        public void RemoveDuplicates_RemovesIdenticalCoords()
        {
            var result = _service.RemoveDuplicates(_objects).ToList();
            Assert.AreEqual(2, result.Count);
        }
    }
}
