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
using System.Reflection;
using Jankilla.Core.Utils;
using Jankilla.Core.Alarms;
using Jankilla.Core.Tags;

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
            AssemblyHelper.LoadDlls();

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

            JsonSubtypesConverterBuilder drvBuilder = JsonSubtypesConverterBuilder.Of<Driver>("discriminator");
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

            JsonSubtypesConverterBuilder dvcBuilder = JsonSubtypesConverterBuilder.Of<Device>("discriminator");
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

            JsonSubtypesConverterBuilder blkBuilder = JsonSubtypesConverterBuilder.Of<Block>("discriminator");
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

            JsonSubtypesConverterBuilder tagBuilder = JsonSubtypesConverterBuilder.Of<Tag>("discriminator");
            foreach (var type in tagTypes)
            {
                if (type.IsGenericTypeDefinition)
                {
                    // 제네릭 형식 정의를 구체적인 형식으로 변환
                    Type[] genericArguments = tagTypes.Where(tt => !tt.IsGenericTypeDefinition).ToArray();
                    foreach (var arg in genericArguments)
                    {
                        var constructedType = type.MakeGenericType(arg);
                        var tag = (Tag)ObjectResolver.Current.Resolve(constructedType);
                        //tagBuilder.RegisterSubtype(constructedType, $"{tag.Discriminator}<{arg.Name}>");
                    }
                }
                else
                {
                    var tag = (Tag)ObjectResolver.Current.Resolve(type);
                    tagBuilder.RegisterSubtype(type, tag.Discriminator);
                }
            }

            _jsonSerializerSettings.Converters.Add(tagBuilder.Build());

            //var alarmType = typeof(TagAlarm);
            //var alarmTypes = assemblies
            //    .SelectMany(assembly => assembly.GetTypes())
            //    .Where(type => alarmType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            //JsonSubtypesConverterBuilder alarmBuilder = JsonSubtypesConverterBuilder.Of<TagAlarm>("Discriminator");
            //foreach (var type in alarmTypes)
            //{
            //    var alarm = (TagAlarm)ObjectResolver.Current.Resolve(type);
            //    alarmBuilder.RegisterSubtype(type, alarm.Discriminator);
            //}

            //_jsonSerializerSettings.Converters.Add(alarmBuilder.Build());

            JsonSubtypesConverterBuilder alarmBuilder = JsonSubtypesConverterBuilder.Of<BaseAlarm>("discriminator");
            alarmBuilder.RegisterSubtype(typeof(ComplexAlarm), nameof(ComplexAlarm));
            alarmBuilder.RegisterSubtype(typeof(NumericTagAlarm), nameof(NumericTagAlarm));
            alarmBuilder.RegisterSubtype(typeof(TextTagAlarm), nameof(TextTagAlarm));

            _jsonSerializerSettings.Converters.Add(alarmBuilder.Build());
        }

        public Project OpenProjectFile(string path)
        {
            Debug.Assert(path != null);
            try
            {
                var dat = File.ReadAllText(path);
                Project project = JsonConvert.DeserializeObject<Project>(dat, _jsonSerializerSettings);

                if (project == null)
                {
                    return null;
                }

                var processor = new AlarmProcessor(project);
                processor.ProcessAlarms();

                return project;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
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
