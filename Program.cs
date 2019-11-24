using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreBustronic.Models;
using AspNetCoreBustronic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBustronic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    var cs = hostBuilderContext.Configuration.GetConnectionString("Postgres");
                    services.AddHostedService<Worker>();
                    services.AddDbContext<BustronicContext>(options =>
                        options.UseNpgsql(cs, o => o.UseNetTopologySuite()));
                });
    }
}
