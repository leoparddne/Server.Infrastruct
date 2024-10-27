namespace Server.Infrastruct.Model.Models.ExceptionExtention
{

    public class HttpCodeException : Exception
    {
        public int HttpCode { get; set; }

        public HttpCodeException(int code, string message) : base(message)
        {
            HttpCode = code;
        }
    }
}
