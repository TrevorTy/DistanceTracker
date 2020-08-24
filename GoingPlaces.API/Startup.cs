using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoingPlaces.API.Data;
using GoingPlaces.API.Helpers;
using GoingPlaces.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace GoingPlaces.API
{
    public class Startup
    {
        //This is the configuration file appsettings.json
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite
            (Configuration.GetConnectionString("DefaultConnection")));

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            //You can also swap it for SqlServer but change the Connectiong string
            //  since there are minor differences
            // For testing change this to UseMysql
            services.AddDbContext<DataContext>(x => {
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        //It is a depedency injectency container
        //Whenever you wanna add something you want to be consumed by another part of the application
        //Then we add it as a service so we are able to inject it as a service by another part of our application
        public void ConfigureServices(IServiceCollection services)
        {
            //We have to provide the database and connectionstring
            //x => x. is a lambda expression
            //The appsettings.Json is a configuration file
            //The key has to be inside of the GetConnectionString
          
            services.AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<DataContext>();
            
            var applicationSettingsConfiguration = Configuration.GetSection("ApllicationSettings");
            services.Configure<AppSettings>(applicationSettingsConfiguration);


            var appSettings = applicationSettingsConfiguration.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);   

             services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //services.BuildServiceProvider().GetService<DataContext>().Database.Migrate();
            services.AddControllers().AddNewtonsoftJson();
            //Realy important for the SPA to accept this Cors Policy
            services.AddCors();
            //Needs a specific assembly to look in
           // services.AddAutoMapper(typeof(GoingPlacesRepository).Assembly);
            //We need this line in order for it to be available for the injection into the controller
           // services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IGoingPlacesRepository, GoingPlacesRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //everything inside this method is Middleware a sort of journy through the HTTP request pipeline
        //Order matters here
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //If the app is in development mode the app will give  a developerfriendly exceptionpage
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     app.UseExceptionHandler(builder => {
            //         builder.Run(async context => {
            //             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //             var error = context.Features.Get<IExceptionHandlerFeature>();
            //             if(error != null)
            //             {
            //                 //This will add a new header to the response
            //                 // context.Response.AddApplicationError(error.Error.Message);
            //                 //This will write an error message to the htto response
            //                 await context.Response.WriteAsync(error.Error.Message);
            //             }
            //         });
            //     });
            // }
            app.UseHsts(); // maybe remove this later
          
           // app.UseDeveloperExceptionPage(); //  remove this later
            app.UseHttpsRedirection(); // maybe remove this later
            app.UseRouting();
            //Change this later on for safety? //
            //You can use With Origin?
            //Later on the course it will be changed for better security
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //We need these 2 methods to run the Angular files
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            //This does nothing at the moment because it is not configured yet
            app.UseAuthorization();
            //As our application start it it will map our controller endpoints into our app so our api
            //knows how to route the request
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // This is if our app doesn't recognize the mapcontroller
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
