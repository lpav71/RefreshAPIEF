using RefreshAPIEF.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Net;
using RefreshAPIEF.Repository;

//Отключаем проверку подлинности сертификата
ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddDbContext<RefreshAPIEFContext>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<DateTimeProcessing>();
builder.Services.AddScoped<MapRepository>();
builder.Services.AddScoped<CommonRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Refresh",
        Description = "API приложения Refresh Software"       
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    );

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/swagger/custom.css");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
