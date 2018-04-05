using System;
using System.Web.Services;
using ChildWatchApi.Web;
using System.Configuration;
using System.Data.SqlClient;
using ChildWatchApi.Data;
using ChildWatchApi.Web;
using Newtonsoft.Json;

namespace ChildWatch
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ValidationResponse ValidateMember(ValidationToken token)
        {
            string connection = ConfigurationManager.ConnectionStrings["database"].ToString();
            SqlConnection sql = new SqlConnection(connection);
            SignInManager manager = new SignInManager(sql);

            Family f = manager.Validate(token.Barcode, token.Pin);
            OrganizationManager organization = new OrganizationManager(sql);
            
            return new ValidationResponse(f != null, f, organization.GetLocations(1));
        }

        [WebMethod]
        public static int SigninMembers(string data)
        {
            var token = JsonConvert.DeserializeObject<SigninToken>(data);
            SignInManager manager = 
                new SignInManager(new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

            int band = manager.SignIn(token.MemberId, token.Assignments);

            return band;
        }
    }
}