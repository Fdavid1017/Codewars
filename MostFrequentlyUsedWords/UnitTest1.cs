using System.Text.RegularExpressions;

namespace MostFrequentlyUsedWords
{
    // https://www.codewars.com/kata/51e056fe544cf36c410000fb

    public class TopWords
    {
        public static List<string> Top3(string s)
        {
            s = Regex.Replace(s, @"[^a-zA-Z0-9\'\ ]", " ");
            s = Regex.Replace(s, @"\s\s+", " ");

            Regex rgx = new Regex(@"(?=.*\w)^(\w|')+$");
            string[] words = s.Trim()
                .Split(' ')
                .Where(x => rgx.IsMatch(x))
                .Select(x => x.ToLower())
                .ToArray();

            var group = words.GroupBy(x => x).OrderByDescending(x => x.Count()).Take(3).ToList();

            return group.Select(x => x.Key).ToList();
        }

        [Test]
        public void SampleTests()
        {
            Assert.AreEqual(new List<string> { "e", "d", "a" }, TopWords.Top3("a a a  b  c c  d d d d  e e e e e"));
            Assert.AreEqual(new List<string> { "e", "ddd", "aa" }, TopWords.Top3("e e e e DDD ddd DdD: ddd ddd aa aA Aa, bb cc cC e e e"));
            Assert.AreEqual(new List<string> { "won't", "wont" }, TopWords.Top3("  //wont won't won't "));
            Assert.AreEqual(new List<string> { "e" }, TopWords.Top3("  , e   .. "));
            Assert.AreEqual(new List<string> { }, TopWords.Top3("  ...  "));
            Assert.AreEqual(new List<string> { }, TopWords.Top3("  '  "));
            Assert.AreEqual(new List<string> { }, TopWords.Top3("  '''  "));
            Assert.AreEqual(new List<string> { "a", "of", "on" }, TopWords.Top3(
                string.Join("\n", new string[]{"In a village of La Mancha, the name of which I have no desire to call to",
                    "mind, there lived not long since one of those gentlemen that keep a lance",
                    "in the lance-rack, an old buckler, a lean hack, and a greyhound for",
                    "coursing. An olla of rather more beef than mutton, a salad on most",
                    "nights, scraps on Saturdays, lentils on Fridays, and a pigeon or so extra",
                    "on Sundays, made away with three-quarters of his income." })));
        }
    }
}