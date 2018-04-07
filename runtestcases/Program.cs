using System;
using System.Data.SqlClient;
using ChildWatchApi;
using System.Configuration;
using ChildWatchApi.Data;
using ChildWatchApi.Data.Report;
using System.Collections.Generic;
using ChildWatchApi.Web;

namespace RunTestCases
{
    public class Program
    {
        private static string connection_string = ConfigurationManager.ConnectionStrings["database"].ToString();

        public static void Main(string[] args)
        {
            Console.Title = "YMCA Child Watch Manager";
            
            bool end = false;
            YMCADataGenerator gen = new YMCADataGenerator();
            SqlConnection connection = new SqlConnection(connection_string);
            MembershipManager membership = new MembershipManager(connection);
            SignInManager signIn = new SignInManager(connection);
            ReportManager reports = new ReportManager(connection);
            while (!end)
            {
                Console.Write("YMCA> ");
                string[] options = Console.ReadLine().Split(' ');

                try
                {
                    switch (options[0].ToUpper())
                    {
                        // Options on members
                        case "MEMBER":
                            switch(options[1]){
                                case "/s":
                                    Member newMember = new Member()
                                    {
                                        MemberId = options[2],
                                        FirstName = options[3],
                                        LastName = options[4],
                                        Barcode = options[5],
                                        Pin = options[6],
                                        PhoneNumber = options[7],
                                        IsActive = options.Length <= 8 ? true : (options[8] == "0" ? false : true)
                                    };
                                    bool res = membership.SaveMember(newMember);
                                    Console.WriteLine(String.Format("Member save {0}", res ? "successful" : "failed"));
                                    break;
                                case "/g":
                                    Member found  = null;
                                    switch (options.Length)
                                    {
                                        case 3:
                                            found = membership.GetMember(options[2]);
                                            break;
                                        case 4:
                                            found = membership.GetMember(options[2], options[3]);
                                            break;
                                    }
                                    if (found != null)
                                    {
                                        Console.WriteLine("Member Information:");
                                        Console.WriteLine(found.ToString());
                                    }
                                    else
                                        Console.WriteLine("Unable to find member information");
                                    break;
                                
                            }
                            break;
                            // Options on children
                        case "CHILD":
                            switch (options[1])
                            {
                                case "/a": // Attach child to member  
                                    bool attached = membership.AttachChild(options[2], int.Parse(options[3]));
                                    Console.WriteLine("Child Attached: " + (attached ? "Completed" : "Failed"));
                                    break;
                                case "/d": // Detach child from member
                                    bool detached = membership.DetachChild(options[2], int.Parse(options[3]));
                                    Console.WriteLine("Child Detached: " + (detached ? "Completed" : "Failed"));
                                    break;
                                case "/u": // Update child
                                    bool updated = membership.UpdateChild(new Child()
                                    {
                                        Id = int.Parse(options[2]),
                                        FirstName = options[3],
                                        LastName = options[4],
                                        BirthDate = DateTime.Parse(options[5])
                                    });
                                    Console.WriteLine("Child Updated: " + (updated ? "Completed" : "Failed"));
                                    break;
                                case "/i": // Insert Child
                                    int id = membership.InsertChild(new Child()
                                    {
                                        FirstName = options[2],
                                        LastName = options[3],
                                        BirthDate = DateTime.Parse(options[4]),
                                        
                                    }, options[5]);
                                    if(id >= 1)
                                    {
                                        Console.WriteLine("Child added with id = " + id.ToString());
                                    }
                                    else
                                        Console.WriteLine("Child Insert: Failed");
                                    break;
                                case "/g": // Get Child
                                    Child c = membership.GetChild(int.Parse(options[2]));

                                    if (c == null)
                                        Console.WriteLine("Child not found");
                                    else
                                        Console.WriteLine("Child Information:\n" + c.ToString());
                                    break;
                            }                
                            break;
                        case "CREATEMEMBERS":
                            
                            Console.WriteLine("Member Count: ");
                            int x = int.Parse(Console.ReadLine());
                            Console.WriteLine("Child Count: ");
                            int y = int.Parse(Console.ReadLine());
                            Random r = new Random();
                            int registered = 0;
                            while (x != 0)
                            {
                                try
                                {
                                    List<Child> child = new List<Child>();
                                    Member m = gen.RandomMember();
                                    x--;

                                    if (y > 0)
                                    {
                                        int childs = Math.Min(r.Next(6), y);
                                        
                                        for (int i = 0; i < childs; i++)
                                            child[i] = gen.RandomChild(m.LastName);

                                        y = y - childs;
                                    }

                                    Family family = new Family()
                                    {
                                        Guardian = m,
                                        Children = child
                                    };

                                    if (membership.RegisterFamily(family)) registered++;
                                }
                                catch
                                {

                                }
                                
                            }
                            Console.WriteLine("Registered " + registered + " members and their children");
                            break;
                        case "MEMBERREPORT":
                            reports.GetMemberReport(true);
                            break;
                        case "CREATESIGNIN":
                            Random random = new Random();
                            DateTime start = new DateTime(2018, 4, 6, 9, 0, 0);
                            DateTime stop = new DateTime(2018, 4, 6, 21, 0, 0);
                            TimeSpan interval = stop - start;
                            int timefromstart = random.Next((int)interval.TotalMinutes);

                            DateTime signinTime = start.AddMinutes(timefromstart);
                            interval = stop - signinTime;

                            DateTime signoutTime = signinTime.AddMinutes(random.Next((int)interval.TotalMinutes));

                            Console.WriteLine(signinTime);
                            Console.WriteLine(signoutTime);

                            Family fam = gen.RandomFamily(reports);
                           
                            Console.WriteLine(fam.Guardian);
                            List<Location> list = signIn.GetLocations(1);
                            int count = random.Next(1,fam.Children.Count-1);
                            List<Assignment> assignments = new List<Assignment>();
                            for(int i = 0; i < count; i++)
                            {
                                Child c = fam.Children[random.Next(list.Count)];
                                Assignment assignment = new Assignment()
                                {
                                    Child = c.Id,
                                    Location = list[random.Next(list.Count)].Id
                                };
                                assignments.Add(assignment);
                            }
                            int band = signIn.SignIn(fam.Guardian.MemberId, assignments.ToArray());

                            if(band > 0)
                                signIn.SignOut(band, signoutTime);

                            break;
                        case "CLEAR":
                            Console.Clear();
                            break;
                      
                        case "EXIT":
                            end = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occurred");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("\n" + ex.StackTrace);
                }
            }
        }
    }
}
