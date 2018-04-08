using System;
using System.Collections.Generic;

namespace ChildWatchApi.Data
{
    public class SignInRecord
    {
        public int Id { get; set; }
        public Member Member { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public int BandNumber { get; set; }
        public string SignedOutBy { get; set; }
        public List<SignInDetail> ChildDetails { get; internal set; }

        public SignInRecord()
        {
            ChildDetails = new List<SignInDetail>();
        }

        public void AddDetail(SignInDetail detail)
        {
            ChildDetails.Add(detail);
        }

        public TimeSpan TotalTimeStayed()
        {
            return TimeOut -TimeIn;
        }
    }
}
