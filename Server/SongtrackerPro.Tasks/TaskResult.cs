namespace SongtrackerPro.Tasks
{
    public class TaskResult<T>
    {
        public TaskResult(bool success)
        {
            Success = success;
        }

        public TaskResult(T data)
        {
            Success = data != null;
            Data = data;
        }

        public TaskResult(TaskException exception)
        {
            Success = false;
            Exception = exception;
        }

        public bool Success { get; }

        public T Data { get; }

        public bool HasNoData => Data == null; 

        public TaskException Exception { get; }

        public bool HasException => Exception != null;
    }
}
