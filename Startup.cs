using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movies.Data;
using Movies.Models;
using Movies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configuración para añadir DBContext usando PostgreSQL
            //Trae la conexión desde ApplicationDBContext
            //Pero actualmente esta hecha la conexión en appsettings.json
            /*
             * Instalar Install-Package EFCore.NamingConventions -Version 5.0.2
             * e incluir .UseSnakeCaseNamingConvention()
             * 
             * Borrar tablas de la base de datos y carpeta migrations y volver a ejecutar:
             * dotnet ef migrations add "Initial Migration"
             * dotnet ef database update
             */
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DBContext"))
                .UseSnakeCaseNamingConvention()
                );

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies", Version = "v1" });
            });

            //Clase única mientras se ejecute
            services.AddSingleton<MovieSingleton>(); 

            //crea varias instancias, es necesario porque ApplicationDbContext es de tipo Scoped
            services.AddScoped<IGenericCRUD<Movie>,  GenericCRUD<Movie>>();
            services.AddScoped<IGenericCRUD<Category>, GenericCRUD<Category>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = Configuration["TokenValidationParameters:Audience"];
                    //options.Authority = "https://localhost:5001/";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["TokenValidationParameters:Issuer"],
                        //Verifica que la firma del token sea la misma
                        IssuerSigningKey =new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["TokenValidationParameters:IssuerSingingKey"])),
                        ValidateIssuerSigningKey=true,
                        ValidateAudience=true,
                        ValidateLifetime=true,
                    };
                }
                );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
