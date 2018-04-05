using System;
using System.Collections.Generic;


namespace ChildWatchApi.Data
{
    public class Family
    {
        public Member Guardian { get; set; }
        public IEnumerable<Child> Children { get; set; } 
    }
}
