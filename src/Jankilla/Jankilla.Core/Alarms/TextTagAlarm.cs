using Jankilla.Core.Contracts.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public class TextTagAlarm : TagAlarm
    {
        #region Event Handlers

        public override event EventHandler<TagAlarmEventArgs> TagAlarmStatusChanged;

        #endregion

        #region Public Properties

        public override string Discriminator => nameof(TextTagAlarm);
        public ETextAlarmCondition AlarmCondition { get; set; }
        public string ValueA { get; set; }

        #endregion

        #region Overrides

        internal override void OnTagPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Tag tag = sender as Tag;

            Debug.Assert(tag != null);

            string val = (string)tag.CalibratedValue;

            string alarmMessage = string.Empty;
            bool bDetected = false;
            switch (AlarmCondition)
            {
                case ETextAlarmCondition.Equals:
                    bDetected = val == ValueA;
                    break;
                case ETextAlarmCondition.NotEquals:
                    bDetected = val != ValueA;
                    break;
                case ETextAlarmCondition.Contains:
                    bDetected = val.Contains(ValueA);
                    break;
                case ETextAlarmCondition.NotContains:
                    bDetected = !val.Contains(ValueA);
                    break;
                case ETextAlarmCondition.IsBlank:
                    bDetected = string.IsNullOrEmpty(val);
                    break;
                case ETextAlarmCondition.IsNotBlank:
                    bDetected = !string.IsNullOrEmpty(val);
                    break;
                case ETextAlarmCondition.BeginsWith:
                    bDetected = val.StartsWith(ValueA);
                    break;
                case ETextAlarmCondition.EndsWith:
                    bDetected = val.EndsWith(ValueA);
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
                    IsOn = bDetected
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

        #endregion
    }
}
