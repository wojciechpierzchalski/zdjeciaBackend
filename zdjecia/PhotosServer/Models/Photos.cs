namespace PhotosServer.Models
{
    public class Photos
    {
        public int PhotoID { get; set; }
        public string PhotoName { get; set; }
        public string PhotoFolder { get; set; }
        public string PhotoDate { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
