using Jankilla.Core.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converter
{
    public static class JSONProjectHelper
    {
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            },
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>()
            {
                new JSONDriverConverter(),
                new JSONDeviceConverter(),
                new JSONBlockConverter(),
                new JSONTagConverter()
            }
        };



        public static Project OpenProjectFile(string path)
        {
            Debug.Assert(path != null);
            var dat = File.ReadAllText(path);

            Project project = JsonConvert.DeserializeObject<Project>(dat, _jsonSerializerSettings);
            return project;
        }

        public static void SaveProjectFile(string path, Project project)
        {
            Debug.Assert(path != null);

            var text = JsonConvert.SerializeObject(project, _jsonSerializerSettings);
            File.WriteAllText(path, text);
        }
    }
}
