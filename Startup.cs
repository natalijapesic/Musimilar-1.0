using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusimilarApi.Interfaces;
using MusimilarApi.MongoDB.Models;
using MusimilarApi.Service;

namespace MusimilarApi
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
            services.Configure<DatabaseSettings>(
            Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);



            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IArtistService, ArtistService>();
            
            services.AddControllers();
                // .AddNewtonsoftJson(options => options.UseMemberCasing());

            AddJwt(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusimilarApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusimilarApi v1"));
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

        private IServiceCollection AddJwt(IServiceCollection services)
		{
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(
            //kaze da i ovako moze da radi ??
			// services.AddAuthentication(options =>
			// {
			// 	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			// 	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			// })
				.AddJwtBearer(options =>
				{
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    //ovo da proverim
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(Configuration.GetSection("JwtKey").ToString())),
                        ValidateIssuer = false,
						ValidateAudience = false
						
                    };
					//options.Validate();
				});

			return services;
		}
    }
}
