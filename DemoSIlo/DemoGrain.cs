using System.Threading.Tasks;
using DemoContracts;
using Orleans;

namespace DemoSIlo
{
    public class DemoGrain : Grain, IDemoGrain
    {
        public Task<double> Sum(double a, double b)
        {
            return Task.FromResult(a + b + this.GetGrainIdentity().PrimaryKeyLong);
        }

        public Task<double> Mul(double a, double b)
        {
            return Task.FromResult(a * b + this.GetGrainIdentity().PrimaryKeyLong);
        }

        public Task<double> Div(double a, double b)
        {
            return Task.FromResult(a / b + this.GetGrainIdentity().PrimaryKeyLong);
        }
    }
}