using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebClient.ApiClient;
using WebClient.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAuthClient, AuthClient>();
builder.Services.AddTransient<IHttpClientCRUD, HttpClientCRUDServices>();
builder.Services.AddTransient<ICatalog, CatalogClient>();
builder.Services.AddTransient<IProduct, ProductClient>();
builder.Services.AddTransient<IOrder, OrderClient>();
builder.Services.AddTransient<IOrderItem, OrderItemClient>();
builder.Services.AddTransient<IPayment, PaymentClient>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
builder.Services.AddSession(option =>
{
     // make the session cookie Essential                                                 // Đăng ký dịch vụ Session
    option.Cookie.Name = "ngocsu";                      // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
    option.IdleTimeout = new TimeSpan(365,24, 0, 0);        // Thời gian tồn tại của Session
    
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
