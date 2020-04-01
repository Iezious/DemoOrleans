using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime.Development;

namespace DemoSIlo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .ConfigureEndpoints(IPAddress.Loopback, 11111, 30000, true)
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "demoCluster";
                            options.ServiceId = "demoService";
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(DemoGrain).Assembly).WithReferences())
                        .ConfigureApplicationParts(partsOptions =>
                        {
                            partsOptions.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                        })
                        .Configure<ProcessExitHandlingOptions>(options => { options.FastKillOnProcessExit = false; });
                })
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); });
    }
}