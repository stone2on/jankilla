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
    public class JSONDeviceConverter : CustomCreationConverter<Device>
    {
        private EDriverDiscriminator _discriminator;
        private Guid _id;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JToken.ReadFrom(reader);
            _discriminator = jobj["discriminator"].ToObject<EDriverDiscriminator>();
            _id = jobj["id"].ToObject<Guid>();
            return base.ReadJson(jobj.CreateReader(), objectType, existingValue, serializer);
        }

        public override Device Create(Type objectType)
        {
            switch (_discriminator)
            {
                case EDriverDiscriminator.MitsubishiMxComponent:
                    return new MitsubishiMxComponentDevice() { ID = _id };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
