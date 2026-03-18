using Jankilla.Core.Contracts;
using Jankilla.Driver.Mitsubishi.MxComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories
{
    class DeviceRepository
    {
        public string ConnectionString { get; set; }

        private MitsubishiMxComponentDeviceRepository MitsubishiMxComponentDeviceRepo { get; set; }

        public DeviceRepository(string connString)
        {
            ConnectionString = connString;

            MitsubishiMxComponentDeviceRepo = new MitsubishiMxComponentDeviceRepository(connString);
        }

        public int Add(Contracts.Driver parent, Device device)
        {
            switch (device.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDeviceRepo.Add(parent, (MitsubishiMxComponentDevice)device);
                default:
                    throw new NotSupportedException();
            }
        }

        public int Delete(Device device)
        {
            switch (device.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDeviceRepo.Delete((MitsubishiMxComponentDevice)device);
                default:
                    throw new NotSupportedException();
            }
        }

        public IEnumerable<Device> GetAll()
        {
            var drivers = new List<Device>();
            drivers.AddRange(MitsubishiMxComponentDeviceRepo.GetAll());

            return drivers;
        }

        public IEnumerable<Device> GetAll(Contracts.Driver parent)
        {
            var drivers = new List<Device>();
            drivers.AddRange(MitsubishiMxComponentDeviceRepo.GetAll(parent));

            return drivers;
        }

        public int Update(Device device)
        {
            switch (device.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDeviceRepo.Update((MitsubishiMxComponentDevice)device);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
