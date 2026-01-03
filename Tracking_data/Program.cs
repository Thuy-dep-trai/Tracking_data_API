using Tracking_data.Hepler;
using Tracking_data.Repositories;

var builder = WebApplication.CreateBuilder(args);

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
