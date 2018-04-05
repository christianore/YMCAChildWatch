using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildWatchApi.Data;

namespace ChildWatchApi.Web
{
    public class ValidationResponse
    {
        public bool IsSuccess { get; internal set; }
        public Family Family { get; internal set; }
        public Location[] Locations { get; internal set; }

        public ValidationResponse(bool success, Family family, Location[] locations)
        {
            IsSuccess = success;
            Family = family;
            Locations = locations;
        }
    }
    public class ValidationToken
    {
        public string Barcode { get; set; }
        public string Pin { get; set; }

    }
}
