namespace RomanNumeralsHelper
{
    // https://www.codewars.com/kata/51b66044bce5799a7f000003

    public class RomanNumerals
    {
        public static Dictionary<string, int> romanNumbers = new Dictionary<string, int>()
        {
            {"M", 1000},
            {"CM", 900},
            {"D", 500},
            {"CD", 400},
            {"C", 100},
            {"XC", 90},
            {"L", 50},
            {"XL", 40},
            {"X", 10},
            {"IX", 9},
            {"V", 5},
            {"IV", 4},
            {"I", 1},
        };

        public static string ToRoman(int n)
        {
            Dictionary<int, string> romanNumbersText = romanNumbers.ToDictionary(x => x.Value, x => x.Key);
            int[] numberKeys = romanNumbersText.Keys.ToArray();
            string romanNumber = "";

            while (n > 0)
            {
                int checkedNumberIndex = 0;
                int largestNumber = numberKeys[checkedNumberIndex];

                while (n - largestNumber < 0)
                {
                    checkedNumberIndex++;
                    largestNumber = numberKeys[checkedNumberIndex];
                }

                romanNumber += romanNumbersText[largestNumber];
                n -= largestNumber;
            }

            return romanNumber;
        }

        public static int FromRoman(string romanNumeral)
        {
            int number = 0;

            int[] numberKeys = romanNumbers.Values.ToArray();

            while (romanNumeral.Length > 0)
            {
                if (romanNumeral.Length >= 2)
                {
                    int firstCharacterValue = romanNumbers[romanNumeral[0].ToString()];
                    int secondCharacterValue = romanNumbers[romanNumeral[1].ToString()];

                    if (secondCharacterValue > firstCharacterValue)
                    {
                        number += secondCharacterValue - firstCharacterValue;
                        romanNumeral = romanNumeral.Remove(0, 2);
                    }
                    else
                    {
                        number += romanNumbers[romanNumeral[0].ToString()];
                        romanNumeral = romanNumeral.Remove(0, 1);
                    }
                }
                else
                {
                    number += romanNumbers[romanNumeral[0].ToString()];
                    romanNumeral = romanNumeral.Remove(0, 1);
                }
            }

            return number;
        }

        [TestFixture]
        public class SolutionTest
        {
            [Test]
            public void TestToRoman_001()
            {
                int input = 1;
                string expected = "I";

                string actual = RomanNumerals.ToRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestToRoman_002()
            {
                int input = 2;
                string expected = "II";

                string actual = RomanNumerals.ToRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestToRoman_1666()
            {
                int input = 1666;
                string expected = "MDCLXVI";

                string actual = RomanNumerals.ToRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestToRoman_2000()
            {
                int input = 2000;
                string expected = "MM";

                string actual = RomanNumerals.ToRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestToRoman_009()
            {
                int input = 9;
                string expected = "IX";

                string actual = RomanNumerals.ToRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestFromRoman_001()
            {
                string input = "I";
                int expected = 1;

                int actual = RomanNumerals.FromRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestFromRoman_002()
            {
                string input = "II";
                int expected = 2;

                int actual = RomanNumerals.FromRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestFromRoman_2000()
            {
                string input = "MM";
                int expected = 2000;

                int actual = RomanNumerals.FromRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestFromRoman_1666()
            {
                string input = "MDCLXVI";
                int expected = 1666;

                int actual = RomanNumerals.FromRoman(input);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void TestFromRoman_004()
            {
                string input = "IV";
                int expected = 4;

                int actual = RomanNumerals.FromRoman(input);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}