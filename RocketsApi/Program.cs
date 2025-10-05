using Data.Contracts;
using Data.DataContext;
using Data.Repositories;
using Logic;
using Logic.Interfaces;
using Mapster;
using MapsterMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<RocketStoreDatabaseSettings>(
    builder.Configuration.GetSection("RocketsStoreDatabase"));

//Dependency injection of the Rocket repository
builder.Services.AddSingleton<IRocketRepository, RocketRepository>();

//Mapper configuration
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

//Dependency injection of the Rocket logic
builder.Services.AddScoped<IRocketsLogic, RocketLogic>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }