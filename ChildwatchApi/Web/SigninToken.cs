using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Web
{
    public class SigninToken
    {
        public string MemberId { get; set; }
        public Assignment[] Assignments { get; set; }
    }
    public class Assignment
    {
        public int Child { get; set; }
        public int Location { get; set; }
    }
    public class ChildwatchAuthentication
    {
        public string User { get; internal set; }
        public AuthContext Authentication {get; internal set;}

        private ChildwatchAuthentication() { }

        public static ChildwatchAuthentication Authenticate(string user, string password, SqlConnection database)
        {
            ChildwatchAuthentication authentication = new ChildwatchAuthentication();
            authentication.User = user;
          
            using (database)
            {
                try
                {
                    database.Open();

                    using (SqlCommand command = new SqlCommand("p_employee_validate", database))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user", user);
                        command.Parameters.AddWithValue("@pass", password);

                        int val = (int)command.ExecuteScalar();

                        
                        authentication.Authentication = (AuthContext)val;

                    }
                }
                finally
                {
                    if (database.State == ConnectionState.Open)
                        database.Close();
                }
            }

            return authentication;
        }
    }
    public enum AuthContext
    {
        Admin = 1,
        User = 0,
        Failed = -1,
        Locked = -2
    }

}
