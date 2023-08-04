using System.Reflection;
using System.Reflection.Emit;

namespace Solution
{
    // https://www.codewars.com/kata/54bd6b4c956834c9870001a1

    sealed class Bagel
    {
        public int Value { get; private set; } = 3;
    }

    class BagelSolver
    {
        public static Bagel Bagel
        {
            get
            {
                Bagel instance = new Bagel();
                Type type = instance.GetType();

                PropertyInfo prop = type.GetProperty("Value");

                prop.SetValue(instance, 4, null);
                return instance;
            }
        }
    }

    [TestFixture]
    public class BagelTest
    {
        [Test]
        public void TestBagel()
        {
            Bagel bagel = BagelSolver.Bagel;
            NUnit.Framework.Assert.AreEqual(4, bagel.Value);
        }
    }
}