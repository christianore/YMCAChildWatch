using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi
{
    public class Member
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberId { get; set; }
        public string Barcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Pin { get; set; }

        public Member(string firstName, string lastName, string memberId, string barCode, string phoneNumber,
            string pin)
        {
            FirstName = firstName;
            LastName = lastName;
            MemberId = memberId;
            Barcode = barCode;
            PhoneNumber = phoneNumber;
            Pin = pin;
        }
        public Member() { }
    }
}
