using System.Text;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
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
            EncodingProvider ppp = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            
            services.AddSingleton<IUserRepository, UserRepository>(s => new UserRepository(
            new MySqlConnection("Server=116.203.194.69; Port=3306; Database=UserDB;Uid=userdb;Pwd=KNnJsvRAUSyWyfAAbuLsibkM5gkzZ736;")));
            services.AddSingleton<IKeyRepository, KeyRepository>(s => new KeyRepository(
                new MySqlConnection("Server=116.203.194.69; Port=3306; Database=KeyDB;Uid=keydb;Pwd=A64jDnpjJzdzV3WgFDbzt4jDXHDtoYwD;")));
            services.AddSingleton<ISaltRepository, SaltMine>(s => new SaltMine(
                new MySqlConnection("Server=116.203.194.69; Port=3306; Database=SaltDB;Uid=saltdb;Pwd=7mRB5p7WTkpcDnirFDQ9RWrZseH4C74M;")));

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<IEncryptionService, RsaEncryptionService>();
            //Deze waardes zouden fout kunnen zijn
            services.AddSingleton<IHashService, Argon2Service>(s => new Argon2Service(100, 4, 8192));
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