using Microsoft.EntityFrameworkCore;
using WA_CERT.Data;

var builder = WebApplication.CreateBuilder(args);

// --- INICIO DE CONFIGURACI�N DE CORS ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Aqu� le dices a tu backend que conf�e en las peticiones
                          // que vienen desde tu frontend de Angular.
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// --- FIN DE CONFIGURACI�N DE CORS ---

// Add services to the container.
// 1. Lee la cadena de conexi�n desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. A�ade el DbContext al contenedor de servicios
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- PASO 2: Habilitar la pol�tica de CORS ---
// El orden es MUY importante. Debe ir antes de UseAuthorization.
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();