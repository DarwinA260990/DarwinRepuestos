using Microsoft.EntityFrameworkCore;
using Repuestos.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar los Controllers.
builder.Services.AddControllers();

// Registrar la documentación OpenAPI.
builder.Services.AddOpenApi();

// Registrar el contexto y la conexión con SQL Server.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();