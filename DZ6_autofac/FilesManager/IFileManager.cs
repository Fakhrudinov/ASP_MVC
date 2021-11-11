using System.Threading.Tasks;

namespace FilesManager
{
    public interface IFileManager
    {
        Task WriteScanToFileAsync(FileModel newFile);
    }
}
