using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.Repositories
{
    public interface ITagRepository<T> where T : Tag
    {
        IEnumerable<Tag> GetAll();
        IEnumerable<Tag> GetAll(Contracts.Block parent);
        int Add(Contracts.Block parent, T tag);
        int Delete(T tag);
        int Update(T tag);
    }
}
