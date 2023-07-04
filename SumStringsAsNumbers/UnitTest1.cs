using System.Numerics;

namespace SumStringsAsNumbers
{
    public class Kata
    {
        public static string sumStrings(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
            {
                return BigInteger.Parse(b).ToString();
            }
            if (string.IsNullOrEmpty(b))
            {
                return BigInteger.Parse(a).ToString();
            }

            return (BigInteger.Parse(a) + BigInteger.Parse(b)).ToString();
        }

        [TestFixture]
        public class CodeWarsTest
        {
            [Test]
            public void Given123And456Returns579()
            {
                Assert.AreEqual("579", Kata.sumStrings("123", "456"));
            }
        }
    }
}