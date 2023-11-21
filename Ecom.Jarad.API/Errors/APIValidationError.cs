namespace Ecom.Jarad.API.Errors
{
    public class APIValidationError:BaseResponse
    {
        public APIValidationError() : base(400)
        {
        }
        public IEnumerable<string> Error { get; set; }
    }
}
