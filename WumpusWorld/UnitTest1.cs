namespace WumpusWorld
{
    // https://www.codewars.com/kata/625c70f8a071210030c8e22a

    public static class WumpusWorld
    {
        class Tile
        {
            public int X { get; init; }
            public int Y { get; init; }
            public int Cost { get; init; } // Tiles traveled from start to get to the current tile
            public int Distance { get; private set; } // Distance to the destination without walls
            public int CostDistance { get; private set; }

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
                CostDistance = Cost + Distance;
            }
        }

        public static bool Solve(char[][] maze)
        {
            const int visitedCharacter = -1;
            const int wallCharacter = 1;

            if (maze.Length <= 1)
            {
                return true;
            }

            // Convert map

            int[][] map = new int[maze.Length][];
            int mapSize = map.Length;

            Tile? finish = null;

            for (int i = 0; i < maze.Length; i++)
            {
                int[] rowItems = new int[maze[i].Length - (i == maze.Length - 1 ? 0 : 1)];
                for (int j = 0; j < rowItems.Length; j++)
                {
                    if (maze[i][j] == '\r') { continue; }

                    if (maze[i][j] == 'P')
                    {
                        rowItems[j] = wallCharacter;
                    }
                    else
                    {
                        rowItems[j] = 0;
                    }

                    if (maze[i][j] == 'G')
                    {
                        finish = new Tile() { X = j, Y = i };
                    }
                }
                map[i] = rowItems;
            }

            if (finish == null)
            {
                return false;
            }

            Tile start = new Tile() { X = 0, Y = 0 };
            start.SetDistance(finish);

            PriorityQueue<Tile, int> openList = new PriorityQueue<Tile, int>();
            openList.Enqueue(start, start.CostDistance);

            while (openList.Count > 0)
            {
                Tile checkTile = openList.Dequeue();
                map[checkTile.Y][checkTile.X] = visitedCharacter;

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    openList.Enqueue(walkableTile, walkableTile.CostDistance);
                }
            }

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(int[][] map, Tile currentTile, Tile targetTile)
            {
                Tile[] possibleTiles = new Tile[]
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Cost = currentTile.Cost + 1 },
                    };

                List<Tile> walkable = new List<Tile>();

                foreach (Tile tile in possibleTiles)
                {
                    if (
                        (tile.X >= 0 && tile.X < mapSize)
                        && (tile.Y >= 0 && tile.Y < mapSize)
                        && map[tile.Y][tile.X] != wallCharacter
                        && map[tile.Y][tile.X] != visitedCharacter
                    )
                    {
                        walkable.Add(tile);
                    }
                }

                return walkable;
            }
        }
    }

    [TestFixture(Description = "Wumpus World")]
    public class SolutionTest
    {
        const bool VERBOSE = true;

        void Act(char[][] cave, bool expected)
        {
            if (VERBOSE) Console.WriteLine($"{Stringify(cave)}\n");
            var userSolution = WumpusWorld.Solve(Copy(cave));
            var msg = $"Failed to predict the outcome for cave:\n\n{Stringify(cave)}\n";
            Assert.AreEqual(expected, userSolution, msg);
        }

        void Act(string cave, bool expected) => Act(ParseCave(cave), expected);
        char[][] ParseCave(string s) => Rasterize(s).Skip(1).ToArray();
        char[][] Rasterize(string s) => s.Split("\n").Select(r => r.ToCharArray()).ToArray();
        T[][] Copy<T>(T[][] matrix) => matrix.Select(r => r.ToArray()).ToArray();
        string Stringify(char[][] matrix) => string.Join("\n", matrix.Select(r => string.Concat(r)));

        [Test(Description = "Sample Tests")]
        [TestCase(@"
____
_W__
___G
P___", true)]
        [TestCase(@"
____
_P__
____
_W_G", true)]
        [TestCase(@"
____
____
W__P
__PG", false)]
        [TestCase(@"
__GP
_P__
W___
____", true)]
        [TestCase(@"
__W_
____
___P
___G", true)]
        [TestCase(@"
__W_
____
__PP
___G", true)]
        [TestCase(@"
__W_
____
_PPP
___G", true)]
        [TestCase(@"
___P
__PG
___P
W___", false)]
        [TestCase(@"
__P_
____
__P_
__WG", true)]
        [TestCase(@"
____
__PW
PG__
____", true)]
        [TestCase(@"
__P_
____
WP__
_G__", true)]
        [TestCase(@"
__PG
____
__WP
____", true)]
        [TestCase(@"
___W
__P_
__G_
P___", true)]
        [TestCase(@"
__WP
_P__
____
_G__", true)]
        [TestCase(@"
__WP
____
__P_
P_G_", true)]
        [TestCase(@"
__PG
___W
__PP
____", true)]
        [TestCase(@"
__P_
____
__G_
PP_W", true)]
        [TestCase(@"
____
____
_P_P
_GWP", true)]
        [TestCase(@"
___P
____
P__P
__GW", true)]
        [TestCase(@"
__WP
____
P__P
___G", true)]
        public void Scenario(string cave, bool expected) => Act(cave, expected);
    }
}