using Jankilla.Core.Contracts;
using JsonSubTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class BlockTest
    {
        private Jankilla.Core.Contracts.Driver _driver;
        private JsonSerializerSettings _settings;
        [TestInitialize]
        public void Setup()
        {
            _settings = new JsonSerializerSettings();

            _settings.Converters.Add(JsonSubtypesConverterBuilder
              .Of<Jankilla.Core.Contracts.Driver>("Discriminator")
              .RegisterSubtype<MitsubishiMxComponentDriver>("MitsubishiMxComponent")
              .Build());

            _settings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<Jankilla.Core.Contracts.Device>("Discriminator")
                .RegisterSubtype<MitsubishiMxComponentDevice>("MitsubishiMxComponent")
                .Build());

            _settings.Converters.Add(JsonSubtypesConverterBuilder
               .Of<Jankilla.Core.Contracts.Block>("Discriminator")
               .RegisterSubtype<MitsubishiMxComponentBlock>("MitsubishiMxComponent")
               .Build());

            _driver = new MitsubishiMxComponentDriver();
            _driver.Name = "TEST01DRV";

            var device = new MitsubishiMxComponentDevice()
            {
                Name = "TEST01DEV",
            };

            _driver.AddDevice(device);
            device.AddBlock(new MitsubishiMxComponentBlock() { Name = "BL1", StationNo = 1, StartAddress = "D1000", BufferSize = 200 });

           
        }

        [TestMethod]
        public void Block_ShouldSerialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Contains("D1000"));
        }

        [TestMethod]
        public void Block_ShouldDeserialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            var drv = JsonConvert.DeserializeObject<Jankilla.Core.Contracts.Driver>(str, _settings);
            var blk = (MitsubishiMxComponentBlock)drv.Devices.FirstOrDefault().Blocks.FirstOrDefault();
            Jankilla.Core.Contracts.Block block = _driver.Devices.FirstOrDefault().Blocks.FirstOrDefault();

            Assert.AreEqual(block.Name, blk.Name);
            Assert.AreEqual(blk.StationNo, 1);
        }
    }
}
