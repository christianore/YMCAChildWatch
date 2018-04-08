using System;
using System.Collections.Generic;
using System.IO;
using ChildWatchApi.Data;
using ChildWatchApi.Data.Report;

namespace RunTestCases
{
    public class YMCADataGenerator
    {
        public string[] MaleFirst;
        public string[] FemaleFirst;
        public string[] Last;
        private Random random;

        public YMCADataGenerator()
        {
            random = new Random();
            LoadNames();
        }

        public Member RandomMember()
        {
            return new Member()
            {
                FirstName = RandomFirstName(),
                LastName = RandomLastName(),
                IsActive = random.NextDouble() < .75,
                Barcode = RandomNumberOfSize(6),
                Pin = RandomNumberOfSize(4),
                MemberId = RandomNumberOfSize(11),
                PhoneNumber = RandomNumberOfSize(10)
            };
        }
        public Family RandomFamily(ReportManager manager)
        {
            Member member = RandomMemberFromData(manager);
            MembershipManager membership = new MembershipManager(manager.ConnectionString);
            return membership.GetFamily(member);
        }
        public Member RandomMemberFromData(ReportManager manager)
        {
            MemberReport report = manager.GetMemberReport(true);
            MemberRecord selected = (MemberRecord)report.Rows[random.Next(report.Rows.Count)];
            return selected.ToMemberObject();
        }
        public Family RandomFamily()
        {
            Member m = RandomMember();
            List<Child> c = new List<Child>();

            for (int i = 0; i < random.Next(6); i++)
                c.Add(RandomChild(m.LastName));

            return new Family()
            {
                Guardian = m,
                Children = c
            };
        }

        public Child RandomChild(string lastname = "")
        {
            return new Child()
            {
                FirstName = RandomFirstName(),
                LastName = lastname == "" ? RandomLastName() : lastname,
                BirthDate = RandomBirthDate(),
                Id = 0
            };
        }

        public DateTime RandomBirthDate()
        {
            DateTime bday = DateTime.Now;
            bool found = false;
            while(!found)
            {
                try
                {
                    bday = new DateTime(random.Next(2000, 2018), random.Next(1, 12), random.Next(1, 31));
                    found = true;
                }
                catch
                {

                }
            }

            return bday;
        }

        public string RandomNumberOfSize(int size)
        {
            string s = "";
            for(int i = 0; i < size; i++)
            {
                s += RandomDigit();
            }
            return s;
        }

        public string RandomDigit(int i = 0)
        {
            return random.Next(i == 0 ? 10 : i).ToString();
        }

        public String RandomFirstName()
        {
            if(random.NextDouble() < .5)
            {
                return MaleFirst[random.Next(MaleFirst.Length)];
            }
            else
            {
                return FemaleFirst[random.Next(FemaleFirst.Length)];
            }
        }

        public String RandomLastName()
        {
            return Last[random.Next(Last.Length)];
        }

        private void LoadNames()
        {          
            MaleFirst = Load("Male.txt");
            FemaleFirst = Load("Female.txt");
            Last = Load("Last.txt");
        }

        private string[] Load(string file)
        {          
            string[] lines = File.ReadAllLines(file);
            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Split(' ')[0];
            }
            return lines;
        }
    }
}
