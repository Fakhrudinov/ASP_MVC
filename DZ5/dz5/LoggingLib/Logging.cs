using LoggingLib.Properties;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LoggingLib
{
    public class Logging : ILogging
    {
        public async void LogWrite(string text)
        {
            text = DateTime.Now.ToString("HH:mm:ss:fffffff: ") + text;

            Console.WriteLine(text);
            await WriteLogToFileAsync(text + "\r\n");
        }

        public async Task WriteLogToFileAsync(string text)
        {
            //имя файла лога
            string fileName = "scanner-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            // папка по умолчанию для записи файлов сканов
            string folderName = "";
            try
            {
                var resourceManager = Resources.ResourceManager;
                folderName = resourceManager.GetString("LogsDefoultFolder");
            }
            catch
            {
                Console.WriteLine("Log Alert: no default folder founded in settings. " + folderName);
            }

            if (folderName.Length > 0)
            {
                // проверить/создать директорию, склеить дир и файл
                if (!Directory.Exists(folderName))
                {
                    Console.WriteLine("Create new folder " + folderName);
                    Directory.CreateDirectory(folderName);
                }

                fileName = Path.Combine(folderName, fileName);
            }


            await WriteAsync(fileName, text);
        }

        private async Task WriteAsync(string fileName, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            // дозапись файла
            try
            {
                using var sourceStream =
                    new FileStream(
                        fileName,
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true);

                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when write {fileName} = {ex.Message}");
            }
        }
    }
}
