using LoginApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace LoginApp.Controllers
{
    
        public class HomeController : Controller
        {

            public ActionResult Index()
            {

            if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Dash");
                }

                else
                {
                    Session.Clear();
                    TempData.Clear();
                    return View("Index1");
                }

            }

  

        public JsonResult Auth(FormCollection form)
        {


            List<temployModel> customers = new List<temployModel>();
            MySqlConnection connection = new MySqlConnection(ConstantModel.ConnectionString);

            string query = $"SELECT firstname, lastname, email, password FROM users where email=\"{form["email"]}\"";
            Console.WriteLine(query);
            MySqlCommand command = new MySqlCommand(query);

            command.Connection = connection;
            connection.Open();
            MySqlDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                temployModel Data = new temployModel();
                Data.FirstName = sdr["firstname"].ToString();
                Data.Lastname = sdr["lastname"].ToString();
                Data.Email = sdr["email"].ToString();
                Data.Password = sdr["password"].ToString();
                customers.Add(Data);
            }
            connection.Close();

            Dictionary<string, string> response = new Dictionary<string, string>();

            MD5Controller md5 = new MD5Controller();

            if (customers.Count == 0 || String.Compare(md5.GetMD5SumHash(form["password"]), customers[0].Password, true) != 0)
            {
                response["status"] = "-1";
                return Json(response);
            }

            else
            {
                response["status"] = "0";

                //TempData["token"] = customers[0].Email;
               Session["fname"] =customers[0].FirstName;
               Session["lname"] =customers[0].Lastname;
               Session["token"]= customers[0].Email;

                System.Web.Security.FormsAuthentication.SetAuthCookie(customers[0].Email ,true);

                return Json(response);


            }


        }


        }
    }