using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi
{
    public class MembershipManager
    {
        private SqlConnection databaseConnection;
        private SqlCommand sqlCommand;

        public MembershipManager()
        {
            databaseConnection = new SqlConnection();
            sqlCommand = new SqlCommand()
            {
                CommandText = "",
                CommandType =CommandType.StoredProcedure,
                Connection = databaseConnection
            };
        }
        public MembershipManager(string connection_string, bool openConnection = false) : this()
        {
            databaseConnection.ConnectionString = connection_string;
            if (openConnection)
                databaseConnection.Open();         
        }
        ~MembershipManager()
        {
            if(databaseConnection.State == ConnectionState.Open)
            {
                databaseConnection.Close();
            }
        }
        /*
        public Member GetMemberById(string id)
        {
            sqlCommand.CommandText = "p_member_getbyid";
        }
        public Member ValidateMember(string barcode, string pin)
        {
            sqlCommand.CommandText = "p_member_validate";
        }
        */
        public void SaveMember(Member member)
        {
            sqlCommand.CommandText = "p_member_save";
        }
        public void SaveMember(Child child)
        {
            sqlCommand.CommandText = "p_child_save";
        }
        
    }
}
