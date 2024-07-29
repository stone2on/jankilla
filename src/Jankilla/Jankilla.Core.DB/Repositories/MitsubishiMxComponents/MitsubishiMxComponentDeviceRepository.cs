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

namespace Jankilla.Core.DB.Repositories
{
    class MitsubishiMxComponentDeviceRepository : IDeviceRepository<MitsubishiMxComponentDevice>
    {
        public string ConnectionString { get; set; }
        public MitsubishiMxComponentDeviceRepository(string connString)
        {
            ConnectionString = connString;
        }

        public IEnumerable<Device> GetAll()
        {
            var devices = new List<MitsubishiMxComponentDevice>();

            var sql = $"SELECT d.* FROM Devices d LEFT JOIN MitsubishiMxComponentDevices mx ON d.ID = mx.ID WHERE d.Discriminator = {(int)0}";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                devices.AddRange(connection.Query<MitsubishiMxComponentDevice>(sql));
            }

            return devices;
        }

        public IEnumerable<Device> GetAll(Core.Contracts.Driver parent)
        {
            var devices = new List<MitsubishiMxComponentDevice>();

            var sql = $"SELECT d.* FROM Devices d LEFT JOIN MitsubishiMxComponentDevices mx ON d.ID = mx.ID WHERE d.Discriminator = {(int)0} AND d.DeviceID = '{parent.ID}'";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                devices.AddRange(connection.Query<MitsubishiMxComponentDevice>(sql));
            }

            return devices;
        }

        public int Add(Contracts.Driver parent, MitsubishiMxComponentDevice device)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute($"INSERT INTO Devices (ID,Name,Path,Description,Discriminator,DriverID) VALUES (@ID,@Name,@Path,@Description,@Discriminator,'{parent.ID}');",
                    device
                    );

                result += connection.Execute("INSERT INTO MitsubishiMxComponentDevices (ID) VALUES (@ID);",
                    device
                    );

                return result;
            }
        }

        public int Delete(MitsubishiMxComponentDevice device)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("DELETE FROM MitsubishiMxComponentDevices WHERE ID = @ID;",
                    device
                    );

                result += connection.Execute("DELETE FROM Devices WHERE ID = @ID;",
                    device
                    );

                return result;
            }
        }

        public int Update(MitsubishiMxComponentDevice device)
        {
            var sql = "UPDATE Devices SET Name = @Name, Description = @Description, Path = @Path, Discriminator = @Discriminator WHERE ID = @ID";
            using (var connection = new SqlConnection(ConnectionString))
            {
              
                connection.Open();
                var result = connection.Execute(sql, device);
                return result;
              
            }
        }
    }

}
