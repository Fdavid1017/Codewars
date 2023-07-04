using NUnit.Framework.Internal;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace ConnectFour
{
    // https://www.codewars.com/kata/56882731514ec3ec3d000009

    public class ConnectFour
    {
        public static string WhoIsWinner(List<string> piecesPositionList)
        {
            string[,] map = new string[7, 6];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = "-";
                }
            }

            int asd = -1;
            foreach (string piece in piecesPositionList)
            {
                asd++;
                int colNo = GetColumnNumber(piece);
                string color = GetColor(piece);

                int placedIndex = AddToColumn(colNo, color);
                string winner = Validate(colNo, placedIndex);

                if (!string.IsNullOrEmpty(winner))
                {
                    for (int i = 0; i < map.GetLength(0); i++)
                    {
                        for (int j = 0; j < map.GetLength(1); j++)
                        {
                            string value = map[i, j] == "Red" ? "R" : "Y";

                            if (map[i, j] == null)
                            {
                                value = "-";
                            }
                            Console.Write(value + " ");
                        }

                        Console.WriteLine();
                    }

                    return winner;
                }
            }

            return "Draw";

            /* ===== LOCAL FUNCTIONS =====*/

            int GetColumnNumber(string text)
            {
                return text[0] - 65;
            }

            // -1 = RED, 1 = YELLOW
            string GetColor(string text)
            {
                return text.Substring(2);
            }

            int AddToColumn(int col, string value)
            {
                for (int i = 0; i < map.GetLength(1); i++)
                {
                    if (map[col, i] == "-")
                    {
                        map[col, i] = value;
                        return i;
                    }
                }

                throw new ArgumentOutOfRangeException($"Col {col} is full");
            }

            string[] GetColumn(int columnIndex)
            {
                return Enumerable.Range(0, map.GetLength(0))
                    .Select(x => map[x, columnIndex] == null ? "-" : map[x, columnIndex])
                    .ToArray();
            }

            string[] GetRow(int rowNumber)
            {
                return Enumerable.Range(0, map.GetLength(1))
                    .Select(x => map[rowNumber, x] == null ? "-" : map[rowNumber, x])
                    .ToArray();
            }

            string Validate(int lastPlacedCol, int placedIndex)
            {
                string placedColor = map[lastPlacedCol, placedIndex];

                string fourInARow = @"(" + placedColor + "){4}";

                // Vertical
                string vertical = string.Join("", GetRow(lastPlacedCol));
                if (Regex.IsMatch(vertical, fourInARow))
                {
                    return placedColor;
                }

                // Horizontal
                string horizontal = string.Join("", GetColumn(placedIndex));
                if (Regex.IsMatch(horizontal, fourInARow))
                {
                    return placedColor;
                }

                // Diagonal 1
                string diagonal = "";

                // Values up
                for (int i = lastPlacedCol, j = placedIndex; i < map.GetLength(0) && j < map.GetLength(1); i++, j++)
                {
                    diagonal += map[i, j];
                }

                // Values down
                for (int i = lastPlacedCol - 1, j = placedIndex - 1; i >= 0 && j >= 0; i--, j--)
                {
                    diagonal = map[i, j] + diagonal;
                }

                if (Regex.IsMatch(diagonal, fourInARow))
                {
                    return placedColor;
                }

                // Diagonal 2
                diagonal = "";

                // Values up
                for (int i = lastPlacedCol, j = placedIndex; i < map.GetLength(0) && j >= 0; i++, j--)
                {
                    diagonal += map[i, j];
                }

                // Values down
                for (int i = lastPlacedCol - 1, j = placedIndex + 1; j >= 0 && j < map.GetLength(1) && i < map.GetLength(0) && i >= 0; i--, j++)
                {
                    diagonal = map[i, j] + diagonal;
                }

                if (Regex.IsMatch(diagonal, fourInARow))
                {
                    return placedColor;
                }

                return string.Empty;
            }
        }


        [TestFixture]
        public class MyTestConnectFour
        {
            [Test]
            public void FirstTest()
            {
                List<string> myList = new List<string>()
            {
                "A_Red",
                "B_Yellow",
                "A_Red",
                "B_Yellow",
                "A_Red",
                "B_Yellow",
                "G_Red",
                "B_Yellow"
            };
                StringAssert.AreEqualIgnoringCase("Yellow", ConnectFour.WhoIsWinner(myList), "it should return Yellow");
            }

            [Test]
            public void SecondTest()
            {
                List<string> myList = new List<string>()
            {
                "C_Yellow",
                "E_Red",
                "G_Yellow",
                "B_Red",
                "D_Yellow",
                "B_Red",
                "B_Yellow",
                "G_Red",
                "C_Yellow",
                "C_Red",
                "D_Yellow",
                "F_Red",
                "E_Yellow",
                "A_Red",
                "A_Yellow",
                "G_Red",
                "A_Yellow",
                "F_Red",
                "F_Yellow",
                "D_Red",
                "B_Yellow",
                "E_Red",
                "D_Yellow",
                "A_Red",
                "G_Yellow", // WIN
                "D_Red",
                "D_Yellow",
                "C_Red"
            };
                StringAssert.AreEqualIgnoringCase("Yellow", ConnectFour.WhoIsWinner(myList));
            }

            [Test]
            public void ThirdTest()
            {
                List<string> myList = new List<string>()
            {
                "A_Yellow",
                "B_Red",
                "B_Yellow",
                "C_Red",
                "G_Yellow",
                "C_Red",
                "C_Yellow",
                "D_Red",
                "G_Yellow",
                "D_Red",
                "G_Yellow",
                "D_Red",
                "F_Yellow",
                "E_Red",
                "D_Yellow"
            };
                StringAssert.AreEqualIgnoringCase("Red", ConnectFour.WhoIsWinner(myList), "it should return Red");
            }

            [Test]
            public void T5()
            {
                List<string> myList = new List<string>()
                {
                    "B_Yellow",
                    "C_Red",
                    "C_Yellow",
                    "A_Red",
                    "A_Yellow",
                    "E_Red",
                    "D_Yellow",
                    "F_Red",
                    "A_Yellow",
                    "D_Red",
                    "G_Yellow",
                    "E_Red",
                    "G_Yellow",
                    "C_Red",
                    "C_Yellow",
                    "E_Red",
                    "E_Yellow",
                    "C_Red",
                    "A_Yellow",
                    "G_Red",
                    "C_Yellow",
                    "A_Red",
                    "D_Yellow",
                    "G_Red",
                    "D_Yellow",
                    "F_Red",
                    "B_Yellow",
                    "B_Red",
                    "D_Yellow",
                    "E_Red",
                    "B_Yellow",
                    "G_Red",
                    "D_Yellow",
                    "F_Red",
                    "A_Yellow",
                    "B_Red",
                    "G_Yellow",
                    "B_Red",
                    "F_Yellow",
                    "E_Red",
                    "F_Yellow",
                    "F_Red",
                };
                StringAssert.AreEqualIgnoringCase("Yellow", ConnectFour.WhoIsWinner(myList), "it should return Red");
            }

            [Test]
            public void T4()
            {
                List<string> myList = new List<string>()
                {
                    "C_Yellow",
                    "B_Red",
                    "B_Yellow",
                    "E_Red",
                    "D_Yellow",
                    "G_Red",
                    "B_Yellow",
                    "G_Red",
                    "E_Yellow",
                    "A_Red",
                    "G_Yellow",
                    "C_Red",
                    "A_Yellow",
                    "A_Red",
                    "D_Yellow",
                    "B_Red",
                    "G_Yellow",
                    "A_Red",
                    "F_Yellow",
                    "B_Red",
                    "D_Yellow",
                    "A_Red",
                    "F_Yellow",
                    "F_Red",
                    "B_Yellow",
                    "F_Red",
                    "F_Yellow",
                    "G_Red",
                    "A_Yellow",
                    "F_Red",
                    "C_Yellow",
                    "C_Red",
                };
                StringAssert.AreEqualIgnoringCase("Yellow", ConnectFour.WhoIsWinner(myList), "it should return Red");
            }
        }
    }
}