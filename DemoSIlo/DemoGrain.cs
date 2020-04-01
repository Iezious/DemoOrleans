using System.Threading.Tasks;
using DemoContracts;
using Orleans;
using Serilog;

namespace DemoSIlo
{
    public class DemoGrain : Grain, IDemoGrain
    {
        private readonly ILogger _logger;

        public DemoGrain(ILogger logger)
        {
            _logger = logger;
        }

        public Task<double> Sum(double a, double b)
        {
            _logger.Information($"Called SUM for {this.GetGrainIdentity().PrimaryKeyLong}");
            return Task.FromResult(a + b + this.GetGrainIdentity().PrimaryKeyLong);
        }

        public Task<double> Mul(double a, double b)
        {
            _logger.Information($"Called MUL for {this.GetGrainIdentity().PrimaryKeyLong}");
            return Task.FromResult(a * b + this.GetGrainIdentity().PrimaryKeyLong);
        }

        public Task<double> Div(double a, double b)
        {
            _logger.Information($"Called DIV for {this.GetGrainIdentity().PrimaryKeyLong}");
            return Task.FromResult(a / b + this.GetGrainIdentity().PrimaryKeyLong);
        }
    }
}