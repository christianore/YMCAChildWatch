using System;
using System.Web.Services;
using System.Web.UI;
using TestApp.localhost;
using System.IO;
using System.Web;

namespace TestApp
{
    public partial class _Default : Page
    {
        private static YMCAWebService service = new YMCAWebService();

        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterRandomMembers();
            string[] names = CreateRandomName().Split(' ');

            Child c = new Child() {
                FirstName = names[0],
                LastName = names[1],
                BirthDate = new DateTime(2005, 10, 20),
                Id = -1
            };

            RegisterChild(c, "00000892443");

        }
 
        public static void RegisterRandomMembers()
        {
            YMCAWebService service = new YMCAWebService();

            Random rd = new Random();

            for (int i = 0; i < 6000; i++)
            {
                string name = CreateRandomName();
                Member x = new Member()
                {
                    MemberId = CreateRandomId(11),
                    Barcode = CreateRandomId(6),
                    Pin = CreateRandomId(4),
                    FirstName = name.Split(' ')[0],
                    LastName = name.Split(' ')[1], 
                    PhoneNumber = CreateRandomId(7)
                };
                service.RegisterMember(x);
            } 


        }
        private static string CreateRandomId(int size)
        {
            String value = "";
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                value += rd.Next(10).ToString();
            }
            return value;
        }

        private static string CreateRandomName()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] first_list = File.ReadAllLines(path + "/App_Data/First.txt");
            string[] last_list = File.ReadAllLines(path + "/App_Data/Last.txt");

            Random rd = new Random();
            string first = first_list[rd.Next(first_list.Length)];
            string last = last_list[rd.Next(last_list.Length)];

            return first.Split(' ')[0] + " " + last.Split(' ')[0];
        }

        [WebMethod]
        public static YMCAServiceResponse ValidateMember(string barcode, string pin)
        {
            YMCAServiceResponse response = service.Validate(barcode, pin);
            return response;
           
         
        }

        [WebMethod]
        public static YMCAServiceResponse RegisterChild(Child c, string member_id)
        {
            return service.RegisterChild(c, member_id);

        }
    }
}