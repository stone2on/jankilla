using Jankilla.Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Alarms
{
    public abstract class BaseAlarm : IIdentifiable, ICloneable 
    {
        public Guid ID { get; set; }
        public Guid? Parent { get; set; }
        public int No { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public abstract string Discriminator { get; }
        public int AlarmLevel { get; set; }     
        public string AlarmMessage { get; set; }
        [JsonIgnore]
        public bool IsOn { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
