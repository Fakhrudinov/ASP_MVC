namespace MiniThreadPool.Interfaces
{
    public interface IAction
    {
        decimal Action();
        int GetActionParameter();
    }
}
