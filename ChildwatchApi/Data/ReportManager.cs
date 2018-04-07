using System;
using System.Data.SqlClient;

namespace ChildWatchApi.Data
{
    public class ReportManager : IDatabaseManager
    {
        public ReportManager(SqlConnection sqlconnector) : base(sqlconnector) { }

        public IntervalReport GetIntervalReport(int partition, DateTime startTime, DateTime endTime)
        {
            OpenConnection("r_interval");
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("partition", partition),
                new SqlParameter("startTime", startTime),
                new SqlParameter("endtime", endTime)
            });

            IntervalReport report = new IntervalReport();
            report.Load(command.ExecuteReader());

            return report;
        }

        public MemberReport GetMemberReport(Boolean? areActive = null)
        {
            OpenConnection("r_members");

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("active",areActive)
            });

            return new MemberReport(command.ExecuteReader());

        }

        public DailyReport GetDailyReport(int days, DateTime dateFrom)
        {
            OpenConnection("r_daily");

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("days", days),
                new SqlParameter("dateFrom", dateFrom)
            });

            return new DailyReport(command.ExecuteReader());
        }  

    }
}
