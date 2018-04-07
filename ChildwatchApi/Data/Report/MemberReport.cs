using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data.Report
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

        public string MemberId
        {
            get { return (string)this["member_id"];}
            set{ this["member_id"] = value; }
        }
        public string FirstName
        {
            get { return (string)this["member_fName"]; }
            set { this["member_fName"] = value;  }
        } 
        public string LastName
        {
            get { return (string)this["member_lName"]; }
            set { this["member_lName"] = value; }
        }
        public string Barcode
        {
            get { return (string)this["barcode"]; }
            set { this["barcode"] = value; }
        }
        public string Pin
        {
            get { return (string)this["pin"]; }
            set { this["pin"] = value; }
        }
        public string Phone
        {
            get { return (string)this["phone"]; }
            set { this["phone"] = value; }
        }
        public bool Active
        {
            get { return (bool)this["active"]; }
            set { this["active"] = value; }
        } 
        public Member ToMemberObject()
        {
            return new Member()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Barcode = this.Barcode,
                Pin = this.Pin,
                PhoneNumber = this.Phone,
                IsActive = this.Active
            };
        }
    }
}
