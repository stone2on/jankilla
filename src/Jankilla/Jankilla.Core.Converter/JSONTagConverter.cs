using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Tags.Base;
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
    public class JSONTagConverter : CustomCreationConverter<Tag>
    {
        private ETagDiscriminator _discriminator;
        private Guid _id;
        private string _name;
        private string _address;
        private EDirection _direction;
        private int _bitIndex;
        private int _byteSize;


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            _discriminator = jobj["discriminator"].ToObject<ETagDiscriminator>();

            _id = jobj["id"].ToObject<Guid>();
            _name = jobj["name"].ToObject<string>();
            _address = jobj["address"].ToObject<string>();
            _direction = jobj["direction"].ToObject<EDirection>();

            if (_discriminator == ETagDiscriminator.Boolean)
            {
                _bitIndex = jobj["bitIndex"].ToObject<int>();
            }

            if (_discriminator == ETagDiscriminator.String)
            {
                _byteSize = jobj["byteSize"].ToObject<int>();
            }

            return base.ReadJson(jobj.CreateReader(), objectType, existingValue, serializer);
        }


        public override Tag Create(Type objectType)
        {
            switch (_discriminator)
            {
                case ETagDiscriminator.Boolean:
                    return new BooleanTag(_name, _address, _direction, _bitIndex);
                case ETagDiscriminator.Int:
                    return new IntTag(_name, _address, _direction);
                case ETagDiscriminator.Short:
                    return new ShortTag(_name, _address, _direction);
                case ETagDiscriminator.String:
                    return new StringTag(_name, _address, _direction, _byteSize);
                case ETagDiscriminator.Float:
                    return new FloatTag(_name, _address, _direction);
                default:
                    throw new NotImplementedException();

            }
        }
    }

}
