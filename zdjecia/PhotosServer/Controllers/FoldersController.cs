using Microsoft.AspNetCore.Mvc;
using PhotosServer.Models;
using System.Data.SqlClient;
using System.Data;

namespace PhotosServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public FoldersController(IConfiguration configuration)
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
            string query = @" SELECT * FROM folders";
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
        // POST api/<PhotosController>
        [HttpPost]
        public void Post([FromForm] Folder folder)
        {

            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" INSERT INTO Folders VALUES(@FolderName, @FolderDate, @ParentID)";

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {

                    myCommand.Parameters.AddWithValue("@FolderName", folder.FolderName);
                    myCommand.Parameters.AddWithValue("@FolderDate", folder.FolderDate);
                    myCommand.Parameters.AddWithValue("@ParentID", folder.ParentID);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }

        }


        [HttpPost("Update")]
        public void Update([FromForm] Folder folder)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" UPDATE Folders SET FolderName = @FolderName, FolderDate = @FolderDate, ParentID = @ParentID WHERE FolderId = @FolderID";

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {

                    myCommand.Parameters.AddWithValue("@FolderName", folder.FolderName);
                    myCommand.Parameters.AddWithValue("@FolderDate", folder.FolderDate);
                    myCommand.Parameters.AddWithValue("@ParentID", folder.ParentID);
                    myCommand.Parameters.AddWithValue("@FolderID", folder.FolderID);

                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }


                }           
            }
        }


        [HttpPost("Rename")]
        public void Rename([FromForm] IDAndName data)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" UPDATE Folders SET FolderName = @FolderName WHERE FolderId = @FolderID";

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {

                    myCommand.Parameters.AddWithValue("@FolderName", data.NewName);
                    myCommand.Parameters.AddWithValue("@FolderID", data.FolderID);

                   // try
                    //{
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex);
                    //}
                    //finally
                    //{
                    //    connection.Close();
                    //}


                }
            }
        }

        [HttpPost("ChangeParent")]
        public void ChangeParent([FromForm] IDAndParentID data)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" UPDATE Folders SET ParentID = @ParentID WHERE FolderId = @FolderID";

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, connection))
                {

                    myCommand.Parameters.AddWithValue("@ParentID", data.ParentID);
                    myCommand.Parameters.AddWithValue("@FolderID", data.FolderID);

                    try
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }            
            }
        }


    }
}
