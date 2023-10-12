using System.Text.RegularExpressions;

namespace TicTacToeChecker
{
    public class Tests
    {
        public class TicTacToe
        {
            private const int emptyMarker = 0;
            private const int xMarker = 1;
            private const int OMarker = 2;

            public int IsSolved(int[,] board)
            {
                int[][] convertedBoard = new int[board.GetLength(0)][];
                bool hasEmptySpaces = false;

                // Check for empty places
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    int[] row = new int[board.GetLength(1)];
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        if (board[x, y] == emptyMarker)
                            hasEmptySpaces = true;


                        row[y] = board[x, y];
                    }
                    convertedBoard[x] = row;
                }

                // Check for horizontal win
                int winner = convertedBoard.Select(row =>
                    row.GroupBy(x => x)
                        .Where(group => group.Count() == 3)
                        .Select(group => (int)group.Key)
                        .FirstOrDefault(-1)
                ).FirstOrDefault(x => x == 1 || x == 2, -1);

                if (winner != -1 && winner != 0)
                {
                    return winner;
                }

                // Check for vertical win

                // (Best method ever ;) )
                int[][] verticalField = new int[][]
                {
                    new int[]
                    {
                        convertedBoard[0][0],
                        convertedBoard[1][0],
                        convertedBoard[2][0],
                    },
                    new int[]
                    {
                        convertedBoard[0][1],
                        convertedBoard[1][1],
                        convertedBoard[2][1],
                    },
                    new int[]
                    {
                        convertedBoard[0][2],
                        convertedBoard[1][2],
                        convertedBoard[2][2],
                    },
                };

                winner = verticalField.Select(row =>
                    row.GroupBy(x => x)
                        .Where(group => group.Count() == 3)
                        .Select(group => (int)group.Key)
                        .FirstOrDefault(-1)
                ).FirstOrDefault(x => x == 1 || x == 2, -1);

                if (winner != -1 && winner != 0)
                {
                    return winner;
                }

                // Check for Diagonal win

                // (Best method ever ;) )
                int[][] diagonalValues = new int[][]
                {
                    new int[]
                    {
                        convertedBoard[0][0],
                        convertedBoard[1][1],
                        convertedBoard[2][2],
                    },
                    new int[]
                    {
                        convertedBoard[0][2],
                        convertedBoard[1][1],
                        convertedBoard[2][0],
                    },
                };
                winner = diagonalValues.Select(row =>
                    row.GroupBy(x => x)
                        .Where(group => group.Count() == 3)
                        .Select(group => (int)group.Key)
                        .FirstOrDefault(-1)
                ).FirstOrDefault(x => x == 1 || x == 2, -1);

                if (winner != -1 && winner != 0)
                {
                    return winner;
                }

                return hasEmptySpaces ? -1 : 0;
            }
        }

        [TestFixture]
        public class TicTacToeTest
        {
            private TicTacToe tictactoe = new TicTacToe();

            [Test]
            public void horizontalXWin()
            {
                int[,] board = new int[,] { { 1, 1, 1 },
                                            { 0, 2, 2 },
                                            { 0, 0, 0 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }

            [Test]
            public void horizontalOWin()
            {
                int[,] board = new int[,] { { 2, 2, 2 },
                                            { 0, 2, 2 },
                                            { 0, 0, 0 } };
                Assert.AreEqual(2, tictactoe.IsSolved(board));
            }
            [Test]
            public void verticalXWin()
            {
                int[,] board = new int[,] { { 1, 0, 0 },
                                            { 1, 2, 2 },
                                            { 1, 0, 0 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }

            [Test]
            public void verticalOWin()
            {
                int[,] board = new int[,] { { 2, 0, 0 },
                                            { 2, 1, 1 },
                                            { 2, 0, 0 } };
                Assert.AreEqual(2, tictactoe.IsSolved(board));
            }

            [Test]
            public void noWinner()
            {
                int[,] board = new int[,] { { 1, 1, 2 },
                                            { 2, 1, 1 },
                                            { 1, 2, 2 } };
                Assert.AreEqual(0, tictactoe.IsSolved(board));
            }

            [Test]
            public void diagoanlXWinner()
            {
                int[,] board = new int[,] { { 1, 0, 0 },
                                            { 2, 1, 0 },
                                            { 2, 0, 1 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }

            [Test]
            public void notFinished()
            {
                int[,] board = new int[,] { { 1, 2, 0 },
                                            { 0, 2, 2 },
                                            { 0, 0, 0 } };
                Assert.AreEqual(-1, tictactoe.IsSolved(board));
            }

            [Test]
            public void test1()
            {
                int[,] board = new int[,] { { 0, 1, 1 },
                                            { 2, 0, 2 },
                                            { 2, 1, 0 } };
                Assert.AreEqual(-1, tictactoe.IsSolved(board));
            }

            [Test]
            public void test2()
            {
                int[,] board = new int[,] { { 2, 1, 1 },
                                            { 0, 1, 1 },
                                            { 2, 2, 2 } };
                Assert.AreEqual(2, tictactoe.IsSolved(board));
            }
        }
    }
}