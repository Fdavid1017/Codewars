namespace BirdMountainRiver
{
    // https://www.codewars.com/kata/5c2fd9188e358f301f5f7a7b
    public class Dinglemouse
    {
        const char PEAK_SYMBOL = '^';
        const char EMPTY_SYMBOL = ' ';
        const char RIVER_SYMBOL = '-';

        public static int[] DryGround(char[][] charMountain)
        {
            if (charMountain.Length == 0)
            {
                return new[] { 0, 0, 0, 0 };
            }

            int[] availablePlaceCount = new int[4];

            string[][] mountain = new string[charMountain.Length][];

            bool containsRiver = false;
            // Converting the terrain (so It supports peaks higher than 9)
            for (int i = 0; i < charMountain.Length; i++)
            {
                mountain[i] = charMountain[i].Select(x => x.ToString()).ToArray();
                if (charMountain[i].Contains(RIVER_SYMBOL))
                {
                    containsRiver = true;
                }
            }

            // Check for river
            if (!containsRiver)
            {
                int terrainSize = mountain.Length * mountain[0].Length;
                return new int[] { terrainSize, terrainSize, terrainSize, terrainSize };
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


            float riverHeight = -0.5f;
            float riverIncreaseByDay = 1f;

            // Day 0
            availablePlaceCount[0] = GetAvailableLandingPlaceCount(mountain);
            Console.WriteLine("\n\nDay 0 {0}\n", riverHeight);
            PrintMountain(mountain);

            // Day 1
            riverHeight += riverIncreaseByDay;
            RaiseRiver(mountain, riverHeight);
            availablePlaceCount[1] = GetAvailableLandingPlaceCount(mountain);
            Console.WriteLine("\n\nDay 1 {0}\n", riverHeight);
            PrintMountain(mountain);

            // Day 2
            riverHeight += riverIncreaseByDay;
            RaiseRiver(mountain, riverHeight);
            availablePlaceCount[2] = GetAvailableLandingPlaceCount(mountain);
            Console.WriteLine("\n\nDay 2 {0}\n", riverHeight);
            PrintMountain(mountain);

            // Day 3
            riverHeight += riverIncreaseByDay;
            RaiseRiver(mountain, riverHeight);
            availablePlaceCount[3] = GetAvailableLandingPlaceCount(mountain);
            Console.WriteLine("\n\nDay 3 {0}\n", riverHeight);
            PrintMountain(mountain);

            Console.WriteLine("{0}, {1}, {2}, {3}", availablePlaceCount[0], availablePlaceCount[1], availablePlaceCount[2], availablePlaceCount[3]);

            return availablePlaceCount;
        }

        static void RaiseRiver(string[][] mountain, float riverHeight)
        {
            for (int i = 0; i < mountain.Length; i++)
            {
                for (int j = 0; j < mountain[i].Length; j++)
                {
                    if (mountain[i][j] == RIVER_SYMBOL.ToString())
                    {
                        RaiseWater(mountain, riverHeight, i, j);
                    }
                }
            }
        }

        static void RaiseWater(string[][] mountain, float waterHeight, int x, int y)
        {
            float currentValue = mountain[x][y] == EMPTY_SYMBOL.ToString()
                ? 0f
                : mountain[x][y] == RIVER_SYMBOL.ToString() ? waterHeight : float.Parse(mountain[x][y]);
            if (currentValue <= waterHeight)
            {
                mountain[x][y] = RIVER_SYMBOL.ToString();
            }
            else
            {
                return;
            }

            List<int[]> neighbors = new List<int[]>();

            if (x > 0)
            {
                neighbors.Add(new int[] { x - 1, y });
            }

            if (x < mountain.Length - 1)
            {
                neighbors.Add(new int[] { x + 1, y });
            }

            if (y > 0)
            {
                neighbors.Add(new int[] { x, y - 1 });
            }

            if (y < mountain[x].Length - 1)
            {
                neighbors.Add(new int[] { x, y + 1 });
            }

            neighbors = neighbors.Where(x => mountain[x[0]][x[1]] != RIVER_SYMBOL.ToString()).ToList();

            for (int i = 0; i < neighbors.Count; i++)
            {
                RaiseWater(mountain, waterHeight, neighbors[i][0], neighbors[i][1]);
            }
        }

        static int GetAvailableLandingPlaceCount(string[][] mountain)
        {
            int count = 0;
            for (int i = 0; i < mountain.Length; i++)
            {
                for (int j = 0; j < mountain[i].Length; j++)
                {
                    string value = mountain[i][j];
                    if (int.TryParse(value, out int t) || value == EMPTY_SYMBOL.ToString())
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        static void NormalizeHeight(string[][] mountain, int i, int j)
        {
            if (
                j < 1
                || mountain[i][j - 1] == EMPTY_SYMBOL.ToString()
                || mountain[i][j - 1] == RIVER_SYMBOL.ToString()
                || mountain[i - 1][j] == RIVER_SYMBOL.ToString()
                )
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
                        .Select(x =>
                        {
                            if (x == RIVER_SYMBOL.ToString())
                            {
                                return 0;
                            }

                            return int.Parse(x.ToString());
                        })
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
            char[][] terrain =
            {
                "  ^^^^^^             ".ToCharArray(),
                "^^^^^^^^       ^^^   ".ToCharArray(),
                "^^^^^^^  ^^^         ".ToCharArray(),
                "^^^^^^^  ^^^         ".ToCharArray(),
                "^^^^^^^  ^^^         ".ToCharArray(),
                "---------------------".ToCharArray(),
                "^^^^^                ".ToCharArray(),
                "   ^^^^^^^^  ^^^^^^^ ".ToCharArray(),
                "^^^^^^^^     ^     ^ ".ToCharArray(),
                "^^^^^        ^^^^^^^ ".ToCharArray()
            };
            Assert.AreEqual(new int[] { 189, 99, 19, 3 }, Dinglemouse.DryGround(terrain));
        }
    }
}