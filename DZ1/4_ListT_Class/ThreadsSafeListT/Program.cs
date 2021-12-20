using System;
using System.Threading;

namespace ThreadsSafeListT
{
    class Program
    {
        static SafeList<int> safeList = new SafeList<int>();

        static void Main(string[] args)
        {
            /// Task 4 
            /// Создайте класс-обертку над List<T>, что бы можно было добавлять и удалять элементы из разных потоков без ошибок.
            /// 
            Console.WriteLine("Ожидаем 20 секунд результатов добавления/удаления в List" );
       
            safeList.Add(1);
            
            Thread threadAdd = new Thread(AddItemsInList);
            threadAdd.Start();
            
            Thread threadRemove = new Thread(RemoveItemsFromList);
            threadRemove.Start();

            //подождем исполнения в потоках
            Thread.Sleep(20000);

            //посмотрим что внутри осталось
            //ожидается, что останется только '1' из 17й строки кода
            Console.WriteLine("Выводим содержимое List, ожидаемый результат 1 элемент с содержимым '1'");
            foreach (int i in safeList)
            {
                Console.WriteLine($"main thread, content of list[{i}] = " + i);
            }
        }

        private static void RemoveItemsFromList()
        {
            int errorCounter = 0;

            for (int i = 0; i < 999; i++)
            {
                bool result = safeList.Remove(10);
                if (!result)
                {
                    errorCounter++;
                }
                Thread.Sleep(10);
            }

            Console.WriteLine("Ошибок удаления = " + errorCounter);
        }

        private static void AddItemsInList()
        {
            for (int i = 0; i < 999; i++)
            {
                safeList.Add(10);
                Thread.Sleep(5);
            }
        }
    }
}
