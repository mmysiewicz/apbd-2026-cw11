using apbd_2026_cw11.Data;
using apbd_2026_cw11.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDbService, DbService>(); 
builder.Services.AddDbContext<DatabaseFirstDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();