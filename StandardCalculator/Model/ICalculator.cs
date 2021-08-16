using System.Threading.Tasks;

namespace StandardCalculator.Model
{
	public interface ICalculator
	{
		double GetResult(string expression);
		Task<double> GetResultAsync(string expression);
	}
}
