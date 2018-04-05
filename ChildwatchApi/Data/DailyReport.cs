using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data
{
    public class DailyReport : Report
    {
        public DailyReport()
        {
            Description = "Reports the members that use the service by day.";
        }
        public DailyReport(SqlDataReader reader) : base(reader)
        {
            Description = "Reports the members that use the service by day.";
        }
        protected override Type GetRowType()
        {
            return typeof(DailyRecord);
        }
        public new DailyRecord NewRow()
        {
            return (DailyRecord)base.NewRow();
        }
    }
    public class DailyRecord : DataRow
    {
        public DailyRecord(DataRowBuilder builder) : base(builder) { }


    }
}
