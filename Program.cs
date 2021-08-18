using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClarityEmailerLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClarityConsole
{
    class Program
    {
        private static List<string> _recipients = new List<string>();
        private static string _sender = "clarity@clarityventures.com";
        private static string _body = "Test mail";
        private static string _subject = "This is just a test.";
        private const int Retries = 3;

        static async Task Main(string[] args)
        {

            _recipients.Add("Jeremy@jeremylarose.com");
            _recipients.Add("toni@tonilarose.com");
            _recipients.Add("macey@maceyblouw.com");

            ClarityMail mail = new ClarityMail();
            await mail.SendMessages(_recipients, _sender, _body, _subject, Retries);

            Console.WriteLine("Application Complete.");
        }

        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddDbContext<ConsoleDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            }

            public void Configure(IApplicationBuilder app)
            {

            }
        }
    }
}
