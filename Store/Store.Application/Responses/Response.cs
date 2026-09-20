using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Store.Application.Responses
{
    public class Response<T>
    {
        private int _statusCode;

        public Response(ResponseStatus status)
        {
            SetStatus(status);
        }

        public Response(ResponseStatus status, List<string> errors)
        {
            SetStatus(status);
            Errors = errors;
        }

        public Response(ResponseStatus status, T data)
        {
            SetStatus(status);
            Data = data;
        }

        public Response(ResponseStatus status, string message)
        {
            SetStatus(status);
            Message = message;
        }

        public Response(T data)
        {
            SetStatus(ResponseStatus.Succeed);
            Data = data;
        }

        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T Data { get; set; }

        public static JsonResult Succeed()
        {
            return new Response<string>(ResponseStatus.Succeed).ToJsonResult();
        }

        public JsonResult ToJsonResult()
        {
            return new JsonResult(this) { StatusCode = _statusCode };
        }

        private void SetStatus(ResponseStatus status)
        {
            Status = status;

            switch (status)
            {
                case ResponseStatus.Failed:
                    Message = "Failed to complete the request";
                    _statusCode = StatusCodes.Status500InternalServerError;
                    break;
                case ResponseStatus.Succeed:
                    Message = "Completed successfully";
                    _statusCode = StatusCodes.Status200OK;
                    break;
                case ResponseStatus.NotFound:
                    Message = "The requested item was not found";
                    _statusCode = StatusCodes.Status404NotFound;
                    break;
                case ResponseStatus.BadRequest:
                    Message = "Invalid request";
                    _statusCode = StatusCodes.Status400BadRequest;
                    break;
                case ResponseStatus.Unauthorized:
                    Message = "Unauthorized";
                    _statusCode = StatusCodes.Status401Unauthorized;
                    break;
                case ResponseStatus.Forbidden:
                    Message = "Access denied";
                    _statusCode = StatusCodes.Status403Forbidden;
                    break;
            }
        }
    }
}
