namespace Ecom.Jarad.API.Errors
{
    public class APIException : BaseResponse
    {
        public APIException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
