namespace findTheOdd
{
    // https://www.codewars.com/kata/54da5a58ea159efa38000836/train/csharp
    public class FindTheOddInt
    {
        public static int find_it(int[] seq)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();

            foreach (int i in seq)
            {
                if (occurrences.ContainsKey(i))
                {
                    occurrences[i]++;
                }
                else
                {
                    occurrences.Add(i, 1);
                }
            }

            foreach (var occurrence in occurrences)
            {
                if (occurrence.Value % 2 == 1)
                {
                    return occurrence.Key;
                }
            }

            return -1;
        }

        [Test]
        public void Tests()
        {
            Assert.AreEqual(5, FindTheOddInt.find_it(new[] { 20, 1, -1, 2, -2, 3, 3, 5, 5, 1, 2, 4, 20, 4, -1, -2, 5 }));
        }
    }
}