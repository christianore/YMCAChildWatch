using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace ChildWatchEmployee.Models
{
    public class SignOut
    {
        [Required]
        [RegularExpression("\\d{4}")]
        [Display(Name = "Band #")]
        public string BandNum { get; set; }
    }
}