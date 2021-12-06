using FilesManager;
using LoggingLib;
using ScanerProcessor.Models;
using System;

namespace ScanerProcessor.ScanHandlers
{
    class PngHandler : AbstractHandler
    {
        public override object Handle(ScanModel scan, ILogging _log)
        {
            if (scan.ScanFormat == (int)EnumImageFormats.png)
            {
                _log.LogWrite("Set encoder - " + EnumImageFormats.png);

                IFileManager fileManager = new FileManager(_log);

                FileModel newFile = new FileModel(
                    EnumImageFormats.png.ToString(), // Encoder/file format
                    scan.ScannedDocument, // file content
                    "scan-" + DateTime.Now.ToString("HH-mm-ss.") + EnumImageFormats.png.ToString()); // file name

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
