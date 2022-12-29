namespace PhotosServer.Models
{
    public class Folder
    {
        public int FolderID { get; set;}
        public string? FolderName { get; set;}
        public string? FolderDate { get; set;}
        public int ParentID {get; set;}

    }
}
