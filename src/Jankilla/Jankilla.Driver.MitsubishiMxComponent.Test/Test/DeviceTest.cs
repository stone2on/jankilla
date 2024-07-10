using Jankilla.Core.Contracts;
using JsonSubTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class DeviceTest
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

            _driver = new MitsubishiMxComponentDriver();
            _driver.Name = "TEST01DRV";
            _driver.AddDevice(new MitsubishiMxComponentDevice()
            {
                Name = "TEST01DEV"
            });
        }

        [TestMethod]
        public void Device_ShouldSerialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Contains("TEST01DEV"));
        }

        [TestMethod]
        public void Device_ShouldDeserialize()
        {
            var str = JsonConvert.SerializeObject(_driver, _settings);
            var drv = JsonConvert.DeserializeObject<Jankilla.Core.Contracts.Driver>(str, _settings);
            var dev = drv.Devices.FirstOrDefault();
            Jankilla.Core.Contracts.Device device = _driver.Devices.FirstOrDefault();

            Assert.AreEqual(device.Name, dev.Name);
        }
    }
}
