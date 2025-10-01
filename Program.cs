using Microsoft.EntityFrameworkCore;
using MotoMonitoramento.Data;

var builder = WebApplication.CreateBuilder(args);

// Lê variáveis de ambiente
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "qmove";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
var dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "root123";

// Monta a connection string MySQL
var connectionString =
    $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPass}";

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options
        .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)))
        .EnableSensitiveDataLogging()
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

// ⚡ Rodar migrations automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Cria banco/tabelas automaticamente
}

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QMove API v1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", () => "API QMove funcionando");

app.UseAuthorization();
app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

#if DEBUG
app.UseDeveloperExceptionPage();
#endif

app.Run();
