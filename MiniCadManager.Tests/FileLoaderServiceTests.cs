using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCadManager.Core.Models;
using MiniCadManager.Core.Services;
using System.IO;
using System.Linq;

namespace MiniCadManager.Tests
{
    [TestClass]
    public class FileLoaderServiceTests
    {
        private FileLoaderService _service = null!;
        private string _testFilePath = "test_objects.json";

        [TestInitialize]
        public void Setup()
        {
            _service = new FileLoaderService();
            File.WriteAllText(_testFilePath, "[{\"Id\":1,\"Name\":\"Test\",\"Type\":\"Line\",\"Layer\":\"0\",\"X\":10.0,\"Y\":20.0}]");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }

        [TestMethod]
        public void LoadFromJson_ValidFile_ReturnsObjects()
        {
            var result = _service.LoadFromJson(_testFilePath);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test", result[0].Name);
            Assert.AreEqual(10.0, result[0].X);
        }

        [TestMethod]
        public void ExportToJson_ValidObjects_WritesFile()
        {
            var objects = new[] { new CadObject { Id = 2, Name = "ExportTest" } };
            string exportPath = "export_test.json";
            
            _service.ExportToJson(objects, exportPath);
            
            Assert.IsTrue(File.Exists(exportPath));
            
            File.Delete(exportPath);
        }
    }
}
