using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Serilog;

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
                    int portShift = (args.Length > 0) ? int.Parse(args[0]) : 0;
                    
                    siloBuilder
                        .UseZooKeeperClustering(config => { config.ConnectionString = "localhost:2181";})
                        .ConfigureEndpoints(IPAddress.Loopback, 11111 + portShift, 30000 + portShift, true)
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "demoCluster";
                            options.ServiceId = "demoService";
                        })
                        .Configure<ProcessExitHandlingOptions>(options => { options.FastKillOnProcessExit = false; });
                })
                .UseSerilog((opt, ctx) => { ctx.WriteTo.Console(); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}