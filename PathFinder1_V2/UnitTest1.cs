namespace PathFinder1_V2
{
    public class Finder
    {
        class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Cost { get; set; } // Tiles traveled from to start to get to the current tile
            public int Distance { get; set; } // Distance to the destination without walls
            public int CostDistance { get; set; }

            public Tile()
            {
                CostDistance = Cost + Distance;
            }

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
                CostDistance = Cost + Distance;
            }
        }

        public class DuplicateKeyComparer<TKey>
            :
                IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;
                else
                    return result;
            }
        }

        public static bool PathFinder(string maze)
        {
            const int visitedCharacter = -1;
            const int wallCharacter = 1;

            if (maze.Length <= 1)
            {
                return true;
            }

            // Convert map
            string[] rows = maze.Split("\n");

            int[][] map = new int[rows.Length][];
            int mapSize = map.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                int[] rowItems = new int[rows.Length];
                for (int j = 0; j < rows[i].Length; j++)
                {
                    rowItems[j] = rows[i][j] == 'W' ? 1 : 0;
                }
                map[i] = rowItems;
            }

            Tile start = new Tile() { X = 0, Y = 0 };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1 };
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

        [Test]
        public void sampleTests()
        {

            string a = ".W.\n" +
                       ".W.\n" +
                       "...",

                   b = ".W.\n" +
                       ".W.\n" +
                       "W..",

                   c = "......\n" +
                       "......\n" +
                       "......\n" +
                       "......\n" +
                       "......\n" +
                       "......",

                   d = "......\n" +
                       "......\n" +
                       "......\n" +
                       "......\n" +
                       ".....W\n" +
                       "....W.";

            Assert.AreEqual(true, Finder.PathFinder(a));
            Assert.AreEqual(false, Finder.PathFinder(b));
            Assert.AreEqual(true, Finder.PathFinder(c));
            Assert.AreEqual(false, Finder.PathFinder(d));
        }
    }
}