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

            _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
               .Of<Tag>("Discriminator")
               .RegisterSubtype<IntTag>(ETagDiscriminator.Int)
               .Build());

            _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<Tag>("Discriminator")
                .RegisterSubtype<ShortTag>(ETagDiscriminator.Short)
                .Build());

            _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<Tag>("Discriminator")
                .RegisterSubtype<StringTag>(ETagDiscriminator.String)
                .Build());

            _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<Tag>("Discriminator")
                .RegisterSubtype<FloatTag>(ETagDiscriminator.Float)
                .Build());

            _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                .Of<Tag>("Discriminator")
                .RegisterSubtype<BooleanTag>(ETagDiscriminator.Boolean)
                .Build());

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var driverType = typeof(Driver);
            var drvTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => driverType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in drvTypes)
            {
                var drv = (Driver)ObjectResolver.Current.Resolve(type);

                _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                    .Of<Driver>("Discriminator")
                    .RegisterSubtype(type, drv.Discriminator)
                    .Build());
            }

            var deviceType = typeof(Device);
            var dvcTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => deviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in dvcTypes)
            {
                var dvc = (Device)ObjectResolver.Current.Resolve(type);

                _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                    .Of<Device>("Discriminator")
                    .RegisterSubtype(type, dvc.Discriminator)
                    .Build());
            }

            var blockType = typeof(Block);
            var blkTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => blockType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in blkTypes)
            {
                var blk = (Block)ObjectResolver.Current.Resolve(type);

                _jsonSerializerSettings.Converters.Add(JsonSubtypesConverterBuilder
                    .Of<Block>("Discriminator")
                    .RegisterSubtype(type, blk.Discriminator)
                    .Build());
            }
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
