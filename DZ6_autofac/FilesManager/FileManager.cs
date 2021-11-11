using FilesManager.Properties;
using LoggingLib;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesManager
{
    public class FileManager : IFileManager
    {
        private ILogging _log;

        public FileManager(ILogging log)
        {
            _log = log;
        }

        public async Task WriteScanToFileAsync(FileModel newFile)
        {
            // папка по умолчанию для записи файлов сканов
            string folderName = "";
            try
            {
                var resourceManager = Resources.ResourceManager;
                folderName = resourceManager.GetString("ScanFileOutputDir");

                _log.LogWrite($"Default folder for scan files is '{folderName}'");
            }
            catch
            {
                _log.LogWrite("Alert: no default folder founded in settings. " + folderName);
            }

            if (folderName.Length > 0)
            {
                // проверить/создать директорию, склеить дир и файл
                if (!Directory.Exists(folderName))
                {
                    _log.LogWrite("Create new folder " + folderName);
                    Directory.CreateDirectory(folderName);
                }

                newFile.FileName = Path.Combine(folderName, newFile.FileName);
            }

            // отправляем данные на запись в файл
            _log.LogWrite($"Try to write file '{newFile.FileName}' size {newFile.ScannedDocument.Length}, encoder {newFile.Encoder}");
            
            await WriteAsync(newFile.FileName, newFile.ScannedDocument);
        }

        private async Task WriteAsync(string filePath, byte[] scannedDocument)
        {
            // запись файла
            try
            {
                using var sourceStream =
                    new FileStream(
                        filePath,
                        FileMode.Create, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true);

                await sourceStream.WriteAsync(scannedDocument, 0, scannedDocument.Length);
                _log.LogWrite(filePath + " writed successfully");
            }
            catch (Exception ex)
            {
                _log.LogWrite($"Exception when write {filePath} = {ex.Message}");
            }
        }
    }
}
