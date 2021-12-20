using LoggingLib;
using ScanerProcessor.Interfaces;
using ScanerProcessor.Models;
using ScanerProcessor.ScanHandlers;

namespace ScanerProcessor
{
    public class WorkWithScan : IWorkWithScan
    {
        private ILogging _log;

        private PngHandler _png;
        private PdfHandler _pdf;
        private JpegHandler _jpeg;

        public WorkWithScan(ILogging log, PngHandler png, PdfHandler pdf, JpegHandler jpeg)
        {
            _log = log;

            _png = png;
            _pdf = pdf;
            _jpeg = jpeg;
        }

        public void AnalizeIncomingScan(ScanModel scannedDocument)
        {
            // на паттерне
            // Цепочка обязанностей // Chain of Responsibility // CoR // Chain of Command

            // цепочка обработки
            _pdf.SetNext(_jpeg).SetNext(_png);

            // Отправка скана в цепочку обработчиков, начиная с pdf
            TryToHandleScan(_pdf, scannedDocument, _log);
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
