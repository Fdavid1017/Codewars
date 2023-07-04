using System;

namespace codeWars
{
    public class Kata
    {
        public static string ToCamelCase(string str)
        {
            string[] exploded = str.Split('_', '-');
            string converted = exploded[0];

            for (int i = 1; i < exploded.Length; i++)
            {
                converted += exploded[i][0].ToString().ToUpper() + exploded[i].Substring(1);
            }

            return converted;
        }

        public static int Solution(int value)
        {
            int sum = 0;

            for (int i = 0; i < value; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                {
                    sum += i;
                }
            }

            return sum;
        }

        [Test]
        public void KataTests()
        {
            Assert.AreEqual("theStealthWarrior", Kata.ToCamelCase("the_stealth_warrior"),
                "Kata.ToCamelCase('the_stealth_warrior') did not return correct value");
            Assert.AreEqual("TheStealthWarrior", Kata.ToCamelCase("The-Stealth-Warrior"),
                "Kata.ToCamelCase('The-Stealth-Warrior') did not return correct value");
            Assert.AreEqual("eotgqqdhcmLxidhelscaNgvtipwgna", Kata.ToCamelCase("eotgqqdhcm_lxidhelscaNgvtipwgna"),
                "Kata.ToCamelCase('eotgqqdhcm_lxidhelscaNgvtipwgna') did not return correct value");
        }

        [Test]
        public void MultiplesTest()
        {
            Assert.AreEqual(23, Kata.Solution(10));
        }
    }
}