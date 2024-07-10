using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jankilla.Core.Contracts.Tags.Base;
using Jankilla.Core.Contracts.Tags;
using JsonSubTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Jankilla.Core.Contracts;
using Jankilla.Core.Converters;
using System.Diagnostics;
using System.IO;

namespace Jankilla.Driver.MitsubishiMxComponent.Test
{
    [TestClass]
    public class DriverTest
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

            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_005", Address = "D0100", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_006", Address = "D0101", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_007", Address = "D0102", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new ShortTag() { Name = "SAMPLE_SRT_DATA_008", Address = "D0103", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_009", Address = "D0110", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_010", Address = "D0112", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_011", Address = "D0114", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            myBlock.AddTag(new IntTag() { Name = "SAMPLE_INT_DATA_012", Address = "D0116", Direction = EDirection.In, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_013", Address = "M0000", Direction = EDirection.In, BitIndex = 0, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_014", Address = "M0001", Direction = EDirection.In, BitIndex = 1, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_015", Address = "M0002", Direction = EDirection.In, BitIndex = 2, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });
            bitBlock.AddTag(new BooleanTag() { Name = "SAMPLE_BOOL_DATA_016", Address = "M0003", Direction = EDirection.In, BitIndex = 3, No = ++noCount, Category = "CDAT01", ID = Guid.NewGuid() });

            JsonProjectHelper.Instance.SaveProjectFile("project.json", _project1);
            CsvProjectHelper.Instance.SaveProjectFile("project.csv", _project1);
        }

        [TestMethod]
        public void Driver_ShouldSerializeJson()
        {
            Assert.AreEqual(true, File.Exists("project.json"));
        }

        [TestMethod]
        public void Driver_ShouldDeserializeJson()
        {
          
        }

        [TestMethod]
        public void Driver_ShouldSerializeCsv()
        {
            Assert.AreEqual(true, File.Exists("project.csv"));
        }

        [TestMethod]
        public void Driver_ShouldDeserializeCsv()
        {
           

        }
    }
}
