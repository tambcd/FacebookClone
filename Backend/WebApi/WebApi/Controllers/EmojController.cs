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
    public class EmojController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmojController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet]
        public JsonResult Get(string idPost,string id)
        {
            string query = @"select ID_emotion,TypeEmotion ,nameEmoj from emotion where emotion.ID_Content ='" + idPost + @"'and emotion.id= '" + id + @"'";
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
        public JsonResult Post(string ID_emotion, string TypeEmotion,string ID_Content, string Idcmt, string id, string nameEmoj)
        {
            string query = @"INSERT INTO dbo.emotion VALUES ('" + ID_emotion + @"' , '" + TypeEmotion + @"', '" + ID_Content + @"' ,'" + Idcmt + @"' ,'" + id + @"','" + nameEmoj + @"' )";
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
            return new JsonResult("like Added Successfull");

        }
        [HttpPatch]
        public JsonResult Patch(string idEm, string typeEm,string nameEm)
        {
            string query = @"update dbo.emotion set TypeEmotion ='" + typeEm + @"', nameEmoj = '" + nameEm + @"' where ID_emotion = '" + idEm + @"'";
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
                        return new JsonResult("Update emoj erro");
                    }

                }



            }
            return new JsonResult("Update emoj Successfull");

        }

        [HttpDelete]
        public JsonResult Delete(string idEm)
        {
            string query = @"DELETE FROM dbo.emotion where ID_emotion = '" + idEm + @"'";
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
                        return new JsonResult("delete emoj erro");
                    }

                }



            }
            return new JsonResult("delete emoj Successfull");

        }
    }
}
