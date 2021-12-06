using Pattern.Adapter.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Adapter.Repositories.Interfaces
{
    interface IDirectRepository
    {
        Task<ResponceObject> ReadTextDirectlyAsync(string filePath);

        Task<ResponceObject> GetHttpDirectRequest(string url, CancellationTokenSource cts);
    }
}
