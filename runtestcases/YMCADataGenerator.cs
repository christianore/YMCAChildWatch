using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChildWatchApi;
using ChildWatchApi.Data;

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
                IsActive = random.NextDouble() < .5,
                Barcode = RandomNumberOfSize(6),
                Pin = RandomNumberOfSize(4),
                MemberId = RandomNumberOfSize(11),
                PhoneNumber = RandomNumberOfSize(10)
            };
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
            long seconds = 788936400000;
            int diff = (int)(1483246800000 - seconds);
            random.Next(diff);
            return new DateTime(seconds + diff);
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

        public char RandomDigit(int i = 0)
        {
            return (char)random.Next(i == 0 ? 10 : i);
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
            Thread[] work = new Thread[3];
            work[0] = new Thread(Load);
            work[1] = new Thread(Load);
            work[2] = new Thread(Load);

            work[0].Start(new LoadObject("Male.txt", ref MaleFirst));
            work[1].Start(new LoadObject("Female.txt",ref FemaleFirst));
            work[2].Start(new LoadObject("Last.txt", ref Last));

            foreach (Thread t in work)
                t.Join();
        }

        private void Load(object obj)
        {
            LoadObject o = (LoadObject)obj;
            string[] lines = File.ReadAllLines(o.file);
            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Split(' ')[0];
            }
            o.list = lines;
        }
        private class LoadObject
        {
            public string file;
            public string[] list;

            public LoadObject(string file, ref string[] list)
            {
                this.file = file;
                this.list = list;
            }
        }
    }
}
