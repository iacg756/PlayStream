using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using PlayStream.Core.Interfaces;
using PlayStream.Infrastructure.Data;
using PlayStream.Infrastructure.Repositories;
using PlayStream.Infrastructure.Mappings;
using PlayStream.Services.Interfaces;
using PlayStream.Services.Services;
using PlayStream.Services.Validators;

namespace PlayStream.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PlayStreamContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            // 1. Registrar los servicios de negocio
            builder.Services.AddTransient<IUsuarioService, UsuarioService>();
            builder.Services.AddTransient<IContenidoService, ContenidoService>();
            builder.Services.AddTransient<IPerfilService, PerfilService>(); // NUEVO

            // 2. Registrar el repositorio genérico
            builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            // 3. Configurar Controladores y NewtonsoftJson
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // 4. Registra AutoMapper escaneando todos los profiles
            builder.Services.AddAutoMapper(typeof(UsuarioProfile).Assembly);

            // 5. Configurar FluentValidation Automático (NUEVO MÉTODO)
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UsuarioDtoValidator>();

            // 6. Configurar OpenAPI
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}