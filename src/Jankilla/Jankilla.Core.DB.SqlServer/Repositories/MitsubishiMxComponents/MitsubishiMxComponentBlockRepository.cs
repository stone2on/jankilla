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
    class MitsubishiMxComponentBlockRepository : IBlockRepository<MitsubishiMxComponentBlock>
    {
        public string ConnectionString { get; set; }
        public MitsubishiMxComponentBlockRepository(string connString)
        {
            ConnectionString = connString;
        }

        public int Add(Device parent, MitsubishiMxComponentBlock block)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute($@"INSERT INTO Blocks (ID,Name,Path,Description,Discriminator,StartAddress,BufferSize,DeviceID) 
                                                        VALUES (@ID,@Name,@Path,@Description,@Discriminator,@StartAddress,@BufferSize,'{parent.ID}');",
                    block);

                result += connection.Execute("INSERT INTO MitsubishiMxComponentBlocks (ID, StationNo) VALUES (@ID, @StationNo);",
                    block);

                return result;
            }
        }

        public int Delete(MitsubishiMxComponentBlock block)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("DELETE FROM MitsubishiMxComponentBlocks WHERE ID = @ID;",
                    block
                    );

                result += connection.Execute("DELETE FROM Blocks WHERE ID = @ID;",
                    block
                    );

                return result;
            }
        }

        public IEnumerable<Block> GetAll()
        {
            var blocks = new List<MitsubishiMxComponentBlock>();

            var sql = $"SELECT d.*, mx.StationNo FROM Blocks d LEFT JOIN MitsubishiMxComponentBlocks mx ON d.ID = mx.ID  WHERE d.Discriminator = {(int)0/*EDriverDiscriminator.MitsubishiMxComponent*/}";
            using (var connection = new SqlConnection(ConnectionString))
            {   
                connection.Open();
                blocks.AddRange(connection.Query<MitsubishiMxComponentBlock>(sql));
            }

            return blocks;
        }

        public IEnumerable<Block> GetAll(Device parent)
        {
            var blocks = new List<MitsubishiMxComponentBlock>();

            var sql = $"SELECT d.*, mx.StationNo FROM Blocks d LEFT JOIN MitsubishiMxComponentBlocks mx ON d.ID = mx.ID  WHERE d.Discriminator = {(int)0/*EDriverDiscriminator.MitsubishiMxComponent*/} AND d.DeviceID = '{parent.ID}'";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                blocks.AddRange(connection.Query<MitsubishiMxComponentBlock>(sql));
            }

            return blocks;
        }

        public int Update(MitsubishiMxComponentBlock block)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int result = 0;

                result += connection.Execute("UPDATE Blocks SET Name = @Name, Description = @Description, Path = @Path, Discriminator = @Discriminator WHERE ID = @ID",
                    block);

                result += connection.Execute("UPDATE MitsubishiMxComponentBlocks SET StationNo = @StationNo WHERE ID = @ID;",
                    block);

                return result;
            }
        }
    }
}
