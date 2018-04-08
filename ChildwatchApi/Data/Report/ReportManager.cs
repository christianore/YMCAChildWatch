using System;
using System.Data.SqlClient;

namespace ChildWatchApi.Data.Report
{
    public class ReportManager : IDatabaseManager
    {
        public ReportManager(SqlConnection sqlconnector) : base(sqlconnector) { }

        public IntervalReport GetIntervalReport(int partition, DateTime startTime, DateTime endTime)
        {
            var report = new IntervalReport(RunData("r_interval", new SqlParameter[]
            {
                new SqlParameter("partition", partition),
                new SqlParameter("startTime", startTime),
                new SqlParameter("endtime", endTime)
            }));
            CloseConnection();
            return report;
        }

        public MemberReport GetMemberReport(Boolean? areActive = null)
        {            
            var report = new MemberReport(RunData("r_members", new SqlParameter[]
            {
                new SqlParameter("active",areActive)
            }));
            CloseConnection();
            return report;
        }

        public DailyReport GetDailyReport(int days, DateTime dateFrom)
        {

            var report = new DailyReport(RunData("r_daily", new SqlParameter[]
            {
                new SqlParameter("days", days),
                new SqlParameter("dateFrom", dateFrom)
            }));
            CloseConnection();
            return report;
        }  
    }
}
