namespace CharacterCount
{
    public class Kata
    {
        public static Dictionary<char, int> Count(string str)
        {
            Dictionary<char, int> result = new Dictionary<char, int>();

            foreach (char c in str)
            {
                if (result.ContainsKey(c))
                {
                    result[c]++;
                }
                else
                {
                    result.Add(c, 1);
                }
            }

            return result;
        }

        [Test]
        public static void FixedTest_aaaa()
        {
            Dictionary<char, int> d = new Dictionary<char, int>();
            d.Add('a', 4);
            Assert.AreEqual(d, Kata.Count("aaaa"));
        }

        [Test]
        public static void FixedTest_aabb()
        {
            Dictionary<char, int> d = new Dictionary<char, int>();
            d.Add('a', 2);
            d.Add('b', 2);
            Assert.AreEqual(d, Kata.Count("aabb"));
        }
    }
}