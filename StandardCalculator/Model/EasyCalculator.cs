using System;
using System.Data;

namespace StandardCalculator.Model
{
	class EasyCalculator : ICalculator
	{
		public double GetResult(string expression)
		{
			return Convert.ToDouble(new DataTable().Compute(expression, null));
		}
	}
}
