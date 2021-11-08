using LoggingLib;
using ScanerProcessor;
using ScanerProcessor.Interfaces;
using ScanerProcessor.Models;
using System;

namespace ScanerPhisicalDevice
{
    public class Scanner : IScaner
    {
        private ILogging _log;
        public Scanner(ILogging log)
        {
            _log = log;
        }

        public void Scan()
        {
            ScanModel scannedDocument = new ScanModel();
            Random rnd = new Random();

            // рандом - в каком формате происходит сканирование            
            scannedDocument.ScanFormat = rnd.Next(0, Enum.GetNames(typeof(EnumImageFormats)).Length);
            _log.LogWrite($"Scanned document format=({scannedDocument.ScanFormat}) " +
                $"{Enum.GetName(typeof(EnumImageFormats), scannedDocument.ScanFormat)}");

            // загрузка процессора во время сканирования
            int loadCPU = rnd.Next(5, 100);
            // загрузка памяти во время сканирования
            int loadRAM = rnd.Next(5, 100);
            _log.LogWrite($"Load levels when perform scan: " +
                $"Cpu={loadCPU}% " +
                $"Memory={loadRAM}%");

            // генерируем массив байт как результат сканирования
            scannedDocument.ScannedDocument = new Byte[rnd.Next(100, 200)];
            rnd.NextBytes(scannedDocument.ScannedDocument);

            _log.LogWrite("Physical scan complete, begin work with scanned data, size=" 
                + scannedDocument.ScannedDocument.Length);

            IWorkWithScan newWork = new WorkWithScan(_log);

            newWork.AnalizeIncomingScan(scannedDocument);       
        }
    }
}
