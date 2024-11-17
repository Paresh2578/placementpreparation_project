namespace backend.Models
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public dynamic?  Data { get; set; }
    }
}
