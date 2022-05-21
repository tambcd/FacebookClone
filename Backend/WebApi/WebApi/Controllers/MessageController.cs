using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MessageController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        public JsonResult Get(string id)
        {
            string query = @"exec ListMessage '" + id + @"'";
            ;
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
        public JsonResult Post(string ID, string Titile, string date, string Attachments ,string ID_RoomChat, string idtk)
        {
            string query = @"insert into dbo.message values ('" + ID + @"' , '" + Titile + @"', '" + date + @"' ,'" + Attachments + @"' ,'" + ID_RoomChat + @"','" + idtk + @"',0)";
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
            return new JsonResult("message Added Successfull");


        }

        [HttpPatch]
        public JsonResult Patch(string id)
        {
            string query = @"UPDATE message set typess = 1 where ID_message = '" + id + @"'";
            ;
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
            return new JsonResult("delete message Successfull");

        }
    }
}
