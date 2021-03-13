namespace SongtrackerPro.Tasks
{
    public interface ITask<in T1, T2>
    {
        TaskResult<T2> DoTask(T1 input);
    }
}