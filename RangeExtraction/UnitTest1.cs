namespace RangeExtraction
{
    // https://www.codewars.com/kata/51ba717bb08c1cd60f00002f

    public class RangeExtraction
    {
        public static string Extract(int[] args)
        {
            List<string> ranges = new List<string>();
            int? start = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 < args.Length && args[i] - args[i + 1] == -1)
                {
                    // Next is in interval
                    if (start == null)
                        start = args[i];
                }
                else
                {
                    // Next isnt in interval
                    ranges.Add(FormatIntervalText(start, args[i]));
                    start = null;
                }
            }
            
            return string.Join(",", ranges);
        }

        private static string FormatIntervalText(int? start, int current)
        {
            if (start == null)
            {
                return current.ToString();
            }
            else if (start - current != -1)
            {
                return $"{start}-{current}";
            }
            else
            {
                return $"{start},{current}";
            }
        }

        [TestFixture]
        public class RangeExtractorTest
        {
            [Test(Description = "Simple tests")]
            public void SimpleTests()
            {
                Assert.AreEqual("1,2", RangeExtraction.Extract(new[] { 1, 2 }));
                Assert.AreEqual("1-3", RangeExtraction.Extract(new[] { 1, 2, 3 }));

                Assert.AreEqual(
                    "-6,-3-1,3-5,7-11,14,15,17-20",
                    RangeExtraction.Extract(new[] { -6, -3, -2, -1, 0, 1, 3, 4, 5, 7, 8, 9, 10, 11, 14, 15, 17, 18, 19, 20 })
                );

                Assert.AreEqual(
                    "-3--1,2,10,15,16,18-20",
                    RangeExtraction.Extract(new[] { -3, -2, -1, 2, 10, 15, 16, 18, 19, 20 })
                );
            }
        }
    }
}