using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Converters;
using Jankilla.Core.Tags;
using Jankilla.Driver.Mitsubishi.MxComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jankilla.Test
{
    [TestClass]
    public class _10_MitsubishiMxComponentTest
    {
        Project _project1;

        [TestInitialize]
        public void Setup()
        {
            _project1 = new Project();
            Core.Contracts.Driver mxDriver = new MitsubishiMxComponentDriver();
            _project1.AddDriver(mxDriver);
            mxDriver.Name = "DRV01";
            mxDriver.Path = "DRV01";
            mxDriver.Description = "My Driver";
            mxDriver.ID = Guid.NewGuid();

            var mxDevice = new MitsubishiMxComponentDevice() { ID = Guid.NewGuid() };
            mxDevice.Name = "DV01";

            mxDriver.AddDevice(mxDevice);

            var myBlock = new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 01", StationNo = 1, StartAddress = "D0000", BufferSize = 2000 };
            mxDevice.AddBlock(myBlock);
            mxDevice.AddBlock(new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 02", StationNo = 1, StartAddress = "D1000", BufferSize = 2000 });
            mxDevice.AddBlock(new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 03", StationNo = 1, StartAddress = "D2000", BufferSize = 2000 });

            var bitBlock = new MitsubishiMxComponentBlock { ID = Guid.NewGuid(), Name = "BLOCK 04", StationNo = 1, StartAddress = "M0000", BufferSize = 10 };
            mxDevice.AddBlock(bitBlock);

            int noCount = 0;

            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_001", Address = "D0000", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_002", Address = "D0010", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_003", Address = "D0020", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new StringTag() { Name = "SAMPLE_STR_DATA_004", Address = "D0030", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            var sTag = new StringTag() { Name = "TAG FOR ALARM TEST", Address = "D0040", Direction = EDirection.In, ByteSize = 10, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() };
            myBlock.AddTag(sTag);

            var srtTag = new ShortTag() { Name = "SAMPLE_SRT_DATA_005", Address = "D0100", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() };
            myBlock.AddTag(srtTag);
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_006", Address = "D0101", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_007", Address = "D0102", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_008", Address = "D0103", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_009", Address = "D0110", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_010", Address = "D0112", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_011", Address = "D0114", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_012", Address = "D0116", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new UShortTag() { Name = "SAMPLE_USRT_DATA_013", Address = "D0200", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new LongTag() { Name = "SAMPLE_LONG_DATA_014", Address = "D0300", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ULongTag() { Name = "SAMPLE_ULONG_DATA_015", Address = "D0400", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ComplexTag() { Name = "SAMPLE_COMPLEX_DATA_016", Address = "D0500", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new ArrayTag<BooleanTag> { Name = "SAMPLE_BOOL_ARRAY", Address = "D0600", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<StringTag> { Name = "SAMPLE_STR_ARRAY", Address = "D0700", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<ShortTag> { Name = "SAMPLE_SRT_ARRAY", Address = "D0800", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<IntTag> { Name = "SAMPLE_INT_ARRAY", Address = "D0900", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<UIntTag> { Name = "SAMPLE_UINT_ARRAY", Address = "D1000", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<FloatTag> { Name = "SAMPLE_FLOAT_ARRAY", Address = "D1100", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<DoubleTag> { Name = "SAMPLE_DOUBLE_ARRAY", Address = "D1200", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<UShortTag> { Name = "SAMPLE_USRT_ARRAY", Address = "D1300", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<LongTag> { Name = "SAMPLE_LONG_ARRAY", Address = "D1400", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<ULongTag> { Name = "SAMPLE_ULONG_ARRAY", Address = "D1500", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });
            myBlock.AddTag(new ArrayTag<ComplexTag> { Name = "SAMPLE_COMPLEX_ARRAY", Address = "D1600", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid(), Length = 10 });

            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_017", Address = "M0000", Direction = EDirection.In, BitIndex = 0, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_018", Address = "M0001", Direction = EDirection.In, BitIndex = 1, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_019", Address = "M0002", Direction = EDirection.In, BitIndex = 2, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_020", Address = "M0003", Direction = EDirection.In, BitIndex = 3, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });



            var myAlarm = new TextTagAlarm() { Name = "TTA", ValueA = "HELLO" };
            myAlarm.SetTag(sTag);
            _project1.AddAlarm(myAlarm);

            var a1 = new TextTagAlarm() { ValueA = "ABC", AlarmCondition = ETextAlarmCondition.Equals };
            a1.SetTag(sTag);
            var a2 = new NumericTagAlarm() { ValueA = 3, AlarmCondition = ENumericAlarmCondition.GreaterThan };
            a2.SetTag(srtTag);

            var c1 = new ComplexAlarm() { ID = Guid.NewGuid(), Name = "COMPLEX TEST ALARM" };
            c1.AddAlarm(a1);
            c1.AddAlarm(a2);

            _project1.AddAlarm(c1);

            JsonProjectHelper.Instance.SaveProjectFile("project.json", _project1);
            CsvProjectHelper.Instance.SaveProjectFile("project.csv", _project1);
        }

        [TestMethod]
        public void File_ShouldSaved()
        {
            Assert.IsTrue(File.Exists("project.json"));
            Assert.IsTrue(File.Exists("project.csv"));
        }

        [TestMethod]
        public void Project_ShouldNotNull()
        {
            Project p = JsonProjectHelper.Instance.OpenProjectFile("project.json");
            Assert.IsNotNull(p);

            p = CsvProjectHelper.Instance.OpenProjectFile("project.csv");
            Assert.IsNotNull(p);
        }
    }
}
