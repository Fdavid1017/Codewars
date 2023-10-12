using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;

namespace CodewarsStyleRanking
{
    // https://www.codewars.com/kata/51fda2d95d6efda45e00004e

    public class Tests
    {
        public class User
        {
            private int _rank = -8;

            public int progress;

            public int rank
            {
                get => _rank;
                set
                {
                    ValidateRank(value);

                    _rank = value;
                }
            }

            private void ValidateRank(int value)
            {
                if (value == 0 || value > 8 || value < -8)
                    throw new ArgumentException("Range must be between -8 and 8 and cant be 0.");
            }

            public void incProgress(int actRank)
            {
                ValidateRank(actRank);

                Console.WriteLine($"\n\nCurrent rank - progress: {rank} - {progress}");


                int points = 0;
                int rankDifference = rank - actRank;
                if (rank < 0 && actRank > 0)
                {
                    Console.WriteLine("hack 1");
                    rankDifference++;
                }

                if (rank == 1 && actRank == -1)
                {
                    Console.WriteLine("hack 2");
                    rankDifference = 1;
                }

                if (rank == -1 && actRank == 1)
                {
                    Console.WriteLine("hack 3");
                    rankDifference = -1;
                }

                Console.WriteLine($"Kata rank: {actRank}, difference: {rankDifference}");

                if (rankDifference > 0)
                {
                    // Completed a smaller ranked kata
                    if (Math.Abs(rankDifference) == 1)
                    {
                        points = 1;
                    }
                }
                else if (rankDifference < 0)
                {
                    int rankAbsoluteDifference = Math.Abs(rankDifference);
                    points = 10 * rankAbsoluteDifference * rankAbsoluteDifference;
                }
                else
                {
                    points = 3;
                }

                AddProgress(points);
            }

            private void AddProgress(int points)
            {
                if (rank == 8)
                    return;

                progress += points;

                Console.WriteLine($"Adding {points}, total: {progress}");
                while (progress >= 100)
                {
                    progress -= 100;

                    int nextRank = rank + 1;
                    if (nextRank == 0)
                    {
                        nextRank = 1;
                    }
                    rank = nextRank;

                    if (nextRank == 8)
                        progress = 0;

                    Console.WriteLine($"Increasing rank with 1 (new rank: {rank}), progress: {progress}");
                }
            }
        }


        [TestFixture]
        public class UserTest
        {

            // Assert correct rank progression
            public void assertRankProgression(int rank, User user, int expRank, int expProgress)
            {
                Assert.True(user.rank == expRank,
                    "Applied Rank: " + rank +
                    "; Expected rank: " + expRank +
                    "; Actual: " + user.rank);

                Assert.True(user.progress == expProgress,
                    "Applied Rank; " + rank +
                    "; Expected progress: " + expProgress +
                    ", Actual: " + user.progress);
            }

            [TestCase(-7, -8, 10)]
            [TestCase(-6, -8, 40)]
            [TestCase(-5, -8, 90)]
            [TestCase(-4, -7, 60)]
            [TestCase(-8, -8, 3)]
            // Check single increments
            public void testValidSingleRankProgression(int rank, int expectedRank, int expectedProgress)
            {
                User user = new User();

                user.incProgress(rank);

                assertRankProgression(rank, user, expectedRank, expectedProgress);
            }

            [TestCase(-1, -2, 10)]
            // Check single increments
            public void testValidMultiRankProgression(int rank, int expectedRank, int expectedProgress)
            {
                User user = new User();

                user.incProgress(rank);

                assertRankProgression(rank, user, expectedRank, expectedProgress);
            }

            [TestCase(9)]
            [TestCase(-9)]
            [TestCase(0)]
            // Check invalid rank progressions
            public void testInvalidRange(int rank)
            {
                User user = new User();
                Assert.Throws<ArgumentException>(() => user.incProgress(rank));
            }
        }
    }
}