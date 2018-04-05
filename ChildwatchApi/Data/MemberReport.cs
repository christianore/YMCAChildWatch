using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data
{
    public class MemberReport : Report
    {
        public MemberReport() : base()
        {
            Description = "Report of members in the program";
        }
        public MemberReport(SqlDataReader reader) : base(reader)
        {
            Description = "Report of members in the program";
        }
        protected override Type GetRowType()
        {
            return typeof(MemberRecord);
        }
        public new MemberRecord NewRow()
        {
            return (MemberRecord)base.NewRow();
        }
    }

    public class MemberRecord : DataRow
    {
        public MemberRecord(DataRowBuilder builder) : base(builder) { }
       
    }
}
