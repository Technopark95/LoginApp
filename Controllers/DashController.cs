using LoginApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginApp.Controllers
{

    public class DashController : Controller
    {
       
        public ActionResult Index()
        {


            TempData["token"] = Session["token"];

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["fname"] = Session["fname"];
            TempData["lname"] = Session["lname"];
            Console.Write(TempData["token"]);
            return View("Dash");
        }

        public ActionResult Logout()
        {
            Session.Clear();

            System.Web.Security.FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");


        }



        public ActionResult Imagers()
        {

            MySqlConnection connection = new MySqlConnection(ConstantModel.ConnectionString);

            MySqlCommand command = new MySqlCommand($@" select picture from users where email=""{Session["token"]}""; ");
            command.Connection = connection;
            connection.Open();
            MySqlDataReader sdr = command.ExecuteReader();

            sdr.Read();

            return File((byte[])sdr["picture"], "application/octet-stream");



        }



    }
}
