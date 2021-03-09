namespace SongtrackerPro.Tasks
{
    public interface ITask<T>
    {
        TaskResult<T> DoTask();
    }
}