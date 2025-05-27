using backend.src.Config;
using backend.src.Middlewares;
using backend.src.Repositories;
using backend.src.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework e SQL Server
builder.Services.AddDbContext<InternacaoContext>(options =>
{
    // A string de conexão deve ser configurada conforme o ambiente
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configuração de serviços e repositórios para injeção de dependências
builder.Services.AddScoped<IPacienteInternacaoService, PacienteInternacaoService>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

builder.Services.AddControllers();
// Configuração do Swagger para testes de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Uso do middleware de tratamento global de erros
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();