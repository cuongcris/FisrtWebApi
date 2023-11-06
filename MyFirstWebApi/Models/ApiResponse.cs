namespace MyFirstWebApi.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Messenger { get; set; }   
        public Object Data { get; set; }
    }
}
