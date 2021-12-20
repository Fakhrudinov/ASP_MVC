namespace MiniThreadPool.Interfaces
{
    public interface IMyThreadPool
    {
        int GetCountOfThreads();
        void WaitForJoinThread();
        void Enqueue(IAction task);
    }
}
