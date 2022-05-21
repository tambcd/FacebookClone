using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CommentsController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        public JsonResult Get(string idPost)
        {
            string query = @"exec ListComment ";
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
        public JsonResult Post(string idcmt, string Titile, string Attachments, string id, string ID_Content, string date)
        {
            string query = @"insert into dbo.Comments values ('" + idcmt + @"' , '" + Titile + @"', '" + Attachments + @"' , '" + id + @"','" + ID_Content + @"' , '" + date + @"')";
            string sqlDataSoure = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoure))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    try {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                    }
                    catch(Exception ex)
                    {
                        return new JsonResult("Comment name already exists");
                    }              
                                                                  

                }

            }
            return new JsonResult("Comment Added Successfull");


        }
    }
}
