using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildWatchApi.Data
{
    public class SignInDetail
    {
        public int ID { get; set; }
        public Child Child { get; set; }
        public Location WatchLocation { get; set; }
    }
}
