using System.Text.RegularExpressions;

namespace YourOrderPlease
{
    public class Kata
    {
        public static string Order(string words)
        {
            if (words.Length == 0) return "";

            string[] wordsArray = words.Trim().Split(' ');
            string[] wordsOrdered = new string[wordsArray.Length];

            foreach (string word in wordsArray)
            {
                int position = int.Parse(Regex.Match(word, @"\d+").Value);
                wordsOrdered[position - 1] = word;
            }

            return string.Join(" ", wordsOrdered);
        }

        [Test, Description("Sample Tests")]
        public void SampleTest()
        {
            Assert.AreEqual("Thi1s is2 3a T4est", Kata.Order("is2 Thi1s T4est 3a"));
            Assert.AreEqual("Fo1r the2 g3ood 4of th5e pe6ople", Kata.Order("4of Fo1r pe6ople g3ood th5e the2"));
            Assert.AreEqual("", Kata.Order(""));
        }
    }
}