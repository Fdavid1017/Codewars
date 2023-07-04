namespace MakeASpiral
{
    // https://www.codewars.com/kata/534e01fbbb17187c7e0000c6

    public class Spiralizor
    {
        public static int[,] Spiralize(int size)
        {
            size = 17;

            int[,] map = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j] = 0;
                }
            }

            for (int i = 0; i < size; i += 2)
            {
                // Vizszintes jobbra
                for (int j = i == 0 ? i : i - 2; j < size - i - 1; j++)
                {
                    map[i, j] = 1;
                }

                // Függõleges le
                for (int j = i; j < size - i; j++)
                {
                    map[j, size - i - 1] = 1;
                }

                // Vizszintes balra
                for (int j = i; j < size - i - 1; j++)
                {
                    if (map[size - i - 2, j] != 1)
                    {
                        map[size - i - 1, j] = 1;
                    }
                }

                // Függõleges fel
                for (int j = i + 2; j < size - i; j++)
                {
                    map[j, i] = 1;
                }
            }


            // Visualizing for debug
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

            return map;
        }

        [TestFixture]
        public class SolutionTest
        {
            [Test]
            public void Test05()
            {
                int input = 5;
                int[,] expected = new int[,]{
                    {1, 1, 1, 1, 1},
                    {0, 0, 0, 0, 1},
                    {1, 1, 1, 0, 1},
                    {1, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1}
                };

                int[,] actual = Spiralizor.Spiralize(input);
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Test08()
            {
                int input = 8;
                int[,] expected = new int[,]{
                    {1, 1, 1, 1, 1, 1, 1, 1},
                    {0, 0, 0, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1, 0, 1},
                    {1, 0, 0, 0, 0, 1, 0, 1},
                    {1, 0, 1, 0, 0, 1, 0, 1},
                    {1, 0, 1, 1, 1, 1, 0, 1},
                    {1, 0, 0, 0, 0, 0, 0, 1},
                    {1, 1, 1, 1, 1, 1, 1, 1},
                };

                int[,] actual = Spiralizor.Spiralize(input);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}