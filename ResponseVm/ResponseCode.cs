namespace Application.Constants
{
    public static class ResponseCode
    {
        public const int Success = 200;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int InternalServerError = 500;

       
        public const int ValidationError = 1001;
        public const int DataNotFound = 1002;
        public const int DuplicateEntry = 1003;
    }
}
