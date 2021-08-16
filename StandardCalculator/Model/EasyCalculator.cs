using System;
using System.Data;
using System.Threading.Tasks;

namespace StandardCalculator.Model
{
	public class EasyCalculator : ICalculator
	{
		public double GetResult(string expression)
		{
			if (expression.Contains(","))
			{
				expression = expression.Replace(',', '.');
			}
			return Convert.ToDouble(new DataTable().Compute(expression, null));
		}

		public async Task<double> GetResultAsync(string expression)
		{
			if (expression.Contains(","))
			{
				expression = expression.Replace(',', '.');
			}
			return await Task<double>.Run(() => Convert.ToDouble(new DataTable().Compute(expression, null)));
		}
	}
}
