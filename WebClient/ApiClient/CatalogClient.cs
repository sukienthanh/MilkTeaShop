using MilkTeaShop.Helper;
using MilkTeaShop.Models;

namespace WebClient.ApiClient
{
    public interface ICatalog
    {
        public  Task<DataResult<IEnumerable<Catalog>>> GetList();
        public  Task<DataResult<Catalog>> GetById(int id);
        public  Task<DataResult<List<Catalog>>> Delete(int[] id);
        public  Task<DataResult<IEnumerable<Catalog>>> Add(IEnumerable<Catalog> data);
        public  Task<DataResult<IEnumerable<Catalog>>> Update(IEnumerable<Catalog> data);
    }
    public class CatalogClient : ICatalog
    {
        private readonly IHttpClientCRUD _apiClient;
        public CatalogClient(IHttpClientCRUD _apiClient)
        {
            this._apiClient = _apiClient;
        }
        public async Task<DataResult<IEnumerable<Catalog>>> GetList()
        {
            return await _apiClient.GetAsync<IEnumerable<Catalog>>("Catalog/GetList");
        }
        public async Task<DataResult<Catalog>> GetById(int id)
        {
            return await _apiClient.GetAsync<Catalog>("Catalog/GetById?id="+id);
        }
        public async Task<DataResult<List<Catalog>>> Delete(int[] id)
        {
            return await _apiClient.DeleteAsync<List<Catalog>>(id,"Catalog/Delete");
        }
        public async Task<DataResult<IEnumerable<Catalog>>> Add(IEnumerable<Catalog> data)
        {
            return await _apiClient.PostAsync<IEnumerable<Catalog>>(data, "Catalog/Add","",false);
        }
        public async Task<DataResult<IEnumerable<Catalog>>> Update(IEnumerable<Catalog> data)
        {
            return await _apiClient.PutAsync<IEnumerable<Catalog>>(data, "Catalog/Update","", false);
        }
    }
}
