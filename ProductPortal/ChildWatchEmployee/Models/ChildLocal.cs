using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ChildWatchEmployee.Models
{
    public class ChildLocal
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Birthday")]      
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(11, ErrorMessage = "Member ID cannot be longer than 11 Digits")]
        [RegularExpression("\\d{11}", ErrorMessage ="Member ID must be an 11 digit number, cannot contain any letters, spaces, or special characters")]
        [Display(Name = "Guardian ID")]
        public string Guardian { get; set; }

        public override string ToString()
        {
            return "{\"firstName\":\"" + FirstName + "\",\"lastName\":\"" + LastName +
                    "\",\"BirthDate\":\"" + Birthday + "\"}";
        }

        public ChildWatchApi.Data.Child ToServer()
        {
            ChildWatchApi.Data.Child newChild = new ChildWatchApi.Data.Child();
            newChild.BirthDate = Birthday;
            newChild.FirstName = FirstName;
            newChild.LastName = LastName;
            return newChild;
        }
    }
}