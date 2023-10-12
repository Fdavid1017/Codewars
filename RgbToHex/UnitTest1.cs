using System.Drawing;

namespace RgbToHex
{
    // https://www.codewars.com/kata/513e08acc600c94f01000001

    public class Kata
    {
        public static string Rgb(int r, int g, int b)
        {
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            Color myColor = Color.FromArgb(r, g, b);
            return myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
        }

        [Test]
        public void FixedTests()
        {
            Assert.AreEqual("FFFFFF", Kata.Rgb(255, 255, 255));
            Assert.AreEqual("FFFFFF", Kata.Rgb(255, 255, 300));
            Assert.AreEqual("000000", Kata.Rgb(0, 0, 0));
            Assert.AreEqual("9400D3", Kata.Rgb(148, 0, 211));
            Assert.AreEqual("9400D3", Kata.Rgb(148, -20, 211), "Handle negative numbers.");
            Assert.AreEqual("90C3D4", Kata.Rgb(144, 195, 212));
            Assert.AreEqual("D4350C", Kata.Rgb(212, 53, 12), "Consider single hex digit numbers.");
        }
    }
}