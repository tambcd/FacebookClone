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
    public class ChatRoomController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ChatRoomController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        public JsonResult Get(string id)
        {
            string query = @"exec listRoomchat '" + id + @"'";
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

        [HttpGet("Room")]
        public JsonResult GetRoom(string idRoom)
        {
            string query = @"exec listRoomchat '" + idRoom + @"'";
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
        public JsonResult Post( string nickname, string ID, string ID_RoomChat)
        {
            string query = @"insert into dbo.participation values (0 , '" + nickname + @"', '" + ID + @"' , '" + ID_RoomChat + @"')";
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
            return new JsonResult(" Added Successfull");


        }
        [HttpPost("CreateRoom")]
        public JsonResult PostRoom(string ID_RoomChat, string Name_RoomChat, string avatar,string idtk)
        {
            string query = @"insert into dbo.RoomChat values ('" + ID_RoomChat + @"' , '" + Name_RoomChat + @"',0, '" + avatar + @"' , '" + idtk + @"')";
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
            return new JsonResult(" Added Successfull");


        }
    }
}
