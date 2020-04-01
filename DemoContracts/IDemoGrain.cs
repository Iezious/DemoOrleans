using System.Threading.Tasks;
using Orleans;

namespace DemoContracts
{
    public interface IDemoGrain : IGrainWithIntegerKey
    {
        Task<double> Sum(double a, double b);
        Task<double> Mul(double a, double b);
        Task<double> Div(double a, double b);
    }
}