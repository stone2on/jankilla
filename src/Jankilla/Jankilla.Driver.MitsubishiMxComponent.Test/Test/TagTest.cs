using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Converters;
using Jankilla.Core.Tags.Base;
using JsonSubTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class TagTest
    {
        private Jankilla.Core.Contracts.Driver _driver;
        private JsonSerializerSettings _settings;

        private IntTag _intTag;
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

            _settings.Converters.Add(JsonSubtypesConverterBuilder
               .Of<Tag>("Discriminator")
               .RegisterSubtype<IntTag>(ETagDiscriminator.Int)
               .Build());

            _driver = new MitsubishiMxComponentDriver();
            _driver.Name = "TEST01DRV";

            var device = new MitsubishiMxComponentDevice()
            {
                Name = "TEST01DEV",
            };

            _driver.AddDevice(device);
            var block = new MitsubishiMxComponentBlock() { Name = "BL1", StationNo = 1, StartAddress = "D1000", BufferSize = 200 };
            device.AddBlock(block);
            _intTag = new IntTag() { Name = "TestIntTag", Address = "D1001" };
            block.AddTag(_intTag);
            

        }

        [TestMethod]
        public void Tag_ShouldSerialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Contains("D1001"));
        }

        [TestMethod]
        public void Tag_ShouldDeserialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            var drv = JsonConvert.DeserializeObject<Jankilla.Core.Contracts.Driver>(str, _settings);
            var blk = (MitsubishiMxComponentBlock)drv.Devices.FirstOrDefault().Blocks.FirstOrDefault();
            Jankilla.Core.Contracts.Block block = _driver.Devices.FirstOrDefault().Blocks.FirstOrDefault();
            var tag = block.Tags.FirstOrDefault();

            Assert.AreEqual(tag.Name, _intTag.Name);
            Assert.AreEqual(tag.Address, _intTag.Address);

            var a = CsvProjectHelper.Instance;
        }

      
    }
}
