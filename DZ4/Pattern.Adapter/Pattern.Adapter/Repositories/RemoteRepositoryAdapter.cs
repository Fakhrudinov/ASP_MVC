using Pattern.Adapter.Model;
using Pattern.Adapter.Repositories.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Adapter.Repositories
{
    class RemoteRepositoryAdapter : IRemoteRepository
    {
        private readonly RemoteRepositoryAdaptee _adaptee;

        public RemoteRepositoryAdapter(RemoteRepositoryAdaptee adaptee)
        {
            _adaptee = adaptee;
        }

        public async Task<ResponceObject> GetHttpRemoteRequest(string url, CancellationTokenSource cts)
        {
            var remoteResult = await _adaptee.GetHttpRemoteRequest(url, cts);

            ResponceObject result = AdaptModel(remoteResult, "Http adapted model");
            return result;
        }

        public async Task<ResponceObject> ReadTextRemoteAsync(string filePath)
        {
            var remoteResult = await _adaptee.ReadTextRemoteAsync(filePath);

            ResponceObject result = AdaptModel(remoteResult, "File adapted model");
            return result;
        }

        public async Task<ResponceObject> ReadTextXMLRemoteAsync(string filePath)
        {
            var remoteResult = await _adaptee.ReadTextXMLRemoteAsync(filePath);

            ResponceObject result = AdaptModel(remoteResult, "XML File adapted model");
            return result;
        }


        private ResponceObject AdaptModel(RemoteResponceObject remoteResult, string source)
        {
            if (remoteResult == null)
            {
                return null;
            }
            else
            {
                ResponceObject result = new ResponceObject();
                result.source = source;
                result.id = remoteResult.productId;
                result.name = remoteResult.productName;
                result.description = $"{remoteResult.info}, color {remoteResult.color}, weight {remoteResult.weight}";
                result.price = remoteResult.price;
                result.categoryId = 7;

                return result;
            }
        }
    }
}
