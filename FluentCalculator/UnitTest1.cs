using System.Data;

namespace FluentCalculator
{
    // https://www.codewars.com/kata/5578a806350dae5b05000021/train/csharp
    public class Kata
    {
        public class FluentCalculator
        {
            string expression = "";
            DataTable dt = new DataTable();

            #region NUMBERS

            public FluentCalculator Zero
            {
                get
                {
                    expression += "0";
                    return this;
                }
            }
            public FluentCalculator One
            {
                get
                {
                    expression += "1";
                    return this;
                }
            }

            public FluentCalculator Two
            {
                get
                {
                    expression += "2";
                    return this;
                }
            }

            public FluentCalculator Three
            {
                get
                {
                    expression += "3";
                    return this;
                }
            }

            public FluentCalculator Four
            {
                get
                {
                    expression += "4";
                    return this;
                }
            }

            public FluentCalculator Five
            {
                get
                {
                    expression += "5";
                    return this;
                }
            }

            public FluentCalculator Six
            {
                get
                {
                    expression += "6";
                    return this;
                }
            }

            public FluentCalculator Seven
            {
                get
                {
                    expression += "7";
                    return this;
                }
            }

            public FluentCalculator Eight
            {
                get
                {
                    expression += "8";
                    return this;
                }
            }

            public FluentCalculator Nine
            {
                get
                {
                    expression += "9";
                    return this;
                }
            }

            public FluentCalculator Ten
            {
                get
                {
                    expression += "10";
                    return this;
                }
            }

            #endregion

            #region OPERATORS

            public FluentCalculator Plus
            {
                get
                {
                    expression += "+";
                    return this;
                }
            }

            public FluentCalculator Minus
            {
                get
                {
                    expression += "-";
                    return this;
                }
            }

            public FluentCalculator Times
            {
                get
                {
                    expression += "*";
                    return this;
                }
            }

            public FluentCalculator DividedBy
            {
                get
                {
                    expression += "/";
                    return this;
                }
            }

            #endregion

            public double Result()
            {
                Console.WriteLine(expression);
                var result = dt.Compute(this.expression, "");
                this.expression = "";
                return Convert.ToDouble(result);
            }

            public static implicit operator double(FluentCalculator value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                return value.Result();
            }
        }


        [TestFixture]
        public class Tests
        {
            [Test]
            public static void BasicAddition()
            {
                var calculator = new FluentCalculator();

                //Test Result Call
                Assert.AreEqual(3, calculator.One.Plus.Two.Result());
            }

            [Test]
            public static void MultipleInstances()
            {
                var calculatorOne = new FluentCalculator();
                var calculatorTwo = new FluentCalculator();

                Assert.AreNotEqual((double)calculatorOne.Five.Plus.Five, (double)calculatorTwo.Seven.Times.Three);
            }

            [Test]
            public static void MultipleCalls()
            {
                //Testing that the expression or reference clears between calls
                var calculator = new FluentCalculator();
                Assert.AreEqual(4, calculator.One.Plus.One.Result() + calculator.One.Plus.One.Result());
            }

            [Test]
            public static void Bedmas()
            {
                //Testing Order of Operations
                var calculator = new FluentCalculator();
                Assert.AreEqual(58, (double)calculator.Six.Times.Six.Plus.Eight.DividedBy.Two.Times.Two.Plus.Ten.Times.Four.DividedBy.Two.Minus.Six);
                Assert.AreEqual(-11.972, calculator.Zero.Minus.Four.Times.Three.Plus.Two.DividedBy.Eight.Times.One.DividedBy.Nine, 0.01);
            }

            [Test]
            public static void StaticCombinationCalls()
            {
                //Testing Implicit Conversions
                var calculator = new FluentCalculator();
                Assert.AreEqual(177.5, 10 * calculator.Six.Plus.Four.Times.Three.Minus.Two.DividedBy.Eight.Times.One.Minus.Five.Times.Zero);
            }
        }
    }
}