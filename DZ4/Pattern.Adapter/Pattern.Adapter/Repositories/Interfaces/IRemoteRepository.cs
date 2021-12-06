using Pattern.Adapter.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Adapter.Repositories.Interfaces
{
    interface IRemoteRepository
    {
        Task<ResponceObject> ReadTextRemoteAsync(string filePath);
        Task<ResponceObject> ReadTextXMLRemoteAsync(string filePath);
        Task<ResponceObject> GetHttpRemoteRequest(string url, CancellationTokenSource cts);
    }
}
