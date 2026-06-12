
using CitasApp.Application.Interfaces;
using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
// Se utiliza WebRootPath. Si es nulo (por ejemplo, si aún no existe la carpeta wwwroot), 
// se usa una ruta alternativa en la raíz del contenido para evitar excepciones.
// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
var webRoot = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
var dataFolder = Path.Combine(webRoot, "data");
Directory.CreateDirectory(dataFolder);

// Rutas para CSV
var csvPacientes = Path.Combine(dataFolder, "pacientes.csv");
var csvMedicos = Path.Combine(dataFolder, "medicos.csv");
var csvCitas = Path.Combine(dataFolder, "citas.csv");

// Rutas para JSON (Aseguramos declarar las rutas físicas para los archivos JSON)
var jsonPacientes = Path.Combine(dataFolder, "pacientes.json");
var jsonMedicos = Path.Combine(dataFolder, "medicos.json");
var jsonCitas = Path.Combine(dataFolder, "citas.json");

// Ruta para SQLite
var sqlitePath = Path.Combine(dataFolder, "citasapp.db");


// ── 2. Elige tus Adapters (DESCOMENTE SOLO EL BLOQUE QUE DESEE USAR) ──────────

// ▶ Bloque A — JSON
// (Para activar JSON: borre el /* de arriba y el */ de abajo. Deje los otros comentados)

builder.Services.AddSingleton<IPacienteRepository>(_ => new JsonPacienteRepository(jsonPacientes));
builder.Services.AddSingleton<IMedicoRepository>(_ => new JsonMedicoRepository(jsonMedicos));
builder.Services.AddSingleton<ICitaRepository>(_ => new JsonCitaRepository(jsonCitas));


// ▶ Bloque B — CSV
// (Para activar CSV: borre el /* de arriba y el */ de abajo. Deje los otros comentados)
/*
builder.Services.AddSingleton<IPacienteRepository>(_ => new CsvPacienteRepository(csvPacientes));
builder.Services.AddSingleton<IMedicoRepository>(_ => new CsvMedicoRepository(csvMedicos));
builder.Services.AddSingleton<ICitaRepository>(_ => new CsvCitaRepository(csvCitas));
*/

// ▶ Bloque C — SQLite
// (Para activar SQLite: borre el /* de arriba y el */ de abajo. Deje los otros comentados)
/*
builder.Services.AddSingleton<IPacienteRepository>(_ => new SqlitePacienteRepository(sqlitePath));
builder.Services.AddSingleton<IMedicoRepository>  (_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddSingleton<ICitaRepository>    (_ => new SqliteCitaRepository(sqlitePath));
*/


// ── 3. Servicios de aplicación ────────────────────────────────────────────────
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<ICitaService, CitaService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

// .NET 9+ optimización de recursos estáticos
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();