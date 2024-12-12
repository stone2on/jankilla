using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Converters.ClassMaps;
using Jankilla.Core.Tags;
using Jankilla.Core.Utils;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Converters
{
    public class CsvProjectHelper
    {
        private static CsvProjectHelper _instance;
        public static CsvProjectHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CsvProjectHelper();
                }

                return _instance;
            }
        }

        private Dictionary<string, Type> _classTypeMap = new Dictionary<string, Type>();
        private IList<ClassMap> _classMaps = new List<ClassMap>();

        private CsvConfiguration _config;

        private CsvProjectHelper()
        {
            AssemblyHelper.LoadDlls();

            _config = new CsvConfiguration(CultureInfo.CurrentCulture);
            _config.HasHeaderRecord = false;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var driverType = typeof(Driver);

            var drvTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => driverType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in drvTypes)
            {
                var classMap = ObjectResolver.Current.Resolve(typeof(DriverMap<>).MakeGenericType(type)) as ClassMap;
                classMap.Map().Index(0).ConstantFixed(classMap.ClassType.Name);

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                 .Where(p => p.GetMethod != null && p.GetMethod.GetBaseDefinition().DeclaringType == type)
                 .ToList();

                foreach (var prop in props)
                {
                    classMap.Map(type, prop);
                }
                _classMaps.Add(classMap);
            }

            var deviceType = typeof(Device);
            var dvcTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => deviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in dvcTypes)
            {
                var classMap = ObjectResolver.Current.Resolve(typeof(DeviceMap<>).MakeGenericType(type)) as ClassMap;
                classMap.Map().Index(0).ConstantFixed(classMap.ClassType.Name);

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                 .Where(p => p.GetMethod != null && p.GetMethod.GetBaseDefinition().DeclaringType == type)
                 .ToList();

                foreach (var prop in props)
                {
                    classMap.Map(type, prop);
                }

                _classMaps.Add(classMap);
            }

            var blockType = typeof(Block);
            var blkTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => blockType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in blkTypes)
            {
                var classMap = ObjectResolver.Current.Resolve(typeof(BlockMap<>).MakeGenericType(type)) as ClassMap;
                classMap.Map().Index(0).ConstantFixed(classMap.ClassType.Name);

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                  .Where(p => p.GetMethod != null && p.GetMethod.GetBaseDefinition().DeclaringType == type)
                  .ToList();

                foreach (var prop in props)
                {
                    classMap.Map(type, prop);
                }

                _classMaps.Add(classMap);
            }

            var tagType = typeof(Tag);
            var tagTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => tagType.IsAssignableFrom(type) &&
                type.IsClass &&
                !type.IsAbstract &&
                type != typeof(ComplexTag));

            foreach (var type in tagTypes)
            {
                if (type == typeof(ArrayTag<>))
                {
                    var arrayTagTypes = tagTypes.Where(t => !t.IsGenericTypeDefinition);

                    foreach (var arrayTagType in arrayTagTypes)
                    {
                        var constructedType = type.MakeGenericType(arrayTagType);

                        ClassMap classMap = ObjectResolver.Current.Resolve(typeof(TagMap<>).MakeGenericType(constructedType)) as ClassMap;
                        classMap.Map().Index(0).ConstantFixed($"{constructedType.Name}<{constructedType.GetGenericArguments()[0].Name}>");

                        _classMaps.Add(classMap);
                    }

                    var complexArrayType = type.MakeGenericType(typeof(ComplexTag));
                    ClassMap complexClassMap = ObjectResolver.Current.Resolve(typeof(TagMap<>).MakeGenericType(complexArrayType)) as ClassMap;
                    complexClassMap.Map().Index(0).ConstantFixed($"{complexArrayType.Name}<{complexArrayType.GetGenericArguments()[0].Name}>");

                    _classMaps.Add(complexClassMap);
                }
                else
                {
                    ClassMap classMap = ObjectResolver.Current.Resolve(typeof(TagMap<>).MakeGenericType(type)) as ClassMap;
                    classMap.Map().Index(0).ConstantFixed(classMap.ClassType.Name);

                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                      .Where(p => p.GetMethod != null && p.GetMethod.GetBaseDefinition().DeclaringType == type && !p.Name.Contains("Value"))
                      .ToList();

                    foreach (var prop in props)
                    {
                        classMap.Map(type, prop);
                    }

                    _classMaps.Add(classMap);
                }
            }

            var complexTagMap = new ComplexTagMap();
            complexTagMap.Map().Index(0).ConstantFixed(complexTagMap.ClassType.Name);
            _classMaps.Add(complexTagMap);

            var tagAlarmType = typeof(TagAlarm);
            var tagAlarmTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => tagAlarmType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var type in tagAlarmTypes)
            {
                var classMap = ObjectResolver.Current.Resolve(typeof(TagAlarmMap<>).MakeGenericType(type)) as ClassMap;
                classMap.Map().Index(0).ConstantFixed(classMap.ClassType.Name);

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                  .Where(p => p.GetMethod != null && p.GetMethod.GetBaseDefinition().DeclaringType == type)
                  .ToList();

                foreach (var prop in props)
                {
                    classMap.Map(type, prop);
                }

                _classMaps.Add(classMap);
            }

            var complexAlarmMap = new ComplexAlarmMap();
            complexAlarmMap.Map().Index(0).ConstantFixed(nameof(ComplexAlarm));
            _classMaps.Add(complexAlarmMap);

            foreach (ClassMap cm in _classMaps)
            {
                string key = cm.ClassType.Name;
                if (cm.ClassType.Name.StartsWith("ArrayTag"))
                {
                    key += $"<{cm.ClassType.GetGenericArguments()[0].Name}>";
                }

                _classTypeMap.Add(key, cm.ClassType);
            }
        }

        public Project OpenProjectFile(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, _config))
                {
                    foreach (var cm in _classMaps)
                    {
                        csv.Context.RegisterClassMap(cm);
                    }

                    Project project = new Project();

                    Driver driver = null;
                    Device device = null;
                    Block block = null;
                    Tag tag = null;
                    BaseAlarm alarm = null;

                    while (csv.Read())
                    {
                        string classTypeStr = csv.GetField(0);
                        var clsType = _classTypeMap[classTypeStr];
                        var baseClsName = clsType.BaseType.Name.ToString();

                        var record = csv.GetRecord(clsType);

                        switch (baseClsName)
                        {
                            case nameof(Driver):
                                driver = (Driver)record;
                                project.AddDriver(driver);
                                break;
                            case nameof(Device):
                                device = (Device)record;
                                driver?.AddDevice(device);
                                break;
                            case nameof(Block):
                                block = (Block)record;
                                device?.AddBlock(block);
                                break;
                            case nameof(Tag):
                                tag = (Tag)record;
                                block?.AddTag(tag);
                                break;

                            case nameof(TagAlarm):
                                alarm = (TagAlarm)record;
                                string tagIdStr = csv.GetField(TagAlarmMap<TagAlarm>.TAG_ID_INDEX);
                                bool bParsed = Guid.TryParse(tagIdStr, out Guid tagId);
                                if (bParsed)
                                {
                                    var targetTag = project.FindTagOrNull(tagId);
                                    if (targetTag != null)
                                    {
                                        ((TagAlarm)alarm).SetTag(targetTag);
                                        project.AddAlarm(alarm);
                                    }
                                }
                                break;
                            case nameof(BaseAlarm):
                                alarm = (BaseAlarm)record;
                                project.AddAlarm(alarm);
                                break;
                        }

                    }

                    foreach (var drv in project.Drivers)
                    {
                        foreach (var dev in drv.Devices)
                        {
                            foreach (var blk in dev.Blocks)
                            {
                                var tagsToRemove = new List<Tag>();

                                foreach (var tg in blk.Tags)
                                {
                                    if (tg is IArrayTag arrayTag)
                                    {
                                        var subTags = blk.Tags
                                            .Where(t => t.Name.StartsWith(tg.Name + "[") && t.Name.EndsWith("]"));

                                        arrayTag.SetTags(subTags);

                                        // ArrayTag의 서브 태그들을 제거할 목록에 추가
                                        tagsToRemove.AddRange(subTags);
                                    }
                                    else if (tg is ComplexTag complexTag)
                                    {
                                        var subTags = blk.Tags
                                            .Where(t => t.Name.StartsWith(complexTag.Name + "."));

                                        foreach (var subTag in subTags)
                                        {
                                            complexTag.AddTag(subTag);
                                        }

                                        // ComplexTag의 서브 태그들을 제거할 목록에 추가
                                        tagsToRemove.AddRange(subTags);
                                    }
                                }

                                // blk.Tags에서 ArrayTag와 ComplexTag의 서브 태그들을 제거
                                foreach (var tagToRemove in tagsToRemove)
                                {
                                    blk.RemoveTag(tagToRemove);
                                }
                            }
                        }
                    }

                    return project;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void SaveProjectFile(string path, Project project)
        {
            Debug.Assert(path != null);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, _config))
                {
                    //csv.Context.TypeConverterCache.AddConverter<Tag>(new IEnumerableConverter());

                    foreach (var cm in _classMaps)
                    {
                        csv.Context.RegisterClassMap(cm);
                    }

                    foreach (var driver in project.Drivers)
                    {
                        csv.WriteRecord((object)driver);
                        csv.NextRecord();
                        foreach (var device in driver.Devices)
                        {
                            csv.WriteRecord((object)device);
                            csv.NextRecord();
                            foreach (var block in device.Blocks)
                            {
                                csv.WriteRecord((object)block);
                                csv.NextRecord();
                                foreach (var tag in block.Tags)
                                {
                                    if (tag.Discriminator == Tags.Base.ETagDiscriminator.Complex)
                                    {
                                        var cTag = (ComplexTag)tag;
                                        var stack = new Stack<Tag>(cTag.Tags.Values);
                                        while (stack.Count > 0)
                                        {
                                            var subTag = stack.Pop();
                                            if (subTag.Discriminator == Tags.Base.ETagDiscriminator.Complex)
                                            {
                                                ComplexTag cSubTag = (ComplexTag)subTag;
                                                foreach (var item in cSubTag.Tags)
                                                {
                                                    stack.Push(item.Value);
                                                }
                                            }
                                            csv.WriteRecord((object)subTag);
                                            csv.NextRecord();
                                        }
                                    }

                                    csv.WriteRecord((object)tag);
                                    csv.NextRecord();
                                }
                            }
                        }
                    }

                    foreach (var alarm in project.Alarms)
                    {
                        if (alarm.Discriminator == nameof(ComplexAlarm))
                        {
                            var cAlarm = (ComplexAlarm)alarm;
                            var stack = new Stack<BaseAlarm>(cAlarm.SubAlarms);
                            while (stack.Count > 0)
                            {
                                var subAlarm = stack.Pop();
                                if (subAlarm.Discriminator == nameof(ComplexAlarm))
                                {
                                    var cSubAlarm = (ComplexAlarm)subAlarm;
                                    foreach (var item in cSubAlarm.SubAlarms)
                                    {
                                        stack.Push(item);
                                    }
                                }
                                csv.WriteRecord((object)subAlarm);
                                csv.NextRecord();
                            }
                        }

                        csv.WriteRecord((object)alarm);
                        csv.NextRecord();
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
