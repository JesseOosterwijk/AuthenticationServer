using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Service;
using Service.Implementations;
using Service.Interfaces;

namespace CyberAuthenticationAPI
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
            services.AddSingleton<IUserRepository, UserRepository>(s => new UserRepository(new MySqlConnection("Wordt later ingevuld")));
            services.AddSingleton<IKeyRepository, KeyRepository>(s => new KeyRepository(new MySqlConnection("Wordt later ingevuld")));
            services.AddSingleton<ISaltRepository, SaltMine>(s => new SaltMine(new MySqlConnection("Wordt later ingevuld")));

            services.AddSingleton<ITokenService, JwtTokenService>(sc => new JwtTokenService("Super veilig geheimpje"));
            //Deze waardes zouden fout kunnen zijn
            services.AddSingleton<IEncryptionService, Argon2Service>(s => new Argon2Service(100, 4, 8192));
            services.AddSingleton<IUserService, UserService>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}