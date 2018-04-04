using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi
{
    public class DatabaseLog : IDatabaseManager
    {
        public DatabaseLog(SqlConnection connector) : base(connector) { }

        public void Write(LogType type, string source, string message)
        {
            OpenConnection("p_log_insert");
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("type", type),
                new SqlParameter("message", message),
                new SqlParameter("source", source)
            });
        }

        public enum LogType
        {
            Initialize = 1,
            Information = 2,
            Error = 3,
            Warning = 4,
            Critical = 5,
            Ending = 6
        }
    }
}
