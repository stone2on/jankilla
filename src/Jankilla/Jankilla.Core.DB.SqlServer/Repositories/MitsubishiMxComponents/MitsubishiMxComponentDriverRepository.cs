using Dapper;
using Jankilla.Driver.Mitsubishi.MxComponent;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.DB.SqlServer.Repositories
{
    class MitsubishiMxComponentDriverRepository : IDriverRepository<MitsubishiMxComponentDriver>
    {
        public string ConnectionString { get; set; }
        public MitsubishiMxComponentDriverRepository(string connString)
        {
            ConnectionString = connString;
        }
        public IEnumerable<Core.Contracts.Driver> GetAll()
        {
            var drivers = new List<MitsubishiMxComponentDriver>();

            var sql = "SELECT d.* FROM Drivers d LEFT JOIN MitsubishiMxComponentDrivers mx ON d.ID = mx.ID";
            using (var connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                drivers.AddRange(connection.Query<MitsubishiMxComponentDriver>(sql));

            }

            return drivers;

        }

        public int Add(MitsubishiMxComponentDriver driver)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                int result = 0;


                connection.Open();

                result += connection.Execute("INSERT INTO Drivers (ID,Name,Path,Description,Discriminator) VALUES (@ID,@Name,@Path,@Description,@Discriminator);",
                    driver
                    );

                result += connection.Execute("INSERT INTO MitsubishiMxComponentDrivers (ID) VALUES (@ID);",
                    driver
                    );



                return result;
            }
        }

        public int Delete(MitsubishiMxComponentDriver driver)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                int result = 0;

                connection.Open();
                result += connection.Execute("DELETE FROM MitsubishiMxComponentDrivers WHERE ID = @ID;",
                    driver
                    );

                result += connection.Execute("DELETE FROM Drivers WHERE ID = @ID;",
                    driver
                    );

                return result;
            }
        }

        public int Update(MitsubishiMxComponentDriver driver)
        {
            var sql = "UPDATE Drivers SET Name = @Name, Description = @Description, Path = @Path, Discriminator = @Discriminator WHERE ID = @ID";
            using (var connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                var result = connection.Execute(sql, driver);
                return result;

            }
        }
    }
}
