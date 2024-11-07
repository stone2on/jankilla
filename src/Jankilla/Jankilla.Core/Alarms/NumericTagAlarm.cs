using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public class NumericTagAlarm : TagAlarm
    {
        public override event EventHandler<TagAlarmEventArgs> TagAlarmStatusChanged;

        public override string Discriminator => nameof(NumericTagAlarm);
        public ENumericAlarmCondition AlarmCondition { get; set; }
        public double ValueA { get; set; }
        public double ValueB { get; set; }
        public double Deadband { get; set; } = 0.0001;

        internal override void OnTagPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Tag tag = sender as Tag;

            Debug.Assert(tag != null);

            double val = (double)tag.CalibratedValue;

            double lowerBound = ValueA - Deadband;
            double upperBound = ValueA + Deadband;

            string alarmMessage = string.Empty;
            bool bDetected = false;
            switch (AlarmCondition)
            {
                case ENumericAlarmCondition.Equals:
                    bDetected = val >= lowerBound && val <= upperBound;
                    break;
                case ENumericAlarmCondition.NotEquals:
                    bDetected = val < lowerBound || val > upperBound;
                    break;
                case ENumericAlarmCondition.LessThan:
                    bDetected = val < lowerBound;
                    break;
                case ENumericAlarmCondition.LessThanOrEqual:
                    bDetected = val <= upperBound;
                    break;
                case ENumericAlarmCondition.GreaterThan:
                    bDetected = val > upperBound;
                    break;
                case ENumericAlarmCondition.GreaterThanOrEqual:
                    bDetected = val >= lowerBound;
                    break;
                case ENumericAlarmCondition.Between:
                    //double lowerBoundB = ValueB - Deadband;
                    double upperBoundB = ValueB + Deadband;
                    bDetected = (val >= lowerBound) && (val <= upperBoundB);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            if (bDetected != IsOn)  // 상태가 변경되었을 때만 처리
            {
                IsOn = bDetected;
                TagAlarmStatusChanged?.Invoke(this, new TagAlarmEventArgs()
                {
                    Tag = tag,
                    Time = DateTime.Now,
                    IsOn = bDetected,
                });
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    TagAlarmStatusChanged = null;
                }

                _disposedValue = true;
            }
        }

       
    }
}
