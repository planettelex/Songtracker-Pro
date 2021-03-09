using System;

namespace SongtrackerPro.Tasks
{
    public class TaskException : Exception
    {
        public TaskException() { }

        public TaskException(string message) : base(message) { }

        public TaskException(Exception innerException, string message = TaskError) : base(message, innerException) { }

        public const string TaskError = "The task errored with the following exception:";
    }
}
