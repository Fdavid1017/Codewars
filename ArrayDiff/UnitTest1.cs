namespace ArrayDiff
{
    // https://www.codewars.com/kata/523f5d21c841566fde000009/train/csharp
    public class ArrayDifference
    {
        public static int[] ArrayDiff(int[] a, int[] b)
        {
            List<int> diffList = a.ToList();

            foreach (var i in b)
            {
                diffList.RemoveAll(item => item == i);
            }

            return diffList.ToArray();
        }

        [Test]
        public void SampleTest()
        {
            Assert.AreEqual(new int[] { 2 }, ArrayDifference.ArrayDiff(new int[] { 1, 2 }, new int[] { 1 }));
            Assert.AreEqual(new int[] { 2, 2 }, ArrayDifference.ArrayDiff(new int[] { 1, 2, 2 }, new int[] { 1 }));
            Assert.AreEqual(new int[] { 1 }, ArrayDifference.ArrayDiff(new int[] { 1, 2, 2 }, new int[] { 2 }));
            Assert.AreEqual(new int[] { 1, 2, 2 }, ArrayDifference.ArrayDiff(new int[] { 1, 2, 2 }, new int[] { }));
            Assert.AreEqual(new int[] { }, ArrayDifference.ArrayDiff(new int[] { }, new int[] { 1, 2 }));
            Assert.AreEqual(new int[] { 3 }, ArrayDifference.ArrayDiff(new int[] { 1, 2, 3 }, new int[] { 1, 2 }));
        }
    }
}