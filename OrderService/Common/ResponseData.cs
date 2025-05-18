namespace OrderService.Common
{
    public class ResponseData<T>
    {
        public ResponseData()
        {
            result = 0;
            error = new Error();
        }
        public ResponseData(int result, int code, string message)
        {
            this.result = result;
            error = new Error(code, message);
        }
        public int result { get; set; }
        public T data { get; set; }
        public Error error { get; set; }
    }
    public class Error
    {
        public Error(int _code, string _message)
        {
            code = _code;
            message = _message;
        }
        public Error()
        {
            code = 200;
            message = string.Empty;
        }
        public int code { get; set; }
        public string message { get; set; }
    }
}
