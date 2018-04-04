using System;
using System.Data;

namespace ChildWatchApi
{
    public class Report : DataTable
    {
        public DateTime DateRan { get; set; }
        public string Description { get; set; }
    }
}
