using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebAppDia2.Contract;
using WebAppDia2.Data;
using WebAppDia2.Models;
using WebAppDia2.Repositories;
using WebAppDia2.Services;
using WebAppDia3.Data;
using WebAppDia3.Mapping;

namespace WebAppDia2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            // Agrega un registro de prueba
            Log.Information("Aplicación iniciada.");
            try
            {
                // Usa Serilog como el logger
                builder.Host.UseSerilog();

                // 2 Configuración de servicios
                builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
                var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

                // 1 Para creacion del token, previo creacion de la clase
                builder.Services.AddSingleton(jwtSettings);
                builder.Services.AddSingleton<JwtTokenService>();

                // 3 Agregar autenticación JWT
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = TimeSpan.Zero // Opcional: Elimina el margen de 5 minutos en la expiración de tokens
                    };
                });


                // 4 Agregar swagger
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtExample API FE", Version = "v1" });
                });


                // Add services to the container.
                builder.Services.AddAutoMapper(typeof(MappingProfile));


                builder.Services.AddControllers();

                //Registrar DbContext
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(new CustomDbCommandInterceptor())
                .AddInterceptors(new PerformanceInterceptor())
                );

                builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                builder.Services.AddScoped<IProductRepository, ProductRepository>();


                var app = builder.Build();


                //5 Configuración del middleware  para swagger
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JwtExample API v1");
                    });
                }



                //6 Habilitar autenticación y autorización
                app.UseAuthentication();
                app.UseAuthorization();

                // Configure the HTTP request pipeline.

                app.UseHttpsRedirection();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "La aplicación falló al iniciar.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
