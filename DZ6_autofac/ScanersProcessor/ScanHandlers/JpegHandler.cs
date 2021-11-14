using FilesManager;
using LoggingLib;
using ScanerProcessor.Models;
using System;

namespace ScanerProcessor.ScanHandlers
{
    public class JpegHandler : AbstractHandler
    {
        public override object Handle(ScanModel scan, ILogging _log)
        {
            if (scan.ScanFormat == (int)EnumImageFormats.jpeg)
            {
                _log.LogWrite("Set encoder - " + EnumImageFormats.jpeg);

                IFileManager fileManager = new FileManager(_log);                

                FileModel newFile = new FileModel(
                    EnumImageFormats.jpeg.ToString(), // Encoder/file format
                    scan.ScannedDocument, // file content
                    "scan-" + DateTime.Now.ToString("HH-mm-ss.") + EnumImageFormats.jpeg.ToString()); // file name

                //отправляем данные на запись в файл
                fileManager.WriteScanToFileAsync(newFile);

                return $"{newFile.FileName} with encoder {newFile.Encoder}";
            }
            else
            {
                return base.Handle(scan, _log);
            }
        }
    }
}
