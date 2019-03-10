namespace restapi.Models
{
    public class FileNotFoundError
    {
        public int ErrorCode { get => 100; }

        public string Message { get => "No file with name provided was found"; }
    }
}