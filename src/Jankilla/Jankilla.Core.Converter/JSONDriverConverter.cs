using Jankilla.Core.Contracts;
using Jankilla.Core.Utils;
using Jankilla.Driver.MitsubishiMxComponent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converter
{
    public class JSONDriverConverter : CustomCreationConverter<Core.Contracts.Driver>
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

        public override Core.Contracts.Driver Create(Type objectType)
        {
            #region Load From Assembly

            //string discriminator = _discriminator.ToString();

            //var fileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            //var fileName = fileNames.FirstOrDefault(a => a.Contains(discriminator));

            //if (fileName == null)
            //{
            //    throw new DllNotFoundException();
            //}

            //Assembly assembly = Assembly.LoadFrom(fileName);

            //var type = assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(Core.Contracts.Driver)));

            //return (Core.Contracts.Driver)Activator.CreateInstance(type, _id);

            #endregion

            switch (_discriminator)
            {
                case EDriverDiscriminator.MitsubishiMxComponent:
                    return new MitsubishiMxComponentDriver() { };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
