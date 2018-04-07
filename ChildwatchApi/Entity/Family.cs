using System;
using System.Collections.Generic;


namespace ChildWatchApi.Data
{
    public class Family
    {
        public Member Guardian { get; set; }
        public List<Child> Children { get; set; } 
    }
}
