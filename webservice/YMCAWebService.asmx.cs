using System;
using System.Web.Services;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ChildWatchApi;
using System.Web.Script.Services;


namespace YMCAWebService
{
    /// <summary>
    /// Summary description for YMCAWebService
    /// </summary>
    [WebService(Namespace = "ChildWatchApi")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)] 
    [ScriptService]
    public class YMCAWebService : System.Web.Services.WebService
    {

        protected string DatabaseConnection {
            get
            {
                return ConfigurationManager.ConnectionStrings["database"].ToString();
            }
        }
       
        [WebMethod]
        public string Version()
        {
            return ConfigurationManager.AppSettings["Version"].ToString();
        }

        [WebMethod]
        public YMCAServiceResponse RegisterMember(Member newMember)
        {
            Log(LogType.Initialize, "Starting to register a new member");
            YMCAServiceResponse serviceResponse = new YMCAServiceResponse();
       
            try
            {
                // Connect to the database and add the new member.
                using (SqlConnection connect = new SqlConnection(DatabaseConnection))                {
                    connect.Open();
                    using (SqlCommand command = new SqlCommand("p_member_insert", connect) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        command.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter("id", newMember.MemberId),
                            new SqlParameter("fname", newMember.FirstName),
                            new SqlParameter("lname", newMember.LastName),
                            new SqlParameter("phone", newMember.PhoneNumber),
                            new SqlParameter("barcode", newMember.Barcode),
                            new SqlParameter("pin", newMember.Pin)
                        });
                        Log(LogType.Information, "Sending information to database");
                        command.ExecuteNonQuery();

                    }
                    connect.Close();
                }
                Log(LogType.Information, "Successfully Added the new member");
                serviceResponse.Message = "Successfully added the new member";
            }
            catch(Exception ex)
            {
                Log(LogType.Error, "Exception Thrown: " + ex.Message);
                //serviceResponse.Data = ex;
                serviceResponse.Error = true;
                serviceResponse.Message = "Error occured when adding the member";
            }
            Log(LogType.Ending, "Exiting Register Member");
            return serviceResponse;
        }

        [WebMethod]
        public YMCAServiceResponse Validate(string barcode, string pin)
        {
           
            Log(LogType.Initialize, "Starting to Validate: " + barcode + ";" + pin); 
            YMCAServiceResponse response = new YMCAServiceResponse();

            if(barcode.Length != 6 && pin.Length != 4)
            {
                response.Error = true;
                response.Message = "Data was not provdided in the correct format";
                response.Data = "Barcode:  " + barcode + ";Pin:  " + pin;

                Log(LogType.Error, "Barcode or Pin not in the correct format");
            }
            else
            {
                SqlConnection connection = new SqlConnection(DatabaseConnection);

                SqlCommand validateCommand = new SqlCommand("p_validate_login", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                validateCommand.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("barcode", barcode),
                    new SqlParameter("pin", pin)
                });

                try
                {
                    Log(LogType.Information, "Validating");
                    connection.Open();

                    DataTable memberInformation = new DataTable();
                    memberInformation.Load(validateCommand.ExecuteReader());

                    int returnCount = memberInformation.Rows.Count;

                    if(returnCount == 1)
                    {
                        validateCommand.CommandText = "p_member_getChildren";
                        validateCommand.Parameters.Clear();
                        validateCommand.Parameters.Add(
                            new SqlParameter("member_id", memberInformation.Rows[0]["member_id"])
                            );

                        DataTable children = new DataTable();
                        children.Load(validateCommand.ExecuteReader());

                        if(children.Rows.Count > 0)
                        {
                            DataRow memberInfo = memberInformation.Rows[0];
                            Member m = new Member()
                            {
                                Barcode = barcode,
                                Pin = pin,
                                FirstName = (string)memberInfo["member_first_name"],
                                LastName = (string)memberInfo["member_last_name"],
                                MemberId = (string)memberInfo["member_id"],
                                PhoneNumber = (string)memberInfo["member_phone"]
                            };
                        }
                        else
                        {
                            response.Message = "Member has no children to load.  Unable to sign in";
                            Log(LogType.Warning, response.Message);
                            response.Error = true;
                        }
                    }
                    else if(returnCount > 1)
                    {
                        response.Message = "Two members have the same barcode and pin.  Data Integrity Compromised";
                        Log(LogType.Critical, response.Message);
                    }
                    else
                    {
                        response.Message = "Unable to validate member";
                        Log(LogType.Warning, response.Message);
                    }
                }
                catch(Exception ex)
                {
                    string err_message = "Exception Thrown: " + ex.Message;
                    Log(LogType.Error, err_message);
                    response.Message = err_message;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }  
            }
            Log(LogType.Ending, "Exiting Validation");

            return response;
        }
        /*
        [WebMethod]
        public YMCAServiceResponse SignIn(Member member, IEnumerable<Assignment> locations)
        {
            YMCAServiceResponse response = new YMCAServiceResponse();

            return response;
        }
        */
        [WebMethod]
        public YMCAServiceResponse SignOut(int band)
        {
            YMCAServiceResponse serviceResponse = new YMCAServiceResponse()
            {
                Error = true,
                Message = "Family was unable to be validated."
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()))
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand("p_signout", connection) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        command.Parameters.Add(new SqlParameter("band", band));

                        int retval = (int)command.ExecuteScalar();

                        if (retval == 0)
                        {
                            serviceResponse.Error = false;
                            serviceResponse.Message = "Family was succesfully signed out.";
                        }


                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Data = ex;

            }
            return serviceResponse;
        }

        [WebMethod]
        public YMCAServiceResponse RegisterChild(Child obj, string member_id)
        {
            YMCAServiceResponse response = new YMCAServiceResponse()
            {
                Error = true
            };

            SqlConnection sql = new SqlConnection(DatabaseConnection);
            SqlCommand cmd = new SqlCommand("p_child_insert", sql)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("fname", obj.FirstName),
                new SqlParameter("lname", obj.LastName),
                new SqlParameter("birth_dt", obj.BirthDate),
                new SqlParameter("member_id", member_id)
            });
            try
            {
                sql.Open();
                int child_id = (int)cmd.ExecuteScalar();
                if(child_id > 0)
                {
                    obj.Id = child_id;
                    response.Error = false;
                    response.Data = obj;
                    response.Message = "Successfully registered this child to this member";
                }
                else
                {
                    response.Message = "Unable to register child";                   
                }
            }
            catch(Exception ex)
            {
                Log(LogType.Error, "Exception Thrown: " + ex.Message);
                response.Message = ex.Message;
            }
            finally
            {
                if (sql.State == ConnectionState.Open)
                    sql.Close();
            }

            return response;
        }

        [WebMethod]
        public YMCAServiceResponse AssignMember(string member_id, int child_id)
        {
            YMCAServiceResponse response = new YMCAServiceResponse()
            {
                Error = true,
                Message = "Unable to attach child to member"
            };
            

            using (SqlConnection db = new SqlConnection(DatabaseConnection))
            using(SqlCommand command = new SqlCommand("p_member_attachChild", db) { CommandType = CommandType.StoredProcedure })
            {
                try
                {
                    db.Open();
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("member_id", member_id),
                        new SqlParameter("child_id", child_id)
                    });

                    int rows = command.ExecuteNonQuery();

                    if (rows <= 0)
                        throw new Exception("Unable to attach child to member");

                    response.Error = false;
                    response.Message = "Successfully added child to member";
                }
                catch(Exception ex)
                {
                    Log(LogType.Error, "Assign Member: " + ex.Message);
                    response.Message = "Assign Member: " + ex.Message;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
            
            return response;
        }

        public enum LogType
        {
            Initialize = 1,
            Information = 2,
            Error = 3,
            Warning = 4,
            Critical = 5,
            Ending = 6
        }
        [WebMethod]
        public void Log(LogType type, string message)
        {
            using(SqlConnection db = new SqlConnection(DatabaseConnection))
            {
                using (SqlCommand command = new SqlCommand("p_log_insert", db) { CommandType = CommandType.StoredProcedure })
                {
                    db.Open();
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("type", type),
                        new SqlParameter("message", message),
                        new SqlParameter("source", "YMCAWEBSERVICE")
                    });
                    command.ExecuteNonQuery();
                    db.Close();
                }
            }
        }
    }
}
