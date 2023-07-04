using System.Diagnostics;

namespace DontRelyOnLuck
{
    public class KataClass
    {
        public static int Guess
        {
            get
            {
                return new Random((int)DateTime.Now.Ticks).Next(1, 100 + 1);
            }
        }

        [Test]
        public void Test()
        {
            var random = new Random().Next(1, 100 + 1);
            Assert.AreEqual(random, KataClass.Guess);
        }
    }
}