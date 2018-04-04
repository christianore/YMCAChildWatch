using System;

namespace ChildWatchApi
{
    public class Child
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Id { get; set; }

        public Child()
        {
            
        }

        public override string ToString()
        {
            return "Child# " + Id.ToString() + "/n" + FirstName + " " + LastName + "/nDOB:" + BirthDate.ToShortDateString();
        }
    }
}
