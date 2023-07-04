namespace BirdMountain
{
    // https://www.codewars.com/kata/bird-mountain
    public class Dinglemouse
    {
        const char PEAK_SYMBOL = '^';
        const char EMPTY_SYMBOL = ' ';

        public static int PeakHeight(char[][] charMountain)
        {
            string[][] mountain = new string[charMountain.Length][];

            bool containsPeak = false;
            for (int i = 0; i < charMountain.Length; i++)
            {
                mountain[i] = charMountain[i].Select(x => x.ToString()).ToArray();
                if (charMountain[i].Contains(PEAK_SYMBOL))
                {
                    containsPeak = true;
                }
            }

            if (!containsPeak)
            {
                return 0;
            }

            // Setting bottom row height
            for (int i = 0; i < charMountain[^1].Length; i++)
            {
                if (charMountain[^1][i] == PEAK_SYMBOL)
                {
                    mountain[^1][i] = "1";
                }
            }

            for (int i = 0; i < mountain.Length; i++)
            {
                for (int j = 0; j < mountain[i].Length; j++)
                {
                    if (charMountain[i][j] != PEAK_SYMBOL)
                    {
                        mountain[i][j] = charMountain[i][j].ToString();
                        continue;
                    }

                    // Setting top row height
                    if (i == 0 || i == mountain.Length - 1)
                    {
                        mountain[i][j] = "1";
                        continue;
                    }

                    if (i == 5 && j == 8)
                    {
                    }

                    mountain[i][j] = GetPeakHeight(mountain, i, j).ToString();

                    NormalizeHeight(mountain, i, j);
                }
            }

            PrintMountain(mountain);

            int largest = 1;

            for (int i = 0; i < mountain.Length; i++)
            {
                for (int j = 0; j < mountain[i].Length; j++)
                {
                    if (mountain[i][j] == EMPTY_SYMBOL.ToString())
                    {
                        continue;
                    }

                    int value = int.Parse(mountain[i][j].ToString());
                    if (value > largest)
                    {
                        largest = value;
                    }
                }
            }

            return largest;
        }

        static void NormalizeHeight(string[][] mountain, int i, int j)
        {
            if (j < 1 || mountain[i][j - 1] == EMPTY_SYMBOL.ToString())
            {
                return;
            }

            int currentHeight = int.Parse(mountain[i][j].ToString());
            int previousHeight = int.Parse(mountain[i][j - 1].ToString());

            if (Math.Abs(previousHeight - currentHeight) > 1)
            {
                mountain[i][j - 1] = GetPeakHeight(mountain, i, j - 1).ToString();
                NormalizeHeight(mountain, i, j - 1);
            }

            if (i < 1 || mountain[i - 1][j] == EMPTY_SYMBOL.ToString())
            {
                return;
            }

            previousHeight = int.Parse(mountain[i - 1][j].ToString());

            if (Math.Abs(previousHeight - currentHeight) > 1)
            {
                mountain[i - 1][j] = GetPeakHeight(mountain, i - 1, j).ToString();
                NormalizeHeight(mountain, i - 1, j);
            }
        }

        static int GetPeakHeight(string[][] mountain, int i, int j)
        {
            List<string> neighbors = new List<string>()
            {
                 mountain[i - 1][j],
                 mountain[i + 1][j],
            };

            if (j > 0)
            {
                neighbors.Add(mountain[i][j - 1]);
            }
            else
            {
                neighbors.Add(EMPTY_SYMBOL.ToString());
            }

            if (j < mountain[i].Length - 1)
            {
                neighbors.Add(mountain[i][j + 1]);
            }
            else
            {
                neighbors.Add(EMPTY_SYMBOL.ToString());
            }

            if (neighbors.Contains(EMPTY_SYMBOL.ToString()))
            {
                return 1;
            }

            int smallestNeighbor = neighbors
                        .Where(x => x != PEAK_SYMBOL.ToString())
                        .Select(x => int.Parse(x.ToString()))
                        .Min();

            int currentHeight = (smallestNeighbor + 1);
            return currentHeight;
        }

        static void PrintMountain(string[][] mountain)
        {
            for (int i = 0; i < mountain.Length; i++)
            {
                for (int j = 0; j < mountain[i].Length; j++)
                {
                    Console.Write(mountain[i][j]);
                }
                Console.WriteLine();
            }
        }


        [Test]
        public void Ex()
        {
            char[][] mountain =
            {
                "^^^^^^        ".ToCharArray(),
                " ^^^^^^^^     ".ToCharArray(),
                "  ^^^^^^^     ".ToCharArray(),
                "  ^^^^^       ".ToCharArray(),
                "  ^^^^^^^^^^^ ".ToCharArray(),
                "  ^^^^^^      ".ToCharArray(),
                "  ^^^^        ".ToCharArray()
            };
            Assert.AreEqual(3, Dinglemouse.PeakHeight(mountain));
        }

        [Test]
        public void Misc1()
        {
            char[][] mountain =
            {
                "^^   ^^^  ^^".ToCharArray(),
                "^ ^^  ^^^   ".ToCharArray(),
                "  ^^^   ^^  ".ToCharArray(),
                "    ^^ ^^   ".ToCharArray(),
                "   ^  ^     ".ToCharArray(),
                "    ^^      ".ToCharArray(),
                " ^^^^^^^^   ".ToCharArray(),
                "  ^^^^^^^^  ".ToCharArray(),
                " ^^ ^^^   ^^".ToCharArray(),
                "^^^    ^^ ^^".ToCharArray(),
            };
            Assert.AreEqual(2, Dinglemouse.PeakHeight(mountain));
        }

        [Test]
        public void Misc2()
        {
            char[][] mountain =
            {
                "     ^^^^^^ ".ToCharArray(),
                " ^^^^^^^^   ".ToCharArray(),
                "^^^^^^^^^   ".ToCharArray(),
                "  ^^^^^^^^  ".ToCharArray(),
                "  ^^^^^^^^^ ".ToCharArray(),
                "^^^^^^^^^^^ ".ToCharArray(),
                "^^^^^^^^^^^ ".ToCharArray(),
                "  ^^^^^^^^^ ".ToCharArray(),
                "  ^^^^^^^^  ".ToCharArray(),
                "  ^^^^^^^   ".ToCharArray(),
                "  ^^^^^^    ".ToCharArray(),
                "   ^^^^^^   ".ToCharArray(),
                "    ^^^^^   ".ToCharArray(),
                "      ^^    ".ToCharArray(),
            };
            Assert.AreEqual(5, Dinglemouse.PeakHeight(mountain));
        }
    }
}