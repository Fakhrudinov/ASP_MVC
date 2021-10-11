using System;
using System.Threading;

namespace InterruptThread
{
    class Program
    {
        /// Task 3
        /// Изучите внимательно статичный класс Thread и не статичный класс. 
        /// Найдите метод, который может прервать выполняющийся поток 
        /// и зафиксируйте ту ошибку, которая формируется при отмене.

        static void Main(string[] args)
        {
            Console.WriteLine("Task3");
            CancellationTokenSource cts = new CancellationTokenSource();

            Thread threadF = new Thread(new ParameterizedThreadStart(Foo));
            threadF.Start(cts.Token);

            Thread threadB = new Thread(new ParameterizedThreadStart(Bar));
            threadB.Start(cts.Token);

            //подождем и пошлем отмену
            Thread.Sleep(500);
            cts.Cancel();

            //почистим за собой
            cts.Dispose();
        }

        static void Foo(object tokenObj)
        {
            CancellationToken ct = (CancellationToken)tokenObj;
            Console.WriteLine("Foo start");

            // вариант 1
            while (!ct.IsCancellationRequested)
            {
                //ct.ThrowIfCancellationRequested();//эта строка безсмысленна в данном случае - while не зайдет в этот контекст при запросе cts.Cancel();
                Console.WriteLine("Статус отмены Foo " + ct.IsCancellationRequested);
                Thread.Sleep(20);
            }

            Console.WriteLine("Статус отмены Foo " + ct.IsCancellationRequested);
            Console.WriteLine("Foo thread stop");
        }

        static void Bar(object tokenObj)
        {
            CancellationToken ct = (CancellationToken)tokenObj;
            Console.WriteLine("Bar thread start");
            
            //вариант 2
            bool exit = false;
            while (!exit)
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    Console.WriteLine("Статус отмены Bar " + ct.IsCancellationRequested);
                    Thread.Sleep(20);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Статус отмены Bar " + ct.IsCancellationRequested);
                    exit = true;
                }
            }

            Console.WriteLine("Bar thread stop");
        }
    }
}
