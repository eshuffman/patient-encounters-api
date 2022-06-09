namespace HealthAPI.Utilities.HttpResponseExceptions
{
    public interface IHttpResponseException
    {
        public HttpResponseExceptionValue Value { get; set; }
    }
}
