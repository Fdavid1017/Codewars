namespace PersistentBugger
{
    // https://www.codewars.com/kata/55bf01e5a717a0d57e0000ec
    public class Persist
    {
        public static int Persistence(long n)
        {
            string numberText = n.ToString();
            int newNumber = 0;
            int persistence = 0;

            while (numberText.Length > 1)
            {
                newNumber = int.Parse(numberText[0].ToString());
                for (int i = 1; i < numberText.Length; i++)
                {
                    newNumber *= int.Parse(numberText[i].ToString());
                }
                numberText = newNumber.ToString();
                persistence++;
            }

            return persistence;
        }


        [Test]
        public void Test1()
        {
            Assert.AreEqual(3, Persist.Persistence(39));
            Assert.AreEqual(0, Persist.Persistence(4));
            Assert.AreEqual(2, Persist.Persistence(25));
            Assert.AreEqual(4, Persist.Persistence(999));
        }
    }
}