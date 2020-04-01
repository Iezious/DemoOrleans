using System;
using System.Threading.Tasks;
using DemoContracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace DemoClient
{
    class Program
    {
        static async Task<int>  Main()
        {
            Console.WriteLine("Started");

            using var client = ConnectToOrleans();

            await Execute(client);

            return 0;
        }

        private static async Task Execute(IClusterClient client)
        {
            await client.Connect();
            
            var ra = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < 1000; i++)
            {
                var id = ra.Next(100);
                var grain = client.GetGrain<IDemoGrain>(id);

                var a = ra.Next(10);
                var b = ra.Next(10);

                try
                {

                    switch (ra.Next(3))
                    {
                        case 0:
                        {
                            var res = await grain.Sum(a, b);
                            Console.WriteLine($"{a}+{b}+id = {res}");
                            break;
                        }

                        case 1:
                        {
                            var res = await grain.Mul(a, b);
                            Console.WriteLine($"{a}*{b}+id = {res}");
                            break;
                        }

                        case 2:
                        {
                            var res = await grain.Div(a, b);
                            Console.WriteLine($"{a}/{b}+id = {res}");
                            break;
                        }
                    }

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static IClusterClient ConnectToOrleans()
        {
            var client = new ClientBuilder()
                .UseZooKeeperClustering(config => { config.ConnectionString = "localhost:2181";})
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "demoCluster";
                    options.ServiceId = "demoService";
                })
                .ConfigureLogging(logBuilder => logBuilder.AddConsole())
                .Build();
            
            
            return client;
        }
    }
}