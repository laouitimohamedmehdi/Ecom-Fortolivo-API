namespace Ecom.API.Errors
{
    public class ApiValidationErrorResponse : BaseCommonResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
        }
    }
}
