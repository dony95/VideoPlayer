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
            services.AddDbContext<VideoManagerDbContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:remoteConn"]));
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VideoManagerDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
