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
        //
        // do poprawy bądź usunięcia
        //
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
            string query = $@" UPDATE Folders SET name = @FolderName WHERE id = @FolderID";

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




        // usunFolderZZawartocia(id)
        //	    idPodfolderow = dajMiPodfoldery(id)                 1
        //	    zdiciaWFolderze = dajMiZdieciaWFolderze(id)         2
        //	    for(zdiecie in zdiciaWFolderze)                     3
        //		    usunZdiecie(zdiecie )                           4
        //	    for(idPodfolderu in idPodfolderow)                  5
        //		    usunFolderZZawartocia(idPodfolderu )            6
        //	    usunFolder(id)                                      7


        // Do zrefaktorania 
        // Jalepiej przeniść tą funkcjię do bazy danych
        // Ograniczyć głębokość rekurencji żeby nie przebiło stosu

        [HttpPost("ChangeParent")]
        public void ChangeParent([FromForm] IDAndParentID data)
        {
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");
            SqlDataReader myReader;
            string query = $@" UPDATE Folders SET arentId = @ParentID WHERE id = @FolderID";

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

        [HttpDelete("{id}")]
        public void DeleteWithContents(int id)
        {
            Console.WriteLine(id);
            string sqlDataSource = _configuration.GetConnectionString("PhotoAppCon");

            // Usuwanie zdięć

            string deletePhotosInCurrentFolder = "DELETE FROM Photos WHERE FolderID = @currentFolder";

            DataTable deletedPhosot = new DataTable();
           

            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();

                SqlDataReader myReader;
           
                using (SqlCommand myCommand = new SqlCommand(deletePhotosInCurrentFolder, connection))
                {
                    myCommand.Parameters.AddWithValue("@currentFolder", id);
                    myReader = myCommand.ExecuteReader();
                    deletedPhosot.Load(myReader);
                    myReader.Close();

                }


                // Usuwanie Folderów z Zawartością

                string getSubFolderIDsQuery = "SELECT Folders.id FROM Folders WHERE Folders.parentId = @currentFolder";
                DataTable subFolderIDs = new DataTable();

                using (SqlCommand myCommand = new SqlCommand(getSubFolderIDsQuery, connection))
                {
                    myCommand.Parameters.AddWithValue("@currentFolder", id);

                    //  1 Pobieranie ID podfoderów
                    myReader = myCommand.ExecuteReader();
                    subFolderIDs.Load(myReader);
                    myReader.Close();

                    foreach (DataRow subFolder in subFolderIDs.Rows)
                    {
                        Console.WriteLine(subFolder["id"]);

                        // rekurencja
                        DeleteWithContents(int.Parse(subFolder["id"].ToString()));
                    }
                }

                // warunek wyjściowy
                // usunięcie obecnego folderu
                DataTable table = new DataTable();
                string deleteCurrentFolderQuery = "DELETE FROM Folders WHERE id = @currentFolder";
                using (SqlCommand myCommand = new SqlCommand(deleteCurrentFolderQuery, connection))
                {
                    myCommand.Parameters.AddWithValue("@currentFolder", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
        }


    }
}
