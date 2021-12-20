using LoggingLib;
using ScanerPhisicalDevice;
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

            Console.WriteLine("эмулятор устройства сканера");
            
            //количество выполнений
            int count = 8;

            ILogging log = new Logging();
            IScaner scanner = new Scanner(log);

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
