using Jankilla.Core.Alarms;
using Jankilla.Core.Contracts.Tags;
using Jankilla.Core.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.Test
{
    [TestClass]
    public class _00_CommonTest
    {
        Tag _numericTag;
        Tag _textTag;

        bool bAlarmStatusChanged = false;
        ManualResetEvent _eventWaitHandle;

        [TestInitialize]
        public void Setup()
        {
            _numericTag = new IntTag();
            _numericTag.ID = Guid.NewGuid();
            _numericTag.Name = "No Name";

            _textTag = new StringTag();
            _textTag.ID = Guid.NewGuid();
            _textTag.Name = "No Name";

            _eventWaitHandle = new ManualResetEvent(false);
        }

        private void SetupAlarm(ENumericAlarmCondition condition, double valueA, double? valueB = null)
        {
            var nAlarm = new NumericTagAlarm
            {
                Name = condition.ToString(),
                AlarmMessage = "test message",
                AlarmCondition = condition,
                ValueA = valueA,
            };

            nAlarm.SetTag(_numericTag);

            if (valueB.HasValue)
            {
                nAlarm.ValueB = valueB.Value;
            }

            nAlarm.TagAlarmStatusChanged += Alarm_TagAlarmStatusChanged;
        }

        private void SetupAlarm(ETextAlarmCondition condition, string valueA)
        {
            var tAlarm = new TextTagAlarm
            {
                Name = condition.ToString(),
                AlarmMessage = "test message",
                AlarmCondition = condition,
                ValueA = valueA,
            };

            tAlarm.SetTag(_textTag);

            tAlarm.TagAlarmStatusChanged += Alarm_TagAlarmStatusChanged;
        }

        private void Alarm_TagAlarmStatusChanged(object sender, TagAlarmEventArgs e)
        {
            bAlarmStatusChanged = true;
            _eventWaitHandle.Set();
        }

        private void CAlarm_ComplexAlarmStatusChanged(object sender, ComplexAlarmEventArgs e)
        {
            bAlarmStatusChanged = true;
            _eventWaitHandle.Set();
        }

        [TestMethod]
        public void NumericTagAlarm_Equals_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.Equals, 3);
            _numericTag.ForceWrite(3);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_NotEquals_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.NotEquals, 3);
            _numericTag.ForceWrite(4);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_LessThan_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.LessThan, 3);
            _numericTag.ForceWrite(2);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_LessThanOrEqual_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.LessThanOrEqual, 3);
            _numericTag.ForceWrite(3);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_GreaterThan_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.GreaterThan, 3);
            _numericTag.ForceWrite(4);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_GreaterThanOrEqual_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.GreaterThanOrEqual, 3);
            _numericTag.ForceWrite(3);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void NumericTagAlarm_Between_ShouldFire()
        {
            SetupAlarm(ENumericAlarmCondition.Between, 3, 5);
            _numericTag.ForceWrite(4);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_Equals_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.Equals, "test");
            _textTag.ForceWrite("test");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_NotEquals_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.NotEquals, "test");
            _textTag.ForceWrite("different");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_Contains_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.Contains, "test");
            _textTag.ForceWrite("this is a test");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_NotContains_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.NotContains, "test");
            _textTag.ForceWrite("this is a sample");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_IsBlank_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.IsBlank, string.Empty);
            _textTag.ForceWrite(string.Empty);

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_IsNotBlank_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.IsNotBlank, string.Empty);
            _textTag.ForceWrite("not blank");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_BeginsWith_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.BeginsWith, "start");
            _textTag.ForceWrite("start of the string");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void TextTagAlarm_EndsWith_ShouldFire()
        {
            SetupAlarm(ETextAlarmCondition.EndsWith, "end");
            _textTag.ForceWrite("this is the end");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestMethod]
        public void ComplexTagAlarm_And_ShouldFire()
        {
            var tAlarm = new TextTagAlarm
            {
                Name = "A",
                AlarmMessage = "test message",
                AlarmCondition = ETextAlarmCondition.BeginsWith,
                ValueA = "HELLO",
            };

            tAlarm.SetTag(_textTag);

            var nAlarm = new NumericTagAlarm
            {
                Name = "B",
                AlarmMessage = "test message",
                AlarmCondition = ENumericAlarmCondition.GreaterThan,
                ValueA = 3,
            };

            nAlarm.SetTag(_numericTag);

            var cAlarm = new ComplexAlarm()
            {
                Name = "COMPLEX TEST ALARM",
                AlarmMessage = "HELLO (AND) GREATER THAN 3",
                ComplexAlarmCondition = EComplexAlarmCondition.And
            };

            cAlarm.AddAlarm(tAlarm);
            cAlarm.AddAlarm(nAlarm);

            cAlarm.ComplexAlarmStatusChanged += CAlarm_ComplexAlarmStatusChanged; 

            _numericTag.ForceWrite(4);
            _textTag.ForceWrite("HELLO WORLD!");

            bool eventFired = _eventWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            Assert.IsTrue(eventFired);
            Assert.IsTrue(bAlarmStatusChanged);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _eventWaitHandle.Dispose();
        }
    }
}
