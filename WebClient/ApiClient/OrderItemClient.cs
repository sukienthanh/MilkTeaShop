using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace WebClient.ApiClient
{
    public interface IOrderItem
    {
        public Task<DataResult<IEnumerable<OrderItem>>> GetList();
        public Task<DataResult<OrderItem>> GetById(int id);
        public Task<DataResult<List<OrderItem>>> Delete(int[] id, string token);
        public Task<DataResult<IEnumerable<OrderItem>>> Add(IEnumerable<OrderItem> data, string token);
        public Task<DataResult<IEnumerable<OrderItem>>> Update(IEnumerable<OrderItem> data, string token);
    }
    public class OrderItemClient : IOrderItem
    {
        private readonly IHttpClientCRUD _apiClient;
        public OrderItemClient(IHttpClientCRUD _apiClient)
        {
            this._apiClient = _apiClient;
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> GetList()
        {
            return await _apiClient.GetAsync<IEnumerable<OrderItem>>("OrderItem/GetList", "", false);
        }
        public async Task<DataResult<OrderItem>> GetById(int id)
        {
            return await _apiClient.GetAsync<OrderItem>("OrderItem/GetById?id=" + id, "", false);
        }
        public async Task<DataResult<List<OrderItem>>> Delete(int[] id, string token)
        {
            return await _apiClient.DeleteAsync<List<OrderItem>>(id, "OrderItem/Delete", token);
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> Add(IEnumerable<OrderItem> data, string token)
        {
            return await _apiClient.PostAsync<IEnumerable<OrderItem>>(data, "OrderItem/Add", token);
        }
        public async Task<DataResult<IEnumerable<OrderItem>>> Update(IEnumerable<OrderItem> data, string token)
        {
            return await _apiClient.PutAsync<IEnumerable<OrderItem>>(data, "OrderItem/Update", token);
        }
    }
}
