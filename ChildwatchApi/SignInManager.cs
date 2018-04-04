using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace ChildWatchApi
{
    public class SignInManager : IDatabaseManager
    {
        public SignInManager(SqlConnection connector) : base(connector) { }

        public Family Validate(string barcode, string pin)
        {           
            if (barcode.Length != 6 && pin.Length != 4)
            {
                throw new InvalidLoginException(barcode, pin);
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

        public int SignIn(SignInRecord signin)
        {
            int band = -1;

            OpenConnection("p_member_signin");

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("member_id", signin.Member.MemberId)
            });

            try
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    signin.Id = (int)reader["signin_id"];
                    band = (int)reader["band"];
                    
                    if (!(signin.Id > 0 && band > 0)) throw new Exception();

                    
                    OpenConnection("p_child_signin");
                    foreach (SignInDetail detail in signin.ChildDetails)
                    {
                        AddParameters(new SqlParameter[]{
                            new SqlParameter("child_id", detail.Child.Id),
                            new SqlParameter("watch_location", detail.WatchLocation)
                        });


                        if (command.ExecuteNonQuery() <= 0) throw new Exception();
                    }
                    
                }
            }
            catch(Exception)
            {
                RollBackSignIn(signin.Id);
            }

            return band;
        }

        protected void RollBackSignIn(int id)
        {

        }
    }
}
