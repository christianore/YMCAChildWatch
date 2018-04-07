using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ChildWatchApi;

namespace ChildWatchEmployee.Models
{
    public static class DBConnection
    {
        private static string connection_string = ConfigurationManager.ConnectionStrings["CLOTest"].ToString();
        /**public static MembershipManager ChildWatchConnection()
        { 
            SqlConnection connection = new SqlConnection(connection_string);
            MembershipManager membership = new MembershipManager();
            return membership;
        }**/
    }
}