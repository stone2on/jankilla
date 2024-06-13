using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts
{
    public sealed class Project
    {
        //private HashSet<Driver> _driverSet = new HashSet<Driver>();
        private List<Driver> _drivers = new List<Driver>();
        public IReadOnlyList<Driver> Drivers 
        {
            get { return _drivers; }
        } 
        
        public bool AddDriver(Driver driver)
        {
            if (_drivers.Contains(driver))
            {
                return false;
            }

            _drivers.Add(driver);
            return true;
        }

        public bool RemoveDriver(Driver driver)
        {
            return _drivers.Remove(driver);
        }

    
    }
}
