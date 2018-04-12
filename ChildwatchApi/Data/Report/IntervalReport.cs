using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data.Report
{
    public class IntervalReport : Report
    {
        public int Interval { get; set; }
        protected override Type GetRowType()
        {
            return typeof(Interval);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new Interval(builder);
        }

        public IntervalReport() { }
        public IntervalReport(SqlDataReader reader) : base(reader) { }
    }

    public class Interval : DataRow
    {
        public Interval(DataRowBuilder builder) : base(builder) { }

        public DateTime Time
        {
            get
            {
                return (DateTime)this["time"];
            }
            set { this["time"] = value; }
        }
        public int ChildCount
        {
            get
            {
                return (int)this["amount"];
            }
            set
            {
                this["amount"] = value;
            }
        }
    }
}
