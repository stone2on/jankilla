using Dapper;
using Jankilla.Core.Contracts;
using Jankilla.Driver.MitsubishiMxComponent;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories
{
    class DriverRepository 
    {
        public string ConnectionString { get; set; }

        private MitsubishiMxComponentDriverRepository MitsubishiMxComponentDriverRepo { get; set; } 

        public DriverRepository(string connString)
        {
            ConnectionString = connString;

            MitsubishiMxComponentDriverRepo = new MitsubishiMxComponentDriverRepository(connString);
        }

        public int Add(Core.Contracts.Driver driver)
        {
            switch (driver.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDriverRepo.Add((MitsubishiMxComponentDriver)driver);
                default:
                    throw new NotSupportedException();
            }
        }

        public int Delete(Core.Contracts.Driver driver)
        {
            switch (driver.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDriverRepo.Delete((MitsubishiMxComponentDriver)driver);
                default:
                    throw new NotSupportedException();
            }
        }
 
        public IEnumerable<Core.Contracts.Driver> GetAll()
        {
            var drivers = new List<Contracts.Driver>();
            drivers.AddRange(MitsubishiMxComponentDriverRepo.GetAll());

            return drivers;
        }

        public int Update(Core.Contracts.Driver driver)
        {
            switch (driver.Discriminator)
            {
                case "MitsubishiMxComponent":
                    return MitsubishiMxComponentDriverRepo.Update((MitsubishiMxComponentDriver)driver);
                default:
                    throw new NotSupportedException();
            }
        }

      
    }

  
}
