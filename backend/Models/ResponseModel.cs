namespace backend.Models
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public dynamic?  Data { get; set; }
    }
}
