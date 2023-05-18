namespace Diploma.Bll.Services.Files.Request
{
    public class CreateFileRequest
    {
        public string Folder { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }
}