using Autofac;
using LoggingLib;
using ScanerPhisicalDevice;
using ScanerProcessor;
using ScanerProcessor.Interfaces;
using ScanerProcessor.ScanHandlers;
using System;
using System.Threading;

namespace EmulateScaner
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Сделайте эмулятор устройства сканера. 
            /// Он сканирует (берет данные из какого либо файла), 
            /// производит фейковые данные о загрузке процессора и памяти. 
            /// Код должен быть прост, и дальнейшую работу стоит вести только с контрактами данного устройства. 
            /// 
            /// Разработать небольшую библиотеку, которая 
            /// * принимает от этого эмулятора байты, 
            /// * сохраняет в различные форматы 
            /// * и мониторит его состояние, 
            /// * записывая в какой-либо лог.
            /// 
            /// Добавлен autofac


            Console.WriteLine("эмулятор устройства сканера");

            //количество выполнений
            int count = 8;

            var builder = new ContainerBuilder();

            //регистрируем основные компоненты сканера
            builder.RegisterType<Logging>().As<ILogging>();
            builder.RegisterType<Scanner>().As<IScaner>();
            builder.RegisterType<WorkWithScan>().As<IWorkWithScan>();

            // регистрируем обработчики
            builder.RegisterType<PngHandler>().AsSelf();
            builder.RegisterType<PdfHandler>().AsSelf();
            builder.RegisterType<JpegHandler>().AsSelf();

            IContainer container = builder.Build();

            ILogging log = container.Resolve<ILogging>();
            IScaner scanner = container.Resolve<IScaner>();

            for (var i = 0; i < count; i++)
            {
                Thread.Sleep(2345);
                //нажимаем кнопку сканировать
                log.LogWrite("Scan initiated");
                scanner.Scan();
            }

            Console.ReadLine();
        }
    }
}
