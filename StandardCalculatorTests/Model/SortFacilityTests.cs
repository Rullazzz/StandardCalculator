using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardCalculator.Model.Tests
{
	[TestClass()]
	public class SortFacilityTests
	{
		[TestMethod()]
		public void GetPolishNotationTest()
		{
			// Arrange
			var expressions = new List<string>()
			{
				"1 + 2",
				"-1 + 2",
				"5*(2+2)",
				"1-(-2)",
				"1-(-(-3))",
				"1+(-2)",
				"5-(-4*3)" // Из-за этого примера сдела приоритет у скобки ноль.
			};
			var answers = new List<string>()
			{
				"1 2 + ",
				"1 ~ 2 + ",
				"5 2 2 + * ",
				"1 2 ~ - ",
				"1 3 ~ ~ - ",
				"1 2 ~ + ",
				"5 4 ~ 3 * - ",
			};
			var sortFacility = new SortFacility();
			var answersFunc = new List<string>();


			// Act
			foreach (var exp in expressions)
			{
				answersFunc.Add(sortFacility.GetPolishNotation(exp));
			}


			// Assert
			for (var i = 0; i < answers.Count; i++)
			{
				Assert.AreEqual(answers[i], answersFunc[i]);
			}		
		}
	}
}