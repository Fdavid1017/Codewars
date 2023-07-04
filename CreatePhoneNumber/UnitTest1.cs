namespace CreatePhoneNumber
{
    public class Kata
    {
        public static string CreatePhoneNumber(int[] numbers)
        {
            return string.Format("({0}) {1}-{2}", string.Join("", numbers.Take(3)), string.Join("", numbers.Skip(3).Take(3)), string.Join("", numbers.Skip(6)));
        }

        [Test]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, ExpectedResult = "(123) 456-7890")]
        [TestCase(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, ExpectedResult = "(111) 111-1111")]
        public static string FixedTest(int[] numbers)
        {
            return Kata.CreatePhoneNumber(numbers);
        }
    }
}