using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Proto;
using Jankilla.Core.Services;
using Jankilla.Core.Tags;
using Jankilla.Driver.Mitsubishi.MxComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Test
{
    [TestClass]
    public class _01_GrpcTest
    {
        private Mock<ServerCallContext> _mockContext;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<ServerCallContext>();
        }

        [TestMethod]
        public async Task GetProject_WithEmptyProject_ReturnsEmptyResponse()
        {
            // Arrange
            var emptyProject = new Core.Contracts.Project();

            var adapter = new ProjectServiceAdapter(emptyProject);

            // Act
            var result = await adapter.GetProject(new Empty(), _mockContext.Object);

            // Assert
            Assert.AreEqual(0, result.Drivers.Count);
        }

        [TestMethod]
        public async Task GetProject_WithSingleDriver_ReturnsCorrectStructure()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var mitsubishiDriver = new MitsubishiMxComponentDriver
            {
                ID = testId,
            };
            var project = new Core.Contracts.Project();
            project.AddDriver(mitsubishiDriver);

            var adapter = new ProjectServiceAdapter(project);

            // Act
            var result = await adapter.GetProject(new Empty(), _mockContext.Object);

            // Assert
            Assert.AreEqual(1, result.Drivers.Count);
            Assert.AreEqual(testId.ToString(), result.Drivers[0].Id);
        }

        [TestMethod]
        public async Task GetProject_WithCompleteStructure_ReturnsAllElements()
        {
            // Arrange
            var project = CreateTestProject();
            var adapter = new ProjectServiceAdapter(project);

            // Act
            var result = await adapter.GetProject(new Empty(), _mockContext.Object);

            // Assert
            // Driver level checks
            Assert.AreEqual(1, result.Drivers.Count);
            var driver = result.Drivers[0];

            // Device level checks
            Assert.AreEqual(1, driver.Devices.Count);
            var device = driver.Devices[0];

            // Block level checks
            Assert.AreEqual(1, device.Blocks.Count);
            var block = device.Blocks[0];

            // Tag level checks
            Assert.AreEqual(2, block.Tags.Count);
            var tag = block.Tags[0];

            // Verify tag properties
            Assert.AreEqual("TestTag", tag.Name);
            Assert.AreEqual("TestCategory", tag.Category);
            Assert.AreEqual(TagKind.Int, tag.Kind);
        }

        [TestMethod]
        public async Task GetProject_WithMultipleTags_PreservesTagOrder()
        {
            // Arrange
            var mitsubishiDriver = new MitsubishiMxComponentDriver
            {
                ID = Guid.NewGuid(),
                Name = "TestDriver"
            };

            var device = new MitsubishiMxComponentDevice
            {
                ID = Guid.NewGuid(),
                Name = "TestDevice"
            };
            mitsubishiDriver.AddDevice(device);

            var block = new MitsubishiMxComponentBlock
            {
                ID = Guid.NewGuid(),
                Name = "TestBlock",
                StartAddress = "D0000",
                BufferSize = 2000,
                StationNo = 1
            };
            device.AddBlock(block);

            block.AddTag(new IntTag { ID = Guid.NewGuid(), Name = "Tag1", Category = "Cat1", Address="D0" });
            block.AddTag(new StringTag { ID = Guid.NewGuid(), Name = "Tag2", Category = "Cat2", Address = "D10" });
            block.AddTag(new IntTag { ID = Guid.NewGuid(), Name = "Tag3", Category = "Cat3", Address = "D30" });

            var project = new Core.Contracts.Project();
            project.AddDriver(mitsubishiDriver);

            var adapter = new ProjectServiceAdapter(project);

            // Act
            var result = await adapter.GetProject(new Empty(), _mockContext.Object);

            // Assert
            var tags = result.Drivers[0].Devices[0].Blocks[0].Tags;
            Assert.AreEqual(3, tags.Count);
            Assert.AreEqual("Tag1", tags[0].Name);
            Assert.AreEqual("Tag2", tags[1].Name);
            Assert.AreEqual("Tag3", tags[2].Name);

            // Additional detailed checks
            Assert.AreEqual("Cat1", tags[0].Category);
            Assert.AreEqual("Cat2", tags[1].Category);
            Assert.AreEqual("Cat3", tags[2].Category);
            Assert.IsTrue(tags[0].Kind == TagKind.Int);
            Assert.IsTrue(tags[1].Kind == TagKind.String);
            Assert.IsTrue(tags[2].Kind == TagKind.Int);
        }

     
        private Core.Contracts.Project CreateTestProject()
        {
            var mitsubishiDriver = new MitsubishiMxComponentDriver
            {
                ID = Guid.NewGuid(),
                Name = "TestDriver"
            };

            var device = new MitsubishiMxComponentDevice
            {
                ID = Guid.NewGuid(),
                Name = "TestDevice"
            };
            var deviceResult = mitsubishiDriver.AddDevice(device);
            Debug.Assert(deviceResult.IsValid, $"Failed to add device: {deviceResult.Message}");

            var block = new MitsubishiMxComponentBlock
            {
                ID = Guid.NewGuid(),
                StartAddress = "D0000",
                BufferSize = 2000,
                Name = "TestBlock",
                StationNo = 1
            };
            var blockResult = device.AddBlock(block);
            Debug.Assert(blockResult.IsValid, $"Failed to add block: {blockResult.Message}");

            var intTag = new IntTag
            {
                ID = Guid.NewGuid(),
                Name = "TestTag",
                Category = "TestCategory",
                Address = "D0000",
            };

            var intTagResult = block.AddTag(intTag);
            Debug.Assert(intTagResult.IsValid, $"Failed to add IntTag: {intTagResult.Message}");

            var stringTag = new StringTag
            {
                ID = Guid.NewGuid(),
                Name = "TestTag2",
                Category = "TestCategory2",
                Address = "D0020",
            };
            var stringTagResult = block.AddTag(stringTag);
            Debug.Assert(stringTagResult.IsValid, $"Failed to add StringTag: {stringTagResult.Message}");

            var project = new Core.Contracts.Project();
            var projectResult = project.AddDriver(mitsubishiDriver);
            Debug.Assert(projectResult.IsValid, $"Failed to add driver: {projectResult.Message}");

            return project;
        }


    }
}


