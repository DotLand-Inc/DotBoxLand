using DotBoxLand.Storage.Api.Models;
using DotBoxLand.Storage.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Configure AWS S3
builder.Services.Configure<AwsS3Settings>(
    builder.Configuration.GetSection("AwsS3Settings"));

// Add services
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<S3Service>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
