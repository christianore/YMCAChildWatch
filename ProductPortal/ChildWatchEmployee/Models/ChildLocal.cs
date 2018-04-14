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
        [Display(Name = "Guardian ID")]
        public int Guardian { get; set; }

        public override string ToString()
        {
            return "{\"firstName\":\"" + FirstName + "\",\"lastName\":\"" + LastName +
                    "\",\"BirthDate\":\"" + Birthday + "\"}";
        }
    }
}