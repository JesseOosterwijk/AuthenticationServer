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
            MySqlConnection keyDB = new MySqlConnection(Configuration["ConnectionStrings:KeyDB"]);
            services.AddSingleton<IKeyRepository, KeyRepository>(s => new KeyRepository(keyDB));
            services.AddSingleton<IKeypairRepository, KeypairRepository>(s => new KeypairRepository(keyDB));
            
            services.AddSingleton<IUserRepository, UserRepository>(s => new UserRepository(
            new MySqlConnection(Configuration["ConnectionStrings:UserDB"])));
            services.AddSingleton<ISaltRepository, SaltMine>(s => new SaltMine(
                new MySqlConnection(Configuration["ConnectionStrings:SaltDB"])));

            services.AddSingleton<ITokenService, JwtTokenService>();
            services.AddSingleton<IEncryptionService, RsaEncryptionService>();
            
            //Deze waardes zouden fout kunnen zijn
            services.AddSingleton<IHashService, Argon2Service>(s => new Argon2Service(100, 4, 8192));
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStringEncryptionService, StringEncryptionService>(s => new StringEncryptionService("HR$2pIjHR$2pIj12"));

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