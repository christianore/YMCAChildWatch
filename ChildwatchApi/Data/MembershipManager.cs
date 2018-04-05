﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ChildWatchApi.Data
{
    public class MembershipManager : IDatabaseManager
    {
        public MembershipManager(SqlConnection connector) : base(connector) { }

        public Member GetMember(string id)
        {
            OpenConnection("p_member_get");
            

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("member_id", id)
            });

            SqlDataReader data = command.ExecuteReader();

            if (data.HasRows)
            {
                data.Read();
                Member m = new Member()
                {
                    FirstName = (string)data["member_fName"],
                    LastName = (string)data["member_lName"],
                    MemberId = (string)data["member_id"],
                    Barcode = (string)data["barcode"],
                    PhoneNumber = (string)data["phone"],
                    Pin = (string)data["pin"]
                };
                data.Close();
                return m;
            }

            return null;
        }
        public Member GetMember(string barcode, string pin)
        {
            OpenConnection("p_member_get");
          

            AddParameters(new SqlParameter[]
            {
                new SqlParameter("barcode", barcode),
                new SqlParameter("pin", pin)
            });

            SqlDataReader data = command.ExecuteReader();

            if (data.HasRows)
            {
                data.Read();
                Member m = new Member()
                {
                    FirstName = (string)data["member_fName"],
                    LastName = (string)data["member_lName"],
                    MemberId = (string)data["member_id"],
                    Barcode = (string)data["barcode"],
                    PhoneNumber = (string)data["phone"],
                    Pin = (string)data["pin"]
                };
                data.Close();
                return m;
            }

            return null;
        }
        public bool SaveMember(Member m)
        {
            OpenConnection("p_member_save");
            
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("member_id", m.MemberId),
                new SqlParameter("barcode", m.Barcode),
                new SqlParameter("pin", m.Pin),
                new SqlParameter("first_name", m.FirstName),
                new SqlParameter("last_name", m.LastName),
                new SqlParameter("phone", m.PhoneNumber),
                new SqlParameter("active", m.IsActive)
            });

            return command.ExecuteNonQuery() > 0;
        }

        public Child GetChild(int id)
        {
            OpenConnection("p_child_get");
            
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("child_id", id)
            });
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close();
                return ExtractChild(reader);             
            }

            return null;
        }
        public bool UpdateChild(Child c)
        {
            OpenConnection("p_child_update");
            
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("child_id", c.Id),
                new SqlParameter("first_name", c.FirstName),
                new SqlParameter("last_name", c.LastName),
                new SqlParameter("birth_date", c.BirthDate)
            });
            return command.ExecuteNonQuery() > 0;
        }
        public int InsertChild(Child c, string memberId)
        {
            OpenConnection("p_child_insert");
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("fname", c.FirstName),
                new SqlParameter("lname", c.LastName),
                new SqlParameter("birth_dt", c.BirthDate),
                new SqlParameter("member_id", memberId)
            });
            return (int)command.ExecuteScalar();
        }
        public IEnumerable<Child> GetChildren(Member m)
        {
            List<Child> children = new List<Child>();

            OpenConnection("p_member_getChildren");

          
            AddParameters(new SqlParameter[]
            {
                new SqlParameter("member_id", m.MemberId)
            });

            SqlDataReader reader = command.ExecuteReader();

            bool read = true;
            do
            {
                Child c = ExtractChild(reader);
                if (c != null)
                    children.Add(c);
                else
                    read = false;
            } while (read);
            reader.Close();
            return children;
        }
        public bool AttachChild(string memberId, int childId)
        {
            OpenConnection("p_member_child_attach");
            
            AddParameters(new SqlParameter[]{
                new SqlParameter("member_id", memberId),
                new SqlParameter("child_id", childId)
            });
            return command.ExecuteNonQuery() > 0;
        }
        public bool AttachChild(Member m, Child c)
        {
            return AttachChild(m.MemberId, c.Id);
        }
        public bool DetachChild(string memberId, int childId)
        {
            OpenConnection("p_member_child_detach");
           
            AddParameters(new SqlParameter[]{
                new SqlParameter("member_id", memberId),
                new SqlParameter("child_id", childId)
            });
            return command.ExecuteNonQuery() > 0;
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
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    BirthDate = (DateTime)reader["BirthDate"]
                };
                
                return c;
                
            }
            return null;
        }
    }
}
