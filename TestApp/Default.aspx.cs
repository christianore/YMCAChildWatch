using System;
using System.Web.Services;
using System.Web.UI;
using TestApp.localhost;

namespace TestApp
{
    public partial class _Default : Page
    {
        private static YMCAWebService service = new YMCAWebService();

        protected void Page_Load(object sender, EventArgs e)
        {
        
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