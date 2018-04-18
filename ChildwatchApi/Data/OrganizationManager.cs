using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildWatchApi.Entity;

namespace ChildWatchApi.Data
{
    public class OrganizationManager : IDatabaseManager
    {
        public OrganizationManager(SqlConnection connector) : base(connector) { }

        public OrganizationManager(string s) : base(s) { }

        public Location[] GetLocations()
        {
            List<Location> locations = new List<Location>();
            SqlDataReader reader = GetSqlDataReader("p_location_get", new SqlParameter[]{});
            try
            {
                while (reader.Read())
                {
                    locations.Add(new Location()
                    {
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

        public Employee GetEmployee(int id)
        {
            foreach(Employee e in GetEmployees())
            {
                if(e.ID == id)
                {
                    return e;
                }
            }
            return null;
        }
        public bool SaveEmployee(Employee e)
        {
            return RunCommand("p_employee_modify", new SqlParameter[]
            {
                new SqlParameter("id", e.ID),
                new SqlParameter("firstname", e.FirstName),
                new SqlParameter("lastname", e.LastName),
                new SqlParameter("blocked", e.Blocked),
                new SqlParameter("reset", e.NeedsReset),
                new SqlParameter("admin", e.Administrator)
            });
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> list = new List<Employee>();
            DataTable t = GetDataTable("p_employee_get", new SqlParameter[] { });
            foreach(DataRow row in t.Rows)
            {
                Employee e = new Employee()
                {
                    FirstName = (string)row["emp_firstname"],
                    LastName = (string)row["emp_lastname"],
                    ID = (int)row["emp_id"],
                    UserName = (string)row["emp_username"]
                };

                list.Add(e);
            }
            return list;
        }


        public bool RegisterEmployee(string firstname, string lastname, bool admin, string username, string password)
        {
            return RunCommand("p_employee_insert", new SqlParameter[]
            {
                new SqlParameter("firstname", firstname),
                new SqlParameter("lastname", lastname),
                new SqlParameter("admin", admin),
                new SqlParameter("username", username),
                new SqlParameter("password", password)
            });

        }
    }
}
