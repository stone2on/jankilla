using Jankilla.Core.Contracts.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public abstract class TagAlarm : BaseAlarm, IDisposable
    {
        #region Event Handlers

        public abstract event EventHandler<TagAlarmEventArgs> TagAlarmStatusChanged;

        #endregion

        #region Public Properties
        [JsonProperty]
        public Guid TagID { get; private set; }
        

        #endregion

        #region Fields

        protected bool _disposedValue;

        private Tag _tag;

        #endregion

        #region Public Methods

        public void SetTag(Tag tag)
        {
            Debug.Assert(tag != null);

            if (_tag != null)
            {
                _tag.PropertyChanged -= OnTagPropertyChanged;
            }

            _tag = tag;
            _tag.PropertyChanged += OnTagPropertyChanged;

            TagID = tag.ID;
        }


        #endregion

        #region Private Helpers

        #endregion

        #region Overrides

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(disposing: true);   
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Events

        internal abstract void OnTagPropertyChanged(object sender, PropertyChangedEventArgs e);

        #endregion
    }
}
