using Microsoft.AspNetCore.Mvc;
using PhotosServer.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

namespace PhotosServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PhotosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<PhotosController>
        [HttpGet]
        public JsonResult Get()
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            Photos photo = new Photos();
            string query = @" SELECT * FROM photos";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }

            }
            return new JsonResult(table);
        }

        // GET api/<PhotosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" SELECT * FROM photos WHERE PhotoID = {id}";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }

            }
            return new JsonResult(table);

        }

       

        //GET serwer/api/Photos/GetPhoto/ID
        [HttpGet("GetPhoto/{id}")] 
        public IActionResult SendImage(int id)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" SELECT PhotoPath FROM photos WHERE PhotoID = {id}";
            DataTable table = new DataTable();
            string photoPath = "";
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    using (var reader = myCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            photoPath = reader.GetValue(0).ToString();
                        }
                    }
                    connection.Close();
                }

            }

            Byte[] b = System.IO.File.ReadAllBytes(photoPath);
            return File(b, "image/jpeg");
        }
        // POST api/<PhotosController>
        [HttpPost]
        public void Post([FromForm] Photos photo)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            photo.PhotoPath = $"../../../Zdjecia/{photo.PhotoFile.FileName}";
            SqlDataReader myReader;
            string query = $@" INSERT INTO photos VALUES(@PhotoName, @PhotoDate, @PhotoPath,  @FolderID)";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@PhotoName",photo.PhotoName);
                    myCommand.Parameters.AddWithValue("@FolderID", photo.FolderID);
                    myCommand.Parameters.AddWithValue("@PhotoDate", photo.PhotoDate);
                    myCommand.Parameters.AddWithValue("@PhotoPath", photo.PhotoPath);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            using (var stream = new FileStream(photo.PhotoPath,FileMode.Create,FileAccess.ReadWrite))
            {
                photo.PhotoFile.CopyTo(stream);
            }

        }

        // PUT api/<PhotosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromForm] Photos photo)
        {
            SqlDataReader myReader;
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            string query = $@"
                           update photos
                           set PhotoName=@PhotoName,
                            FolderID=@FolderID,
                            PhotoDate=@PhotoDate
                            where PhotoID=@PhotoID
                            ";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@PhotoName", photo.PhotoName);
                    myCommand.Parameters.AddWithValue("@FolderID", photo.FolderID);
                    myCommand.Parameters.AddWithValue("@PhotoDate", photo.PhotoDate);
                    myCommand.Parameters.AddWithValue("@PhotoID", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
        }

        // DELETE api/<PhotosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            SqlDataReader myReader;
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            string query = $@"
                           delete from photos where PhotoID=@PhotoID
                            ";
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@PhotoID", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
        }
    }
}
