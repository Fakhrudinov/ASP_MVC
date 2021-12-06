using LoggingLib;
using ScanerProcessor.Interfaces;
using ScanerProcessor.Models;
using ScanerProcessor.ScanHandlers;

namespace ScanerProcessor
{
    public class WorkWithScan : IWorkWithScan
    {
        private ILogging _log;
        public WorkWithScan(ILogging log)
        {
            _log = log;
        }

        public void AnalizeIncomingScan(ScanModel scannedDocument)
        {
            // на паттерне
            // Цепочка обязанностей // Chain of Responsibility // CoR // Chain of Command
            
            // Создаем обработчики
            var png = new PngHandler();
            var jpeg = new JpegHandler();
            var pdf = new PdfHandler();
            //цепочка обработки
            pdf.SetNext(jpeg).SetNext(png);

            // Отправка скана в цепочку обработчиков, начиная с pdf
            TryToHandleScan(pdf, scannedDocument, _log);
        }

        private void TryToHandleScan(IHandler handler, ScanModel scannedDocument, ILogging _log)
        {
            var result = handler.Handle(scannedDocument, _log);

            if (result != null)
            {
                _log.LogWrite($"Processed: {result}\r\n");
            }
            else
            {
                _log.LogWrite($"Error! Document with format {scannedDocument.ScanFormat}: " +
                    $"no equal handlers was found\r\n");
            }
        }
    }
}
