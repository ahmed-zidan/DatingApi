namespace DatingApi.Errors
{
    public class ApiResponseDetail:ApiResponse
    {
        public string Detail { get; set; }
        public ApiResponseDetail() : base(400)
        {
            
        }
        public ApiResponseDetail(int code , string message,string detail):base(code,message)
        {
            Detail = detail;
        }
    }
}
