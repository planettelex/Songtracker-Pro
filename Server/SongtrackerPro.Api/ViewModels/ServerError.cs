using System;
using SongtrackerPro.Utilities.Extensions;

namespace SongtrackerPro.Api.ViewModels
{
    public class ServerError
    {
        public ServerError(Exception exception)
        {
            Type = nameof(ServerError);
            TimeStamp = DateTime.Now;
            Message = exception.Message;
            Exception = exception.InnerException?.Message.SpaceUniformly();
            StackTrace = exception.InnerException?.StackTrace?.SpaceUniformly();
        }

        public string Type { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public string StackTrace { get; set; }
    }
}
