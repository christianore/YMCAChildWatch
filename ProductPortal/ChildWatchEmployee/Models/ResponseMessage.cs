using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class ResponseMessage
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public Object Model { get; set; }

    }
}