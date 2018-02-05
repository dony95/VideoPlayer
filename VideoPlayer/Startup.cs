using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoPlayer.DAL;
using Microsoft.EntityFrameworkCore;
using VideoPlayer.DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VideoPlayer.Models;
using Microsoft.AspNetCore.Identity;
using VideoPlayer.Services;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace VideoPlayer
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
            services.AddMvc();
            services.AddScoped<FilmRepository>();
            services.AddScoped<CartoonRepository>();
            services.AddScoped<SeriesRepository>();
            services.AddScoped<SeasonRepository>();
            services.AddScoped<EpisodeRepository>();
            services.AddDbContext<VideoManagerDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:localConn"]));
            //services.AddDbContext<VideoManagerDbContext>(options =>
            //options.UseSqlServer(Configuration["ConnectionStrings:PublishConn"]));
            services.AddMvc().AddJsonOptions(options => 
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = Configuration["Auth:ValidIssuer"],
                        ValidAudience = Configuration["Auth:ValidAudience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(Configuration["Auth:JwtSecurityKey"]))
                    };
                });
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Auth:Google:client_id"];
                googleOptions.ClientSecret = Configuration["Auth:Google:client_secret"];
            });
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<VideoManagerDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddApiVersioning(options =>
               {
                   options.ReportApiVersions = true;
                   options.AssumeDefaultVersionWhenUnspecified = true;
                   options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 1);
                   options.ApiVersionReader = new HeaderApiVersionReader("api-v");
               });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Video API", Version = "v1.0" });
                options.SwaggerDoc("v1.1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Video API", Version = "v1.1" });

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                                       .OfType<ApiVersionAttribute>()
                                       .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VideoManagerDbContext context)
        {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Video API V1.0");
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Video API V1.1");
            });
        }
    }
}
