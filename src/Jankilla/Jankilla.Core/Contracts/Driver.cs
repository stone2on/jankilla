using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Jankilla.Core.Contracts
{
    public abstract class Driver : BaseContract
    {

        #region Public Properties
        public abstract IReadOnlyList<Device> Devices { get; }

        #endregion

        #region Fields


        #endregion

        #region Constructor

   
        #endregion

        #region Public Methods

        public abstract bool ValidateDevice(Device device);

        public abstract bool AddDevice(Device device);

        public abstract bool RemoveDevice(Device device);

        public abstract void RemoveAllDevices();

        public abstract void Start(int tick);

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Name} / {Discriminator}";
        }

        public override bool Equals(object obj)
        {
            return obj is Driver driver &&
                   ID.Equals(driver.ID) &&
                   Name == driver.Name &&
                   Path == driver.Path &&
                   Description == driver.Description &&
                   Discriminator == driver.Discriminator;
        }

        public override int GetHashCode()
        {
            int hashCode = 2040496800;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Discriminator.GetHashCode();
            return hashCode;
        }



        #endregion

    }
}
