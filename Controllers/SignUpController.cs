using LoginApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoginApp.Controllers
{
    public class SignUpController : Controller
    {
        static public int InsertUser(string query)
        {

            MySqlConnection connection = new MySqlConnection(ConstantModel.ConnectionString);
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;
            connection.Open();
            int result = command.ExecuteNonQuery();
            Console.WriteLine(result);
            connection.Close();
            return result;


        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyForm"></param>
        /// <returns></returns>
        public int PushUser(FormCollection MyForm)
        {
            SignUpModel Sign = new SignUpModel();

            Sign.FirstName = MyForm["firstname"];
            Sign.LastName = MyForm["lastname"];
            Sign.Email = MyForm["email"]; 
            Sign.Password = MyForm["password"];

            Stream sr = Request.Files[0].InputStream;

            BinaryReader br = new BinaryReader(sr);
            
           byte[] imageData = br.ReadBytes((int)sr.Length);


            MySqlConnection connection = new MySqlConnection(ConstantModel.ConnectionString);
            MySqlCommand command = new MySqlCommand($@"insert into users values(@fname , @lname , @email , MD5(@password) , @picture);");
            command.Parameters.Add("@fname",MySqlDbType.VarChar);
            command.Parameters.Add("@lname", MySqlDbType.VarChar);
            command.Parameters.Add("@email", MySqlDbType.VarChar);
            command.Parameters.Add("@password", MySqlDbType.VarChar);
            command.Parameters.Add("@picture", MySqlDbType.MediumBlob);
            command.Parameters["@fname"].Value = Sign.FirstName;
            command.Parameters["@lname"].Value = Sign.LastName;
            command.Parameters["@email"].Value = Sign.Email;
            command.Parameters["@password"].Value = Sign.Password;
            command.Parameters["@picture"].Value = imageData;

            command.Connection = connection;
            connection.Open();
            int result = command.ExecuteNonQuery();
            Console.WriteLine(result);
            connection.Close();
            return result;

        }

        [HttpPost]
        public async Task<int> CheckValidEmail()
        {

            StreamReader y = new StreamReader(Request.InputStream);
            string body = await y.ReadToEndAsync();
            string query = $@"SELECT email FROM users where email= ""{body}""";
            MySqlConnection connection = new MySqlConnection(ConstantModel.ConnectionString);
            Console.WriteLine(query);
            MySqlCommand command = new MySqlCommand(query);
            command.Connection = connection;
            connection.Open();
            MySqlDataReader sdr = command.ExecuteReader();
            int rowsfetched = 0;

            while (sdr.Read())
            {
                ++rowsfetched;
            }

            connection.Close();
            Console.WriteLine(rowsfetched);
            return rowsfetched;


        }



       

        public ActionResult Loader()
        {

            return View("test");
        }

    }
}


