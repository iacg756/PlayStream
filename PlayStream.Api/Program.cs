using Microsoft.EntityFrameworkCore;
using PlayStream.Api.Filters;
using PlayStream.Core.Interfaces;
using PlayStream.Infrastructure.Data;
using PlayStream.Infrastructure.Mappings;
using PlayStream.Infrastructure.Repositories;
using PlayStream.Services.Interfaces;
using PlayStream.Services.Services;
using PlayStream.Services.Validators;
using PlayStream.Core.DTOs;
using FluentValidation;

namespace PlayStream.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD MySql 
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<PlayStreamContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            #region Registro de Dependencias (Capa Infrastructure & Core)
           
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            #endregion
            #region Registro de Servicios (Capa Services)
            builder.Services.AddTransient<IUsuarioService, UsuarioService>();
            builder.Services.AddTransient<IContenidoService, ContenidoService>();
            builder.Services.AddTransient<IPerfilService, PerfilService>();
            builder.Services.AddTransient<ICalificacionService, CalificacionService>();
            builder.Services.AddTransient<IFavoritoService, FavoritoService>();
            #endregion
            #region Configuración de Controladores y NewtonsoftJson
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            #endregion

            #region AutoMapper y Validadores
          
            builder.Services.AddAutoMapper(typeof(UsuarioProfile).Assembly);

            builder.Services.AddScoped<IValidator<UsuarioDto>, UsuarioDtoValidator>();
            builder.Services.AddScoped<IValidator<PerfilDto>, PerfilDtoValidator>();
            builder.Services.AddScoped<IValidator<CalificacionDto>, CalificacionDtoValidator>();
            builder.Services.AddScoped<IValidator<FavoritoDto>, FavoritoDtoValidator>();

            #endregion

            builder.Services.AddOpenApi();
 
            var app = builder.Build();

            #region Middleware de Excepciones (Global)
        
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            #endregion

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