using System;
using System.Data.SqlClient;
using ChildWatchApi;
using System.Configuration;

namespace RunTestCases
{
    public class Program
    {
        private static string connection_string = ConfigurationManager.ConnectionStrings["database"].ToString();

        public static void Main(string[] args)
        {
            Console.Title = "YMCA Child Watch Manager";
            
            bool end = false;
           
            SqlConnection connection = new SqlConnection(connection_string);
            MembershipManager membership = new MembershipManager(connection);

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
