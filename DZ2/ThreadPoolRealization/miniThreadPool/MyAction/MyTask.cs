using MiniThreadPool.Interfaces;

namespace MiniThreadPool.MyAction
{
    class MyTask : IAction
    {
        private int Index;

        public MyTask(int index)
        {
            Index = index;
        }

        public int GetActionParameter()
        {
            return Index;
        }

        public decimal Action()
        {
            return FibonacciCalculation.GetFibonacci(Index);
        }
    }
}
