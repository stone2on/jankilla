using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories
{
    public interface IDeviceRepository<T> where T : Core.Contracts.Device
    {
        IEnumerable<Core.Contracts.Device> GetAll();
        IEnumerable<Core.Contracts.Device> GetAll(Contracts.Driver parent);
        int Add(Contracts.Driver parent, T device);
        int Delete(T device);
        int Update(T device);
    }
}
