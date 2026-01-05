using Tracking_data.Hepler;
using Tracking_data.Repositories;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// 🔥 BẮT BUỘC – GỠ LIMIT 50MB
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = long.MaxValue;
});

// Thêm service Repository
builder.Services.AddSingleton<oracle_helper>();
builder.Services.AddSingleton<sql_helper>();
builder.Services.AddSingleton<DataRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cho phép chạy trên tất cả IP

// Hoặc cấu hình trực tiếp certificate

var app = builder.Build();
//app.Urls.Add("http://0.0.0.0:7000"); // hoặc https
//app.Urls.Add("https://0.0.0.0:7001");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


app.Run();
