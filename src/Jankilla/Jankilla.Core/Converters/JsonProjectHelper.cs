using Jankilla.Core.Contracts;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Tags.Base;
using JsonSubTypes;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace Jankilla.Core.Converters
{
    public class JsonProjectHelper 
    {
        private static JsonProjectHelper _instance;
        public static JsonProjectHelper Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = new JsonProjectHelper();
                }

                return _instance;
            } 
        }

        private JsonSerializerSettings _jsonSerializerSettings;

        private JsonProjectHelper()
        {
            _jsonSerializerSettings = new JsonSerializerSettings()
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
            };

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var driverType = typeof(Driver);
            var drvTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => driverType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            JsonSubtypesConverterBuilder drvBuilder = JsonSubtypesConverterBuilder.Of<Driver>("Discriminator");
            foreach (var type in drvTypes)
            {
                var driver = (Driver)ObjectResolver.Current.Resolve(type);
                drvBuilder.RegisterSubtype(type, driver.Discriminator);
            }

            _jsonSerializerSettings.Converters.Add(drvBuilder.Build());
 

            var deviceType = typeof(Device);
            var dvcTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => deviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            JsonSubtypesConverterBuilder dvcBuilder = JsonSubtypesConverterBuilder.Of<Device>("Discriminator");
            foreach (var type in dvcTypes)
            {
                var device = (Device)ObjectResolver.Current.Resolve(type);
                dvcBuilder.RegisterSubtype(type, device.Discriminator);
            }

            _jsonSerializerSettings.Converters.Add(dvcBuilder.Build());

            var blockType = typeof(Block);
            var blkTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => blockType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            JsonSubtypesConverterBuilder blkBuilder = JsonSubtypesConverterBuilder.Of<Block>("Discriminator");
            foreach (var type in blkTypes)
            {
                var block = (Block)ObjectResolver.Current.Resolve(type);
                blkBuilder.RegisterSubtype(type, block.Discriminator);
            }

            _jsonSerializerSettings.Converters.Add(blkBuilder.Build());

            var tagType = typeof(Tag);
            var tagTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => tagType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            JsonSubtypesConverterBuilder tagBuilder = JsonSubtypesConverterBuilder.Of<Tag>("Discriminator");
            foreach (var type in tagTypes)
            {
                var tag = (Tag)ObjectResolver.Current.Resolve(type);
                tagBuilder.RegisterSubtype(type, tag.Discriminator);
            }

            _jsonSerializerSettings.Converters.Add(tagBuilder.Build());
        }

        public Project OpenProjectFile(string path)
        {
            Debug.Assert(path != null);
            Project project = null;
            try
            {
                var dat = File.ReadAllText(path);
                project = JsonConvert.DeserializeObject<Project>(dat, _jsonSerializerSettings);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return project;
        }

        public void SaveProjectFile(string path, Project project)
        {
            Debug.Assert(path != null);

            try
            {
                var text = JsonConvert.SerializeObject(project, _jsonSerializerSettings);
                File.WriteAllText(path, text);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
