using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class UpdateMember
    {
        [Required]
        public string MemberID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Barcode")]
        public string BarCode { get; set; }

        public string Pin { get; set; }

        public override string ToString()
        {
            return "{\"firstName\":\"" + FirstName + "\",\"lastName\":\"" + LastName +
                "\",\"phoneNumber\":\"" + PhoneNumber + "\",\"Pin\":\"" + Pin + "\"}";
        }

        public ChildWatchApi.Data.Member toServer()
        {
            ChildWatchApi.Data.Member newMember = new ChildWatchApi.Data.Member();
            newMember.FirstName = this.FirstName;
            newMember.LastName = this.LastName;
            newMember.MemberId = this.MemberID;
            newMember.Pin = this.Pin;
            newMember.PhoneNumber = this.PhoneNumber;
            newMember.Barcode = this.BarCode;
            newMember.IsActive = true;
            return newMember;
            //return new ChildwatchApi.Member(this.FirstName, this.LastName, this.MemberID,
            //     this.BarCode, this.PhoneNumber, this.Pin);
        }
    }
}