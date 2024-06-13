using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories
{
    public interface IDriverRepository<T> where T : Core.Contracts.Driver
    {
        IEnumerable<Core.Contracts.Driver> GetAll();

        int Add(T driver);
        int Delete(T driver);
        int Update(T driver);
    }
}
