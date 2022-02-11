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
using MusimilarApi.Helpers;
using MusimilarApi.Interfaces;
using MusimilarApi.Service;
using Neo4j.Driver;
using StackExchange.Redis;

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
            services.AddCors();
            services.AddControllers();

            services.Configure<DatabaseSettings>(
            Configuration.GetSection(nameof(DatabaseSettings)));


            services.AddSingleton<IDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            var multiplexer = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            

            services.AddSingleton(GraphDatabase.Driver(Configuration.GetSection("Neo4jSettings:Server").Value,
                                      AuthTokens.Basic(Configuration.GetSection("Neo4jSettings:UserName").Value,
                                                       Configuration.GetSection("Neo4jSettings:Password").Value)));


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IPlaylistService, PlaylistService>();

            AddJwt(services);
            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusimilarApi", Version = "v1" });
            });

            services.AddAutoMapper(typeof(OrganizationProfile));
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

            //app.UseHttpsRedirection(); 

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection AddJwt(IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    //ovo da proverim
					options.TokenValidationParameters = new TokenValidationParameters
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
