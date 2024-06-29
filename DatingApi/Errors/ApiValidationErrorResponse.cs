namespace DatingApi.Errors
{
    public class ApiValidationErrorResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
           
        }
        public ApiValidationErrorResponse(int code , string message , List<string>errors):base(code, message)
        {
            Errors = errors;
        }
    }
}
