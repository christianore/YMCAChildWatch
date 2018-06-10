using System;
using System.Data;
using System.Data.SqlClient;

namespace ChildWatchApi.Data
{
    /// <summary>
    ///  Class for pulling reports from child watch database.
    /// </summary>
    public class ReportManager : IDatabaseManager
    {
        public ReportManager(SqlConnection sqlconnector) : base(sqlconnector) { }

        /// <summary>
        /// Gets a report for the children signed in over certain amount of time broken into increments.
        /// </summary>
        /// <param name="interval">Incremental amount of minutes to partition</param>
        /// <param name="startTime">Start date and time</param>
        /// <param name="endTime">End date and time/param>
        /// <param name="locationID">Id of the location of the children</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the current list of signed in children
        /// </summary>
        /// <returns>A datatable with a list of signed in children.</returns>
        public DataTable GetChildrenSignedIn()
        {
            return GetDataTable("r_children_signedin", new SqlParameter[] { });
        }
        /// <summary>
        /// Gets information amount the memebers in the database.
        /// </summary>
        /// <param name="areActive">Boolean flag to determine if they are active or not.  A null value will pull both.</param>
        /// <returns>A list of results from the report pull.</returns>
        public DataTable GetMemberReport(Boolean? areActive = null)
        {
            return GetDataTable("r_members", new SqlParameter[]
            {
                new SqlParameter("active",areActive)
            });
        }
        /// <summary>
        /// Gets a list of data showing the amount of children who used the service on that day.
        /// </summary>
        /// <param name="dateStart">Starting day</param>
        /// <param name="dateEnd">Ending day</param>
        /// <param name="location">Location ID to pull a list for</param>
        /// <returns>A list of children who used the child watch service at the specidifed location between the dates given.</returns>
        public DataTable GetDailyReport(DateTime dateStart, DateTime dateEnd, int location)
        {
            if(dateStart > dateEnd)
                throw new Exception("Date range invalid.  Starting date must me less than the end date.");

            return GetDataTable("r_daily", new SqlParameter[]
            {
                new SqlParameter("dayStart", dateStart),
                new SqlParameter("dayEnd", dateEnd),
                new SqlParameter("location", location)
            });
        }  
    }
}
