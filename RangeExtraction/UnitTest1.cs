namespace RangeExtraction
{
    // https://www.codewars.com/kata/51ba717bb08c1cd60f00002f

    public class RangeExtraction
    {
        public static string Extract(int[] args)
        {
            List<string> ranges = new List<string>();

            int? rangeStart = args[0];
            int rangeEnd = args[1];

            for (int i = 1; i < args.Length; i++)
            {
                if (!rangeStart.HasValue)
                {
                    rangeStart = args[i];
                    continue;
                }

                rangeEnd = args[i];
                if (args.Length == i + 1 || Math.Abs(rangeEnd - args[i + 1]) > 1)
                {
                    // Not in range with the next item
                    if (Math.Abs(rangeStart.Value - rangeEnd) == 1 || args[i - 1] == rangeStart)
                    {
                        // No element between start and end OR not a range with the previous item
                        ranges.Add($"{rangeStart},{rangeEnd}");
                    }
                    else
                    {
                        ranges.Add($"{rangeStart}-{rangeEnd}");
                    }
                    rangeStart = null;
                }
                else
                {
                    // In range with previous item
                }
            }


            return string.Join(",", ranges);
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