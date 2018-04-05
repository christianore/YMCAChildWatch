using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Web
{
    public class SigninToken
    {
        public string MemberId { get; set; }
        public Assignment[] Assignments { get; set; }
    }
    public class Assignment
    {
        public int Child { get; set; }
        public int Location { get; set; }
    }
}
