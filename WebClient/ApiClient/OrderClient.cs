using MilkTeaShop.Helper;
using MilkTeaShop.Models;
using MimeKit;
using System.Net;
using System.Net.Mail;
using WebClient.Models;

namespace WebClient.ApiClient
{
    public interface IOrder
    {
        public Task<DataResult<IEnumerable<Order>>> GetList();
        public Task<DataResult<Order>> GetById(int id);
        public Task<DataResult<List<Order>>> Delete(int[] id, string token);
        public Task<DataResult<IEnumerable<Order>>> Add(IEnumerable<Order> data, string token);
        public Task<DataResult<IEnumerable<Order>>> Update(IEnumerable<Order> data, string token);
        public Task<bool> SendMailAsync();
    }
    public class OrderClient : IOrder
    {
        private readonly IHttpClientCRUD _apiClient;
        private readonly EmailConfiguration _emailConfiguration;
        public OrderClient(IHttpClientCRUD _apiClient, EmailConfiguration _emailConfig)
        {
            this._apiClient = _apiClient;
            this._emailConfiguration = _emailConfig;
        }
        public async Task<DataResult<IEnumerable<Order>>> GetList()
        {
            return await _apiClient.GetAsync<IEnumerable<Order>>("Order/GetList", "", false);
        }
        public async Task<DataResult<Order>> GetById(int id)
        {
            return await _apiClient.GetAsync<Order>("Order/GetById?id=" + id, "", false);
        }
        public async Task<DataResult<List<Order>>> Delete(int[] id, string token)
        {
            return await _apiClient.DeleteAsync<List<Order>>(id, "Order/Delete", token);
        }
        public async Task<DataResult<IEnumerable<Order>>> Add(IEnumerable<Order> data, string token)
        {
            return await _apiClient.PostAsync<IEnumerable<Order>>(data, "Order/Add", token);
        }
        public async Task<DataResult<IEnumerable<Order>>> Update(IEnumerable<Order> data, string token)
        {
            return await _apiClient.PutAsync<IEnumerable<Order>>(data, "Order/Update", token);
        }
        public async Task<bool> SendMailAsync()
        {
            MailMessage message = new MailMessage(
                from: _emailConfiguration.From,
                to: _emailConfiguration.To,
                subject: _emailConfiguration.Subject,
                body: _emailConfiguration.Body
            );
                TextPart html = new TextPart(MimeKit.Text.TextFormat.Html);
            html.Text = "<a href ='https://webclient-kw5.conveyor.cloud/' style='color:red;'>click here</a>";
            message.Body = html.Text;            
            message.Subject = "Bạn Có đơn hàng mới";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                try
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(_emailConfiguration.From, _emailConfiguration.Password);
                    client.EnableSsl = true;
                    await client.SendMailAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }            
        }
    }
}

