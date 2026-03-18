using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Utils
{
    public class PriorityBasedContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            // NOTE:: 프로젝트 정렬 방법
            // 1. id, name 속성이 있을 경우, 가장 처음으로.
            // 2. 배열, 리스트, 딕셔너리 속성의 경우 마지막으로.
            var orderedProperties = properties
                .OrderBy(p => p.PropertyName != "id" && p.PropertyName != "name")
                    .ThenBy(p => p.PropertyType.IsArray
                        || (p.PropertyType.IsGenericType && typeof(IReadOnlyList<>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition()))
                        || (p.PropertyType.IsGenericType && typeof(IReadOnlyDictionary<,>).IsAssignableFrom(p.PropertyType.GetGenericTypeDefinition())))
                .ToList();

            return orderedProperties;
        }
    }
}
