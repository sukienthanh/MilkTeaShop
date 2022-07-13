using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace WebClient.ApiClient
{
    public interface IPayment
    {
        public Task<DataResult<IEnumerable<Payment>>> GetList();
        public Task<DataResult<Payment>> GetById(int id);
        public Task<DataResult<List<Payment>>> Delete(int[] id, string token);
        public Task<DataResult<IEnumerable<Payment>>> Add(IEnumerable<Payment> data, string token);
        public Task<DataResult<IEnumerable<Payment>>> Update(IEnumerable<Payment> data, string token);
    }
    public class PaymentClient : IPayment
    {
        private readonly IHttpClientCRUD _apiClient;
        public PaymentClient(IHttpClientCRUD _apiClient)
        {
            this._apiClient = _apiClient;
        }
        public async Task<DataResult<IEnumerable<Payment>>> GetList()
        {
            return await _apiClient.GetAsync<IEnumerable<Payment>>("Payment/GetList", "", false);
        }
        public async Task<DataResult<Payment>> GetById(int id)
        {
            return await _apiClient.GetAsync<Payment>("Payment/GetById?id=" + id, "", false);
        }
        public async Task<DataResult<List<Payment>>> Delete(int[] id, string token)
        {
            return await _apiClient.DeleteAsync<List<Payment>>(id, "Payment/Delete", token);
        }
        public async Task<DataResult<IEnumerable<Payment>>> Add(IEnumerable<Payment> data, string token)
        {
            return await _apiClient.PostAsync<IEnumerable<Payment>>(data, "Payment/Add", token);
        }
        public async Task<DataResult<IEnumerable<Payment>>> Update(IEnumerable<Payment> data, string token)
        {
            return await _apiClient.PutAsync<IEnumerable<Payment>>(data, "Payment/Update", token);
        }
    }
}
