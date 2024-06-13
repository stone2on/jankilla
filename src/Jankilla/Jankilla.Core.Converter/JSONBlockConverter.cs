using Jankilla.Core.Contracts;
using Jankilla.Core.Utils;
using Jankilla.Driver.MitsubishiMxComponent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converter
{
    public class JSONBlockConverter : CustomCreationConverter<Block>
    {
        private EDriverDiscriminator _discriminator;
        private Guid _id;
        private string _name;
        private int _stationNo;
        private string _startAddress;
        private int _bufferSize;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            _discriminator = jobj["discriminator"].ToObject<EDriverDiscriminator>();
            _id = jobj["id"].ToObject<Guid>();
            _name = jobj["name"].ToObject<string>();
            _stationNo = jobj["stationNo"].ToObject<int>();
            _startAddress = jobj["startAddress"].ToObject<string>();
            _bufferSize = jobj["bufferSize"].ToObject<int>();
            return base.ReadJson(jobj.CreateReader(), objectType, existingValue, serializer);
        }

        public override Block Create(Type objectType)
        {
            switch (_discriminator)
            {
                case EDriverDiscriminator.MitsubishiMxComponent:
                    return new MitsubishiMxComponentBlock(_name, _stationNo, _startAddress, _bufferSize) { ID = _id };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
