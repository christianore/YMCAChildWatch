using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi.Data.Report
{
    public class Report : DataTable
    {
        public DateTime DateRan { get; set; }
        public string Description { get; set; }

        public Report()
        {
            DateRan = DateTime.Now;
            Description = "";
        }
        public Report(SqlDataReader reader) : this()
        {
            Load(reader);
        }
    }
}
