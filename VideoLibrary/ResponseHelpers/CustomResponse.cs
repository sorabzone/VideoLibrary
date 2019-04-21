namespace VideoLibrary.ResponseHelpers
{
    public class CustomResponse<TData>
    {
        public StatusCode Code { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }

    public enum StatusCode
    {
        Success = 200,
        Fail = 300,
        Exception = 500,
    }
}
