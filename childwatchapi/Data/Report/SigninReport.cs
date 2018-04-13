using System;
using System.Data;
using System.Data.SqlClient;
namespace ChildWatchApi.Data.Report
{
    public class SigninReport : Report 
    {
        public SigninReport() : base()
        {
            Description = "Children signed into the system";
        }

        public SigninReport(SqlDataReader reader) : base(reader)
        {
            Description = "Children signed into the system";
        }

        protected override Type GetRowType()
        {
            return typeof(SigninRecord);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new SigninRecord(builder);
        }
    }
    public class SigninRecord : DataRow
    {
        public SigninRecord(DataRowBuilder builder) : base(builder) { }

        public int ChildID
        {
            get { return (int)this["child_id"]; }
            set { this["child"] = value; }
        }
        public string FirstName
        {
            get { return (string)this["child_fName"]; }
            set { this["child_fName"] = value;  }
        }
        public string LastName
        {
            get { return (string)this["child_lName"]; }
            set { this["child_lName"] = value; }
        }
        public DateTime BirthDate
        {
            get { return (DateTime)this["birthdate"]; }
            set { this["birthdate"] = value; }
        }
        public string Location
        {
            get { return (string)this["location_name"]; }
            set { this["location_name"] = value; }
        }

    }
}

