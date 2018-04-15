using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi.Data
{
    public class ReportManager : IDatabaseManager
    {
        public ReportManager(SqlConnection sqlconnector) : base(sqlconnector) { }

        public DataTable GetIntervalReport(int interval, DateTime startTime, DateTime endTime, int locationID)
        {
            return GetDataTable("r_interval", new SqlParameter[]
            {
                new SqlParameter("interval", interval),
                new SqlParameter("start", startTime),
                new SqlParameter("end", endTime),
                new SqlParameter("location", locationID)
            });
        }

        public DataTable GetChildrenSignedIn()
        {
            return GetDataTable("r_children_signedin", new SqlParameter[] { });
        }

        public DataTable GetMemberReport(Boolean? areActive = null)
        {
            return GetDataTable("r_members", new SqlParameter[]
            {
                new SqlParameter("active",areActive)
            });
        }

        public DataTable GetDailyReport(int days, DateTime dateFrom)
        {

            return GetDataTable("r_daily", new SqlParameter[]
            {
                new SqlParameter("days", days),
                new SqlParameter("dateFrom", dateFrom)
            });
        }  
    }
}
