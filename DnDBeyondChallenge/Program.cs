using DnDBeyondChallenge.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// Load singletons
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost:6379")
);

builder.Services.AddSingleton<RedisService>();
builder.Services.AddSingleton<CharacterService>();

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Initialize
var redisInitializer = app.Services.GetRequiredService<RedisService>();
await redisInitializer.InitializeCacheFromFiles("Data");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
