using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data
{
    public class OrganizationManager : IDatabaseManager
    {
        public OrganizationManager(SqlConnection connector) : base(connector) { }

        public Location[] GetLocations(int i)
        {
            List<Location> locations = new List<Location>();
            SqlDataReader reader = GetSqlDataReader("p_location_get", new SqlParameter[]{});
            try
            {
                while (reader.Read())
                {
                    locations.Add(new Location()
                    {
                        BranchId = i,
                        Name = (string)reader["loc_name"],
                        Id = (int)reader["loc_id"]
                    });
                }
            }
            finally
            {
                CloseReader(reader);
                CloseConnection();
            }

            return locations.ToArray();
        }
    }
}
