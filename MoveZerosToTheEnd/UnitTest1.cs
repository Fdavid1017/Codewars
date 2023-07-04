namespace MoveZerosToTheEnd
{
    public class Kata
    {
        public static int[] MoveZeroes(int[] arr)
        {
            List<int> list = arr.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == 0)
                {
                    int item = list[i];
                    list.RemoveAt(i);
                    list.Add(item);
                }
            }

            return list.ToArray();
        }

        [Test]
        public void Test()
        {
            Assert.AreEqual(new int[] { 1, 2, 1, 1, 3, 1, 0, 0, 0, 0 }, Kata.MoveZeroes(new int[] { 1, 2, 0, 1, 0, 1, 0, 3, 0, 1 }));
        }
    }
}