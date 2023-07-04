namespace TicTacToeChecker
{
    public class Tests
    {
        public class TicTacToe
        {
            public int IsSolved(int[,] board)
            {
                foreach (int i in board)
                {
                    

                }

                return -1;
            }
        }

        [TestFixture]
        public class TicTacToeTest
        {
            private TicTacToe tictactoe = new TicTacToe();

            [Test]
            public void test1()
            {
                int[,] board = new int[,] { { 1, 1, 1 }, { 0, 2, 2 }, { 0, 0, 0 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }

            [Test]
            public void test2()
            {
                int[,] board = new int[,] { { 2, 2, 2 }, { 0, 2, 2 }, { 0, 0, 0 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }

            [Test]
            public void test3()
            {
                int[,] board = new int[,] { { 1, 2, 0 }, { 0, 2, 2 }, { 0, 0, 0 } };
                Assert.AreEqual(1, tictactoe.IsSolved(board));
            }
        }
    }
}