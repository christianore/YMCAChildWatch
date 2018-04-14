using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChildWatchApi;

namespace ChildWatchEmployee.Models
{
    public class Member
    {
        [Required]
        [MaxLength(11, ErrorMessage ="Member ID cannot be longer than 11 Digits")]
        [RegularExpression("\\d{11}")]
        public string MemberID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="First Name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage ="Last Name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$",
            ErrorMessage ="Must Enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("\\d{6}")]
        [Display(Name = "Barcode")]
        public string BarCode { get; set; }

        [Required]
        [StringLength(4)]
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