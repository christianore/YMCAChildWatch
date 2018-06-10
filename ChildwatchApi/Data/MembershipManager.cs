using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ChildWatchApi.Data
{
    public class MembershipManager : IDatabaseManager
    {
        public MembershipManager(SqlConnection connector) : base(connector) { }
        public MembershipManager(String s) : base(s) { }

        public Member GetMember(string id)
        {                 
            SqlParameter[] arr = new SqlParameter[]
            {
                new SqlParameter("member_id", id)
            };

            SqlDataReader data = GetSqlDataReader("p_member_get", arr);

            Member member = null;
            try
            {
                if (data.Read())
                {
                    member = new Member()
                    {
                        FirstName = (string)data["member_fName"],
                        LastName = (string)data["member_lName"],
                        MemberId = (string)data["member_id"],
                        Barcode = (string)data["barcode"],
                        PhoneNumber = (string)data["phone"],
                        Pin = (string)data["pin"]
                    };
                }
            }
            finally
            {
                CloseReader(data);
                CloseConnection();
            }
            return member;
        }
        public Member GetMember(string barcode, string pin)
        {
            SqlParameter[] arr = new SqlParameter[]
            {
                new SqlParameter("barcode", barcode),
                new SqlParameter("pin", pin)
            };

            SqlDataReader data = GetSqlDataReader("p_member_get", arr);
            Member m = null;
            try
            {
                if (data.Read())
                {
                    m = new Member()
                    {
                        FirstName = (string)data["member_fName"],
                        LastName = (string)data["member_lName"],
                        MemberId = (string)data["member_id"],
                        Barcode = (string)data["barcode"],
                        PhoneNumber = (string)data["phone"],
                        Pin = (string)data["pin"]
                    };

                }
            }
            finally
            {
                CloseReader(data);
                CloseConnection();
            }

            return m;
        }
        public bool SaveMember(Member m)
        {
            SqlParameter[] arr = new SqlParameter[]
            {
                new SqlParameter("member_id", m.MemberId),
                new SqlParameter("barcode", m.Barcode),
                new SqlParameter("pin", m.Pin),
                new SqlParameter("first_name", m.FirstName),
                new SqlParameter("last_name", m.LastName),
                new SqlParameter("phone", m.PhoneNumber),
                new SqlParameter("active", m.IsActive)
            };

            return RunCommand("p_member_save", arr);
        }
        public bool RegisterFamily(Family family)
        {
            bool complete = true;
            SaveMember(family.Guardian);

            foreach(Child c in family.Children)
            {
                int id = InsertChild(c, family.Guardian);
                if (id > 0)
                    c.Id = id;
                else
                    complete = false;
            }


            return complete;
        }
        public Child GetChild(int id)
        {
            Child c = null;
            SqlParameter[] arr = new SqlParameter[]
            {
                new SqlParameter("child_id", id)
            };
            
            SqlDataReader reader = GetSqlDataReader("p_child_get", arr);
            try
            {
                c = ExtractChild(reader);
            }
            finally
            {
                
                CloseReader(reader);
                CloseConnection();
            }

            return c;
        }
        public bool UpdateChild(Child c)
        {
            OpenConnection("p_child_update");

            return RunCommand("p_child_update", new SqlParameter[]
            {
                new SqlParameter("child_id", c.Id),
                new SqlParameter("first_name", c.FirstName),
                new SqlParameter("last_name", c.LastName),
                new SqlParameter("birth_date", c.BirthDate)
            });
        }
        public int InsertChild(Child c, Member m)
        {
            return InsertChild(c, m.MemberId);
        }
        public int InsertChild(Child c, string memberId)
        {
            return (int)RunValue("p_child_insert",new SqlParameter[]
            {
                new SqlParameter("fname", c.FirstName),
                new SqlParameter("lname", c.LastName),
                new SqlParameter("birth_dt", c.BirthDate),
                new SqlParameter("member_id", memberId)
            });
        }
        public List<Child> GetChildren(Member m)
        {
            List<Child> children = new List<Child>();
            bool read = true;
            SqlDataReader reader = GetSqlDataReader("p_member_child_get", new SqlParameter[]
            {
                new SqlParameter("member_id", m.MemberId)
            });

            try
            {
                do
                {
                    Child c = ExtractChild(reader);
                    if (c != null)
                        children.Add(c);
                    else
                        read = false;
                } while (read);
            }
            finally
            {
                CloseReader(reader);
                CloseConnection();
            }
        
            return children;
        }
        public bool AttachChild(string memberId, int childId)
        {
            return RunCommand("p_member_child_attach", new SqlParameter[]{
                new SqlParameter("member_id", memberId),
                new SqlParameter("child_id", childId)
            });
        }
        public bool AttachChild(Member m, Child c)
        {
            return AttachChild(m.MemberId, c.Id);
        }
        public bool DetachChild(string memberId, int childId)
        {
            return RunCommand("p_member_child_detach", new SqlParameter[]{
                new SqlParameter("member_id", memberId),
                new SqlParameter("child_id", childId)
            });
        }
        public bool DetachChild(Member m, Child c)
        {
            return DetachChild(m.MemberId, c.Id);
        }
        protected Child ExtractChild(SqlDataReader reader)
        {
            if(reader.Read())
            {
                Child c = new Child()
                {
                    Id = (int)reader["child_id"],
                    FirstName = (string)reader["child_fName"],
                    LastName = (string)reader["child_lName"],
                    BirthDate = (DateTime)reader["birthdate"]
                };
                
                return c;               
            }
            return null;
        }
        public Family GetFamily(string id)
        {
            Member m = GetMember(id);
            Family family = new Family()
            {
                Guardian = m,
                Children = GetChildren(m)
            };
            return family;
        }
        public Family GetFamily(Member m)
        {
            return GetFamily(m.MemberId);
        }
   
    }
}
