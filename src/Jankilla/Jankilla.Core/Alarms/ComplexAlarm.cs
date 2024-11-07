using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public class ComplexAlarm : BaseAlarm, IDisposable
    {
        #region Event Handlers

        public event EventHandler<ComplexAlarmEventArgs> ComplexAlarmStatusChanged;

        #endregion

        #region Public Properties

        public EComplexAlarmCondition ComplexAlarmCondition { get; set; }

        public override string Discriminator => nameof(ComplexAlarm);

        public IReadOnlyList<BaseAlarm> SubAlarms => _subAlarms;

        #endregion

        #region Fields

        protected ObservableCollection<BaseAlarm> _subAlarms = new ObservableCollection<BaseAlarm>();
        private bool disposedValue;


        #endregion

        #region Constructor

        public ComplexAlarm()
        {
            _subAlarms.CollectionChanged += subAlarms_CollectionChanged;
        }

        #endregion

        #region Public Methods

        public void AddAlarm(BaseAlarm alarm)
        {
            _subAlarms.Add(alarm);
        }

        public bool RemoveAlarm(BaseAlarm alarm)
        {
            if (_subAlarms.Remove(alarm))
            {
                return true;
            }
            return false;
        }

        public void RemoveAllAlarms()
        {
            _subAlarms.Clear();
        }

        #endregion

        #region Overrides

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ComplexAlarmStatusChanged = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion

        #region Private Helpers

        private void subAlarms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (TagAlarm alarm in e.NewItems)
                        {
                            alarm.Parent = ID;
                            alarm.TagAlarmStatusChanged += subAlarm_TagAlarmStatusChanged;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null)
                    {
                        foreach (TagAlarm alarm in e.OldItems)
                        {

                            alarm.TagAlarmStatusChanged -= subAlarm_TagAlarmStatusChanged;
                        }
                    }
                    if (e.NewItems != null)
                    {
                        foreach (TagAlarm alarm in e.NewItems)
                        {
                            alarm.Parent = ID;
                            alarm.TagAlarmStatusChanged += subAlarm_TagAlarmStatusChanged;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (TagAlarm alarm in e.OldItems)
                        {
                            alarm.TagAlarmStatusChanged -= subAlarm_TagAlarmStatusChanged;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    foreach (TagAlarm alarm in _subAlarms)
                    {
                        alarm.TagAlarmStatusChanged -= subAlarm_TagAlarmStatusChanged;
                    }
                    break;
                default:
                    break;

            }
        }

        private void subAlarm_TagAlarmStatusChanged(object sender, TagAlarmEventArgs e)
        {
            bool bDetected = false;
            switch (ComplexAlarmCondition)
            {
                case EComplexAlarmCondition.And:
                    {
                        foreach (var subAlarm in _subAlarms)
                        {
                            bDetected = subAlarm.IsOn;
                            if (bDetected == false)
                            {
                                break;
                            }
                        }
                        break;
                    }
                case EComplexAlarmCondition.Or:
                    {
                        foreach (var subAlarm in _subAlarms)
                        {
                            bDetected = subAlarm.IsOn;
                            if (bDetected)
                            {
                                break;
                            }
                        }
                        break;
                    }
                default:
                    break;
            }

            if (bDetected != IsOn)  // 상태가 변경되었을 때만 처리
            {
                IsOn = bDetected;

                ComplexAlarmStatusChanged?.Invoke(this, new ComplexAlarmEventArgs()
                {
                    Time = DateTime.Now,
                    SubAlarms = _subAlarms,
                    IsOn = bDetected
                });
            }
        }

        #endregion

    }
}
