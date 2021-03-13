namespace SongtrackerPro.Tasks
{
    public class TaskResult<T>
    {
        public TaskResult(T data)
        {
            Success = true;
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
