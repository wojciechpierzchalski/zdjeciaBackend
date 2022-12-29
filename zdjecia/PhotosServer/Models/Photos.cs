using System.Drawing;

namespace PhotosServer.Models
{
    public class Photos
    {
        public int PhotoID { get; set; }
        public string PhotoName { get; set; }
        public int FolderID { get; set; }
        public string PhotoDate { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? PhotoFile { get; set; }

        public override string ToString()
        {
            return $"{PhotoID},{PhotoName},{FolderID}, {PhotoDate}, {PhotoPath}, {PhotoFile}";
        }
    }
}
