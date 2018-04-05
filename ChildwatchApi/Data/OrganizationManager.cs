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
            OpenConnection("p_location_get");
            AddParameters(new SqlParameter("branch_id", i));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                locations.Add(new Location()
                {
                    BranchId = i,
                    Name = (string)reader["location_name"],
                    Id = (int)reader["location_id"]
                });
            }
            return locations.ToArray();
        }
    }
}
