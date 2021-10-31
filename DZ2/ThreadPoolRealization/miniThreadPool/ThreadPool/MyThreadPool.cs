using MiniThreadPool.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;


namespace MiniThreadPool.ThreadPool
{
    public class MyThreadPool : IMyThreadPool
    {
        private BlockingCollection<IAction> _myTaskCollection;
        private List<Thread> _threads;

        private int _maxThreadCount;
        private int _currentThreadCount;

        private ManualResetEvent _manualResetEvent;

        public MyThreadPool(int maxThreadCount)
        {
            _maxThreadCount = maxThreadCount;
            _currentThreadCount = 0;

            _myTaskCollection = new BlockingCollection<IAction>(10);

            _manualResetEvent = new ManualResetEvent(false);
            _threads = new List<Thread>();

            for (int i = 0; i < _maxThreadCount; i++)
            {
                CreateThread();
            }
        }

        public int GetCountOfThreads()
        {
            return _currentThreadCount;
        }

        public void Enqueue(IAction task)
        {
            _manualResetEvent.Set();


            while (_myTaskCollection.TryAdd(task) == false)
            {
                CreateThread();
            }

            _manualResetEvent.Reset();
        }

        private void CreateThread()
        {
            int id = Interlocked.Increment(ref _currentThreadCount);

            if (id > _maxThreadCount)
            {
                Interlocked.Decrement(ref _currentThreadCount);
                return;
            }

            Thread thread = new Thread(ThreadDoAction);
            thread.Name = $"Thread-{id}";
            thread.Start();

            _threads.Add(thread);

            Console.WriteLine($"Thread count : {GetCountOfThreads()}");
        }

        private void ThreadDoAction()
        {
            string threadName = Thread.CurrentThread.Name;
            Console.WriteLine(threadName + " is running");


            while (!_myTaskCollection.IsCompleted)
            {
                IAction task;
                while (_myTaskCollection.TryTake(out task, 100))
                {
                    Console.WriteLine($"{threadName} calculate Fibonacci with index {task.GetActionParameter()}, result is " + task.Action());
                }

                if (_manualResetEvent.WaitOne(2000) == false)
                {
                    if (Interlocked.Decrement(ref _currentThreadCount) < 0)
                    {
                        Interlocked.Increment(ref _currentThreadCount);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Console.WriteLine($"{threadName} are stopped, now thread count is : {GetCountOfThreads()}");
        }

        public void WaitForJoinThread()
        {
            _myTaskCollection.CompleteAdding();
            _manualResetEvent.Set();

            foreach (Thread thread in _threads)
            {
                if (thread != null)
                {
                    thread.Join();
                }
            }
        }
    }
}
