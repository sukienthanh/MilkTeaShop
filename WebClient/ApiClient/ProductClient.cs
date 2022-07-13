using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace WebClient.ApiClient
{
    public interface IProduct
    {
        public Task<DataResult<IEnumerable<Product>>> GetList();
        public Task<DataResult<Product>> GetById(int id);
        public Task<DataResult<List<Product>>> Delete(int[] id,string token);
        public Task<DataResult<IEnumerable<Product>>> Add(IEnumerable<Product> data,string token);
        public Task<DataResult<IEnumerable<Product>>> Update(IEnumerable<Product> data,string token);
    }
    public class ProductClient : IProduct
    {
        private readonly IHttpClientCRUD _apiClient;
        public ProductClient(IHttpClientCRUD _apiClient)
        {
            this._apiClient = _apiClient;
        }
        public async Task<DataResult<IEnumerable<Product>>> GetList()
        {
            return await _apiClient.GetAsync<IEnumerable<Product>>("Product/GetList", "", false);
        }
        public async Task<DataResult<Product>> GetById(int id)
        {
            return await _apiClient.GetAsync<Product>("Product/GetById?id=" + id, "", false);
        }
        public async Task<DataResult<List<Product>>> Delete(int[] id, string token)
        {
            return await _apiClient.DeleteAsync<List<Product>>(id, "Product/Delete",token);
        }
        public async Task<DataResult<IEnumerable<Product>>> Add(IEnumerable<Product> data, string token)
        {
            return await _apiClient.PostAsync<IEnumerable<Product>>(data, "Product/Add",token);
        }
        public async Task<DataResult<IEnumerable<Product>>> Update(IEnumerable<Product> data, string token)
        {
            return await _apiClient.PutAsync<IEnumerable<Product>>(data, "Product/Update",token);
        }
    }
}
