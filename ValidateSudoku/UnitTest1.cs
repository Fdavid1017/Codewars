using System.Runtime.Intrinsics.X86;

namespace ValidateSudoku
{

    // https://www.codewars.com/kata/540afbe2dc9f615d5e000425

    public class SudokuTests
    {

        class Sudoku
        {
            private readonly int[][] data;
            private int littleSquaresSize = 0;

            public Sudoku(int[][] sudokuData)
            {
                this.data = sudokuData;
                this.littleSquaresSize = divisor(sudokuData.Length);

                foreach (var row in sudokuData)
                {
                    Console.WriteLine(string.Join(" ", row));
                }
            }

            public bool IsValid()
            {
                //Check for empty values
                if (data.Length == 0 || data[0].Length == 0)
                {
                    return false;
                }

                //Check for 1x1 data
                if (data.Length == 1 || data[0].Length == 1)
                {
                    return data[0][0] > 0 && data[0][0] < 10;
                }

                // Checking if all filed if filled
                for (int i = 1; i < data.Length; i++)
                {
                    if (data[0].Length != data[i].Length)
                        return false;
                }

                // Check if data size is valid
                if (data[0].Length % littleSquaresSize != 0) return false;

                //Check the whole grid
                if (!CheckRowsAndColumns(data)) return false;


                //Check little squares
                for (int i = 0; i < data.Length / littleSquaresSize; i++)
                {
                    int[][] square = GetSquare(i * littleSquaresSize, i * littleSquaresSize + littleSquaresSize);
                    if (FlattenMap(square).GroupBy(x => x)
                            .Where(item => item.Count() > 1).Count() != 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            private bool CheckRowsAndColumns(int[][] square)
            {
                // Check rows
                for (int i = 0; i < data.Length; i++)
                {
                    if (!ValidateSection(data[i])) return false;
                }

                // Check columns
                for (int i = 0; i < data[0].Length; i++)
                {
                    if (!ValidateSection(GetColumn(i, data))) return false;
                }

                return true;
            }

            private int[][] GetSquare(int from, int to)
            {
                int[][] square = new int[littleSquaresSize][];

                Array.Copy(data, from, square, 0, to - from);

                for (int i = 0; i < square.Length; i++)
                {
                    int[] row = new int[littleSquaresSize];
                    Array.Copy(square[i], from, row, 0, to - from);
                    square[i] = row;
                }

                return square;
            }

            private int[] FlattenMap(int[][] map)
            {
                List<int> flatMap = new List<int>();

                foreach (var row in map)
                {
                    flatMap.AddRange(row);
                }

                return flatMap.ToArray();
            }

            private bool ValidateSection(int[] section)
            {
                return section.GroupBy(x => x)
                   .Where(item => item.Count() > 1).Count() == 0;
            }

            private int[] GetColumn(int index, int[][] values)
            {
                return Enumerable.Range(0, values.GetLength(0))
                    .Select(x => values[x][index])
                    .ToArray();
            }

            private int divisor(int number)
            {
                int i;
                for (i = 2; i <= Math.Sqrt(number); i++)
                {
                    if (number % i == 0)
                    {
                        return number / i;
                    }
                }
                return 1;
            }
        }

        [Test]
        public void Test1()
        {
            var goodSudoku1 = new Sudoku(
                new int[][] {
                    new int[] {7,8,4, 1,5,9, 3,2,6},
                    new int[] {5,3,9, 6,7,2, 8,4,1},
                    new int[] {6,1,2, 4,3,8, 7,5,9},

                    new int[] {9,2,8, 7,1,5, 4,6,3},
                    new int[] {3,5,7, 8,4,6, 1,9,2},
                    new int[] {4,6,1, 9,2,3, 5,8,7},

                    new int[] {8,7,6, 3,9,4, 2,1,5},
                    new int[] {2,4,3, 5,6,1, 9,7,8},
                    new int[] {1,9,5, 2,8,7, 6,3,4}
                });
            Assert.IsTrue(goodSudoku1.IsValid());
        }

        [Test]
        public void Test2()
        {
            var goodSudoku2 = new Sudoku(
                new int[][] {
                    new int[] {1,4, 2,3},
                    new int[] {3,2, 4,1},

                    new int[] {4,1, 3,2},
                    new int[] {2,3, 1,4}
                });
            Assert.IsTrue(goodSudoku2.IsValid());
        }

        [Test]
        public void Test3()
        {
            var badSudoku1 = new Sudoku(
                new int[][] {
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},

                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},

                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9},
                    new int[] {1,2,3, 4,5,6, 7,8,9}
                });
            Assert.IsFalse(badSudoku1.IsValid());
        }

        [Test]
        public void Test4()
        {
            var badSudoku2 = new Sudoku(
                new int[][] {
                    new int[] {1,2,3,4,5},
                    new int[] {1,2,3,4},

                    new int[] {1,2,3,4},
                    new int[] {1}
                });
            Assert.IsFalse(badSudoku2.IsValid());
        }

        [Test]
        public void Custom()
        {
            var badSudoku3 = new Sudoku(
                new int[][] {
                    new int[]{1,2,3, 4,5,6, 7,8,9},
                    new int[]{2,3,1, 5,6,4, 8,9,7},
                    new int[]{3,1,2, 6,4,5, 9,7,8},

                    new int[]{4,5,6, 7,8,9, 1,2,3},
                    new int[]{5,6,4, 8,9,7, 2,3,1},
                    new int[]{6,4,5, 9,7,8, 3,1,2},

                    new int[]{7,8,9, 1,2,3, 4,5,6},
                    new int[]{8,9,7, 2,3,1, 5,6,4},
                    new int[]{9,7,8, 3,1,2, 6,4,5}
                });
            Assert.IsFalse(badSudoku3.IsValid());
        }

        [Test]
        public void Custom2()
        {
            var badSudoku3 = new Sudoku(
                new int[][] {
                    new int[]{2},
                });
            Assert.IsFalse(badSudoku3.IsValid());
        }
    }
}