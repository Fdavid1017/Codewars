using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BEDMASCalculator
{
    // https://www.codewars.com/kata/56a14b6b56e5917073000022

    public class Kata
    {
        public static double calculateOLD(string expression)
        {
            DataTable dt = new DataTable();
            expression = expression.Trim().Replace(" ", "");

            // Fixing POWER issue
            string regexPattern = @"\(([^()]+)\)";
            MatchCollection matches;

            do
            {
                matches = Regex.Matches(expression, regexPattern);

                foreach (Match match in matches)
                {
                    if (match.Value.Contains("^"))
                    {
                        expression = expression.Replace(match.Value, CalculatePow(match.Value).ToString());
                    }
                    else
                    {
                        var result = dt.Compute(match.Value.Replace(",", "."), "");
                        expression = expression.Replace(match.Value, result.ToString());
                    }
                }
            } while (matches.Count > 0);

            if (expression.Contains("^"))
            {
                expression = expression.Replace(expression, CalculatePow(expression).ToString());
            }

            return Convert.ToDouble(dt.Compute(expression.Replace(",", "."), ""));

            // ==========  LOCAL FUNCTIONS  ==========
            double CalculatePow(string powExpression)
            {
                int indexOfPow = powExpression.IndexOf("^");
                string beforePow = powExpression.Substring(0, indexOfPow).Replace("(", "");
                string afterPow = powExpression.Substring(indexOfPow + 1).Replace(")", "");

                if (afterPow.Contains("^"))
                {
                    afterPow = CalculatePow(afterPow).ToString();
                }

                string digitPattern = @"\d";
                Regex rgx = new Regex(digitPattern);


                int powValue = int.Parse(rgx.Match(afterPow).Value);

                double beforePowValue;

                if (!double.TryParse(beforePow, out beforePowValue))
                {
                    beforePowValue = Convert.ToDouble(dt.Compute(beforePow.Replace(",", "."), ""));
                }

                afterPow = rgx.Replace(afterPow, "", 1);
                double pow = Math.Pow(beforePowValue, powValue);
                double result = Convert.ToDouble(dt.Compute($"{pow}{afterPow}".Replace(",", "."), ""));
                Console.WriteLine(result);
                return result;
            }
        }

        public static double calculate(string expression)
        {
            DataTable dt = new DataTable();
            expression = expression.Replace(" ", "");

            // Fixing POWER issue
            string parenthesesPattern = @"\(([^()]+)\)";
            MatchCollection matches;

            Console.WriteLine($"\n\nStarting expression:\n{expression}");
            do
            {
                matches = Regex.Matches(expression, parenthesesPattern);

                foreach (Match match in matches)
                {
                    string equation = match.Value;
                    expression = expression.Replace(equation, CalculateEquation(equation).ToString(CultureInfo.InvariantCulture));
                }

                Console.WriteLine(expression);
            } while (matches.Count > 0);

            double result = CalculateEquation(expression);
            Console.WriteLine($"Result: {result}");
            return result;

            // ==========  LOCAL FUNCTIONS  ==========
            double CalculateEquation(string equation)
            {
                equation = equation.Replace("(", "").Replace(")", "");
                int indexOfPow = equation.IndexOf("^");
                while (indexOfPow != -1)
                {
                    equation = ReplacePow(equation, indexOfPow);
                    indexOfPow = equation.IndexOf("^");
                }

                return Convert.ToDouble(dt.Compute(equation, ""));
            }

            string ReplacePow(string equation, int indexOfPow)
            {
                string beforePow = equation.Substring(0, indexOfPow);
                string afterPow = equation.Substring(indexOfPow + 1);

                string lastNumberPattern = @"((?:\d*\.)?\d+)(?!.*((?:\d*\.)?\d+))";
                string firstNumberPattern = @"^\D*(\d+(?:\.\d+)?)";

                string n1 = Regex.Match(beforePow, lastNumberPattern).Value;
                double number1 = double.Parse(n1, CultureInfo.InvariantCulture);
                string n2 = Regex.Match(afterPow, firstNumberPattern).Value;
                double number2 = double.Parse(n2, CultureInfo.InvariantCulture);

                string powExpressionPart = $"{number1.ToString(CultureInfo.InvariantCulture)}^{number2.ToString(CultureInfo.InvariantCulture)}";
                double powValue = Math.Pow(number1, number2);

                return equation.Replace(powExpressionPart, powValue.ToString(CultureInfo.InvariantCulture));
            }
        }

        [TestFixture]
        public class CalculatorTest
        {
            public bool close(double a, double b)
            {
                if (Math.Abs(a - b) < 0.000000001) return true;
                return false;
            }

            [Test]
            public void PublicTestsDebug()
            {
                /*
                    4 + 2 * ( (226 - (5 * 3) ^ 2) ^ 2 + (10.7 - 7.4) ^ 2 - 6.89)
                    4 + 2 * ( 14 ^ 2 + 3.3 ^ 2 - 6.89)
                    4 + 2 * 200
                    404
                 */


                Assert.AreEqual(true, close(Kata.calculate("4 + 2 * ( (226 - (5 * 3) ^ 2) ^ 2 + (10.7 - 7.4) ^ 2 - 6.89)"), 3));
            }

            [Test]
            public void PublicTests()
            {
                Assert.AreEqual(true, close(Kata.calculate("1 + 2"), 3));
                Assert.AreEqual(true, close(Kata.calculate("2*2"), 4));
                Assert.AreEqual(true, close(Kata.calculate("2 ^ 5"), 32));
                Assert.AreEqual(true, close(Kata.calculate("123      -( 4^ (       3 -   1) * 8 - 8      /(     1 + 1 ) *(3 -1) )"), 3));
            }

            [Test]
            public void HardTests()
            {
                Assert.AreEqual(true, close(Kata.calculate("3 * (4 +       (2 / 3) * 6 - 5)"), 9));
                Assert.AreEqual(true, close(Kata.calculate("123      -( 4^ (       3 -   1) * 8 - 8      /(     1 + 1 ) *(3 -1) )"), 3));
                Assert.AreEqual(true, close(Kata.calculate("4 + 2 * ( (226 - (5 * 3) ^ 2) ^ 2 + (10.7 - 7.4) ^ 2 - 6.89)"), 3));
            }
        }
    }
}