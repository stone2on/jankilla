using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Tags
{
    public interface IArrayTag
    {
        IEnumerable<Tag> Tags { get; }
        void SetTags(IEnumerable<Tag> tags);
    }
}
