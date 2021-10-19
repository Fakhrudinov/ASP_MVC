using MiniThreadPool.MyAction;
using MiniThreadPool.ThreadPool;
using System;

namespace MiniThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("miniThreadPool");

            MyThreadPool myTreadPool = new MyThreadPool(4);

            Console.WriteLine("Threads count from main thread = " + myTreadPool.GetCountOfThreads());
            for (int i = 0; i < 15; i++)
            {
                myTreadPool.Enqueue(new MyTask(i));
            }
            Console.ReadKey();
            myTreadPool.WaitForJoinThread();    
        }
    }
}
