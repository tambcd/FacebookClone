using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApi.Models;
using System.Text;
using System;
using System.Security.Cryptography;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public static string mahoa(string toEncrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("IloveU"));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes("IloveU");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /* public static string GiaiMa(string toDecrypt)
         {
             bool useHashing = true;
             byte[] keyArray;
             byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

             if (useHashing)
             {
                 MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                 keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("IloveU"));
             }
             else
                 keyArray = UTF8Encoding.UTF8.GetBytes("IloveU");

             TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
             tdes.Key = keyArray;
             tdes.Mode = CipherMode.ECB;
             tdes.Padding = PaddingMode.PKCS7;

             ICryptoTransform cTransform = tdes.CreateDecryptor();
             byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

             return UTF8Encoding.UTF8.GetString(resultArray);
         }*/
        [HttpGet("alluserchat")]
        public JsonResult Getall(string id)
        {
            string query = @"exec userchat '" + id + @"'";
            DataTable table = new DataTable();
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }



            }
            return new JsonResult(table);

        }
        [HttpGet]
        public JsonResult Get(string Username,string Password)
              {
                  string query = @"select * from dbo.Accounts where ID = '" + Username + @"' and  Pass = '" + mahoa(Password) + @"'";
                  DataTable table = new DataTable();
                  string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
                  SqlDataReader myReader;
                  using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
                  {

                      myCon.Open();
                      using (SqlCommand myCommand = new SqlCommand(query, myCon))
                      {
                          myReader = myCommand.ExecuteReader();
                          table.Load(myReader);
                          myReader.Close();
                          myCon.Close();
                      }              



               }       
                     return new JsonResult(table);

              }
        [HttpGet("userinf")]
        public JsonResult GetUser(string Username)
        {
            string query = @"select lastName,FirsName,gender,story,Phone,countryside,dateOfBrith,avatar,wall,school,email from dbo.Accounts where ID = '" + Username + @"'";
            DataTable table = new DataTable();
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }



            }
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(string id, string passw, string lastName,string firstName,string email ) 
              {
            var wall = "https://img4.thuthuatphanmem.vn/uploads/2020/05/07/hinh-anh-anh-nen-anime-4k-dep_094210852.jpg";
            var avatar = "https://img4.thuthuatphanmem.vn/uploads/2020/05/13/anh-nen-anime-4k-cho-may-tinh_062423437.jpg";
                string query = @"insert into dbo.Accounts (ID,Pass,lastName,FirsName,avatar,wall,email) values ('" + id + @"' , '" + mahoa(passw) + @"', '" + lastName + @"' , '" + firstName + @"' ,'" + avatar + @"','" + wall + @"' , '" + email + @"')";
                string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
                {

                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(ex);
                    }
                        
                    }
                    


            }
            return new JsonResult("Added Successfull");


        }
             
        [HttpPatch]
        public JsonResult Patch(Accounts accounts)
        {
            string query = @"update dbo.Accounts set Pass ='" + mahoa(accounts.pass) + @"',lastName = '" + accounts.lastName + @"' ,FirsName = '" + accounts.firstName + @"',gender = '" + accounts.gender + @"' ,story = '" +  accounts.story + @"',Phone ='" + accounts.phone + @"' , countryside='" + accounts.countryside + @"', dateOfBrith='" + accounts.dateOfBirth + @"' , avatar ='" + accounts.avatar + @"',wall = '" + accounts.wall + @"' , school = '" + accounts.school + @"',email = '" + accounts.email + @"' where ID = '" + accounts.id + @"'";
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Update erro");
                    }

                }



            }
            return new JsonResult("Update Successfull");

        }

        [HttpPatch("updatePass")]
        public JsonResult PatchPass(string id ,string passRe)
        {
            string query = @"update dbo.Accounts set Pass ='" + mahoa(passRe) + @"' where ID = '" + id + @"'";
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Update pass erro");
                    }

                }



            }
            return new JsonResult("Update pass Successfull");

        }

        [HttpPatch("updateavatar")]
        public JsonResult Patchavatr(string id, string avatar)
        {
            string query = @"update dbo.Accounts set avatar ='" + avatar + @"' where ID = '" + id + @"'";
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Update avatar erro");
                    }

                }



            }
            return new JsonResult("Update avatar Successfull");

        }


        [HttpPatch("updateWall")]
        public JsonResult PatchWall(string id, string Wall)
        {
            string query = @"update dbo.Accounts set wall ='" + Wall + @"' where ID = '" + id + @"'";
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Update Wall erro");
                    }

                }



            }
            return new JsonResult("Update Wall Successfull");

        }




        /* [HttpGet]
         public IActionResult Login(string username,string pass)
         {
          UserLogin userLogin = new UserLogin(username,pass);

             IActionResult res = Unauthorized();

             var user = Authenticate(userLogin);
             if (user != null)
             {
                 var TokenStr = GenerateJSOToken(user);
                 res = Ok(new { TokenStr = TokenStr });
             }
             return res;
         }

     private string GenerateJSOToken(Accounts user)
     {
         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
         var claims = new[]
        {
             new Claim(JwtRegisteredClaimNames.Sub, user.id),
             new Claim(JwtRegisteredClaimNames.Email, user.email),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

         };
         var token = new JwtSecurityToken(
             issuer: _configuration["Jwt: Key"],
             audience: _configuration["Jwt:Issuer"],
             claims,
             expires: DateTime.Now.AddMinutes(120),
             signingCredentials: credentials);

         var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
         return encodetoken;
     }

     private Accounts Authenticate(UserLogin userLogin)
         {
             Accounts accounts = null;
             string query = @"select * from dbo.Accounts where ID = '" + userLogin.Username + @"' and  Pass = '" + userLogin.Password + @"' ";
             DataTable table = new DataTable();
             string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
             SqlDataReader myReader;
             using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
             {

                 myCon.Open();
                 using (SqlCommand myCommand = new SqlCommand(query, myCon))
                 {
                     myReader = myCommand.ExecuteReader();
                     table.Load(myReader);
                     myReader.Close();
                     myCon.Close();
                 }

             }
         accounts.id = "tam123";
         accounts.pass = "123";
             return accounts;
         }



     [HttpPost("Post")]
     public string post()
     {
         var identity = HttpContext.User.Identity as ClaimsIdentity;
         IList<Claim> claims = identity.Claims.ToList();
         var userName = claims[0].Value;
         return "Welcome to " + userName;
     }
     [HttpGet("Getvalue")]
     public ActionResult<IEnumerable<string>> Get()
     {
         return new string[] { "value1", "value31", "value2" };
     }*/
    }
}
