using CitasApp.Application.Interfaces;
using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar Controladores
builder.Services.AddControllers();

// 1. REGISTRAR LA POLÍTICA DE CORS (Permite que el HTML conecte con tu API)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registrar Repositorio Paciente usando la fábrica y el Decorador (Este se queda)
builder.Services.AddScoped<IPacienteRepository>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var repo = RepositoryFactory.CrearPacienteRepository(
    builder.Environment.EnvironmentName, env);
    return new LoggingPacienteRepository(repo);
});

// SE ELIMINÓ LA SEGUNDA DECLARACIÓN DE IPacienteRepository PARA EVITAR QUE SOBRESCRIBA AL DECORADOR

// Registrar Otros Repositorios (Base de datos / JSON)
builder.Services.AddScoped<IMedicoRepository>(provider =>
    new JsonMedicoRepository("App_Data/medicos.json"));

builder.Services.AddScoped<ICitaRepository>(provider =>
    new JsonCitaRepository("App_Data/cita.json"));

// Registrar Servicios de aplicación
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<PacienteService>(); // <-- Ya lo teníamos agregado

builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<MedicoService>(); // <--- AGREGAR ESTA LÍNEA (Soluciona el error actual)

builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<CitaService>();

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();