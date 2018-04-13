using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data
{
    public class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberId { get; set; }
        public string Barcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Pin { get; set; }
        public bool IsActive { get; set; }

        public Member(string firstName, string lastName, string memberId, string barCode, string phoneNumber,
            string pin, bool active)
        {
            FirstName = firstName;
            LastName = lastName;
            MemberId = memberId;
            Barcode = barCode;
            PhoneNumber = phoneNumber;
            Pin = pin;
            IsActive = active;
        }

        public Member() { }

        public override string ToString()
        {
            return "Member# " + MemberId + "\n" + FirstName + " " + LastName + "\nBarcode: " + Barcode + "\nPin: " + Pin + "\nStatus: " + (IsActive ? "Active" : "InActive");
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                Member member = (Member)obj;
                return MemberId.ToLower().Equals(member.MemberId.ToLower());
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return MemberId.GetHashCode();
        }
    }
}
