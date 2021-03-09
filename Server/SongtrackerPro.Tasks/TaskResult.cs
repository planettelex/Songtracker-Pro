namespace SongtrackerPro.Tasks
{
    public class TaskResult<T> 
    {
        public TaskResult(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        public TaskResult(TaskException exception)
        {
            Success = false;
            Exception = exception;
        }

        public bool Success { get; }

        public T Data { get; }

        public TaskException Exception { get; }
    }
}
