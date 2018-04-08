using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using ChildWatchApi.Web;

namespace ChildWatchApi.Data
{
    public class SignInManager : IDatabaseManager
    {
        public SignInManager(SqlConnection connector) : base(connector) { }

        public Family Validate(string barcode, string pin)
        {           
            if (barcode.Length != 6 && pin.Length != 4)
            {
                throw new Exception("One or more arguments invalid.");
            }
            else
            {
                MembershipManager membership = new MembershipManager(ConnectionString);
                
                Member member = membership.GetMember(barcode, pin);
                  
                if (member !=  null)
                {
                    List<Child> children = membership.GetChildren(member);
                        
                    if (children.Count() > 0)
                    {
                        Family family = new Family()
                        {
                            Guardian = member,
                            Children = children
                        };

                        return family;
                    }                         
                }

            }
           
            return null;
        }
  

        public bool SignOut(int code)
        {
            return Run("p_signin_out", new SqlParameter[] { new SqlParameter("band", code) });
        }

        public Signin SignIn(string member_id, Assignment[] arr)
        {
            Signin signin = new Signin(-1, -1);
                        
            SqlParameter band_num = new SqlParameter()
            {
                Direction = System.Data.ParameterDirection.Output,
                ParameterName = "band_number",
                Size = int.MaxValue,
                SqlDbType = System.Data.SqlDbType.Int
            };
            SqlParameter signin_id = new SqlParameter()
            {
                Direction = System.Data.ParameterDirection.Output,
                ParameterName = "signin_id",
                Size = int.MaxValue,
                SqlDbType = System.Data.SqlDbType.Int
            };

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("member_id", member_id),
                band_num,
                signin_id
            };

            try
            {
                bool val = Run("p_signin_in", parms);

                signin.Band = (int)band_num.Value;
                signin.Id = (int)signin_id.Value;

                if (!(signin.Id > 0 && signin.Band > 0)) throw new Exception();

                foreach (Assignment a in arr)
                {
                    SqlParameter[] assignments = new SqlParameter[]{
                        new SqlParameter("child_id", a.Child),
                        new SqlParameter("location_id", a.Location),
                        new SqlParameter("signin_id", signin.Id)
                    };

                    if (!Run("p_signin_detail_add", assignments)) throw new Exception();
                }

            }
            catch(Exception ex)
            {
                var s = ex.Message;
            }

            return signin;
        }
    }
    public class Signin
    {
        public int Band { get; set; }
        public int Id { get; set; }

        public Signin(int band, int id)
        {
            Band = band;
            Id = id;
        }
    }
}
