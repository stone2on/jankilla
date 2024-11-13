using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories
{
    public interface IBlockRepository<T> where T : Core.Contracts.Block
    {
        IEnumerable<Core.Contracts.Block> GetAll();
        IEnumerable<Core.Contracts.Block> GetAll(Contracts.Device parent);
        int Add(Contracts.Device parent, T block);
        int Delete(T block);
        int Update(T block);
    }
}
