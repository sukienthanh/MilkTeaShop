using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MilkTeaShop.Models;
using MilkTeaShop.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSwaggerGen(option =>
     {
         //adding the security definition in our swagger configuration
         option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
         {
             Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
             Name = "Authorization",
             In = Microsoft.OpenApi.Models.ParameterLocation.Header,
             Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
         });
         //Security Requirement
         option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
     });

builder.Services.AddDbContext<MilkTeaShopContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("CN"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        //setup token' parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddTransient<IUser,UserServices>();
builder.Services.AddTransient<IAuth, AuthServices>();
builder.Services.AddTransient<IProduct, ProductServices>();
builder.Services.AddTransient<IRole, RoleServices>();
builder.Services.AddTransient<IAbout, AboutServices>();
builder.Services.AddTransient<IOrder, OrderServices>();
builder.Services.AddTransient<IOrderItem, OrderItemServices>();
builder.Services.AddTransient<IPayment, PaymentServices>();
builder.Services.AddTransient<ICatalog, CatalogServices>();
builder.Services.AddTransient<IPrinter, PrinterServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.UseSession();

app.MapControllers();

app.Run();
