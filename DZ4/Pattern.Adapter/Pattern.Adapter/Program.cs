using Pattern.Adapter.Model;
using Pattern.Adapter.Repositories;
using Pattern.Adapter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Adapter
{
    class Program
    {
        public static readonly CancellationTokenSource cts = new CancellationTokenSource();        

        static async Task Main(string[] args)
        {
            /// 1.	Придумайте небольшое приложение консольного типа, который берет различные Json структуры 
            /// (предположительно из разных веб сервисов), олицетворяюющие товар в магазинах. 
            /// Структуры не похожи друг на друга, но вам нужно их учесть, сделать универсально. 
            /// Структуры на ваше усмотрение.

            Console.WriteLine("Pattern Adapter realisation.");

            IDirectRepository dr = new DirectRepository();

            RemoteRepositoryAdaptee adaptee = new RemoteRepositoryAdaptee();
            IRemoteRepository target = new RemoteRepositoryAdapter(adaptee);

            // Здесь создаем набор задач
            var taskDirectFileData = dr.ReadTextDirectlyAsync(@".\ExampleFiles\product1.json");
            var taskRemoteFileData = target.ReadTextRemoteAsync(@".\ExampleFiles\product2.json");
            var taskRemoteXmlFileData = target.ReadTextXMLRemoteAsync(@".\ExampleFiles\product3.xml");

            var taskDirectHttpData = dr.GetHttpDirectRequest(@"https://api.jsonbin.io/b/617b8ba89548541c29ca0d71/1", cts);
            var taskRemoteHttpData = target.GetHttpRemoteRequest(@"https://api.jsonbin.io/b/617b8c37aa02be1d446054a1/1", cts);

            var tasks = new List<Task<ResponceObject>>();
            tasks.AddRange(new[] { taskDirectFileData, taskRemoteFileData, taskRemoteXmlFileData, taskDirectHttpData, taskRemoteHttpData });

            //отправляем задачи на выполнение
            Console.WriteLine("Send task pull to execute.");
            try
            {
                //ждать выполнения не более 3сек
                cts.CancelAfter(3000);
                // Ждем, пока все задачи будут готовы
                await Task.WhenAll(tasks);
                Console.WriteLine("All Task complete!");
            }
            catch (Exception oce)
            {
                Console.WriteLine("Exception thrown when task executing - " + oce.Message);
            }
            finally
            {
                cts.Dispose();
            }

            //успешные таски обработаем
            foreach (var task in tasks)
            {
                if (task.IsCompletedSuccessfully && task.Result != null)
                {
                    Console.WriteLine($"id: {task.Result.id}, " +
                        $"from: {task.Result.source}, " +
                        $"name:{task.Result.name}, " +
                        $"desc: {task.Result.description}, " +
                        $"price: {task.Result.price}");
                }
            }

            Console.WriteLine("Jobs done.");
            Console.ReadLine();
        }
    }
}


