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
                //throw new InvalidLoginException(barcode, pin);
            }
            else
            {
                MembershipManager membership = new MembershipManager(Database);
                
                Member member = membership.GetMember(barcode, pin);
                  
                if (member !=  null)
                {
                    IEnumerable<Child> children = membership.GetChildren(member);
                        
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
            OpenConnection("p_signout");
            AddParameters(new SqlParameter("band", code));
            return command.ExecuteNonQuery() > 0;
        }

        public int SignIn(string member_id, Assignment[] arr)
        {
            int band = -1;
            int id = -1;

            OpenConnection("p_signin_in");

            SqlParameter band_num = new SqlParameter()
            {
                Direction = System.Data.ParameterDirection.Output,
                ParameterName = "band_number",
                Size = int.MaxValue
            };
            SqlParameter signin_id = new SqlParameter()
            {
                Direction = System.Data.ParameterDirection.Output,
                ParameterName = "signin_id",
                Size = int.MaxValue
            };

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("member_id", member_id),
                band_num,
                signin_id
            });

            try
            {
                command.ExecuteNonQuery();

                band = (int)band_num.Value;
                id = (int)signin_id.Value;

                if (!(id > 0 && band > 0)) throw new Exception();

                    
                OpenConnection("p_signin_detail_add");

                foreach (Assignment a in arr)
                {
                    AddParameters(new SqlParameter[]{
                        new SqlParameter("child_id", a.Child),
                        new SqlParameter("location_id", a.Location),
                        new SqlParameter("signin_id", id)
                    });

                    if (command.ExecuteNonQuery() <= 0) throw new Exception();
                }

            }
            catch(Exception)
            {
                
            }

            return band;
        }
    }
}
