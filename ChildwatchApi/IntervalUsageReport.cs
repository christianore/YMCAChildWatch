using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi
{
    public class IntervalUsageReport : Report
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
                return (DateTime)this["Time"];
            }
            set { this["Time"] = value; }
        }
        public int ChildCount
        {
            get
            {
                return (int)this["ChildCount"];
            }
            set
            {
                this["ChildCount"] = value;
            }
        }
    }
}
