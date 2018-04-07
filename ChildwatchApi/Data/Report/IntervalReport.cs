using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data.Report
{
    public class IntervalReport : Report
    {
        public new Type GetRowType()
        {
            return typeof(Interval);
        }
        public new Interval NewRow()
        {
            return (Interval)base.NewRow();
        }
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
            set { this["Time"] = value; }
        }
        public int ChildCount
        {
            get
            {
                return (int)this["child_count"];
            }
            set
            {
                this["child_count"] = value;
            }
        }
    }
}
