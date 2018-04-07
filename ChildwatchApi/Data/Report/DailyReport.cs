using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi.Data.Report
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

        public DateTime Date
        {
            get { return (DateTime)this["date"]; }
            set { this["date"] = value; }
        }
        
        public int Count
        {
            get { return (int)this["count"]; }
            set { this["count"] = value; }
        }
        public string LocationName
        {
            get { return (string)this["location_name"]; }
            set { this["location_name"] = value; }
        }
        public int LocationId
        {
            get { return (int)this["location_id"]; }
            set { this["location_id"] = value; }
        }

    }
}
