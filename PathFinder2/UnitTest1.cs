namespace PathFinder2
{
    public class Finder
    {
        class Tile
        {
            private int cost;

            public int X { get; set; }
            public int Y { get; set; }
            public int Cost
            {
                get
                {
                    int c = cost;
                    if (Parent != null)
                    {
                        c += Math.Abs(MoveCost - Parent.MoveCost);
                    }
                    return c;
                }
                set { cost = value; }
            } // Tiles traveled from to start to get to the current tile
            public int Distance { get; set; } // Distance to the destination without walls
            public int CostDistance { get; set; } // Cost and distance;
            public Tile Parent { get; set; }
            public int MoveCost { get; set; } = 0;

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
            }
        }

        public static int PathFinder(string maze)
        {
            const int visitedCharacter = -1;

            if (maze.Length <= 1)
            {
                return 0;
            }

            // Convert map
            string[] rows = maze.Split("\n");

            int[][] map = new int[rows.Length][];
            int[][] ogMap = new int[rows.Length][];
            int mapSize = map.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                int[] rowItems = new int[rows.Length];
                int[] ogRowItems = new int[rows.Length];
                for (int j = 0; j < rows[i].Length; j++)
                {
                    rowItems[j] = int.Parse(rows[i][j].ToString());
                    ogRowItems[j] = int.Parse(rows[i][j].ToString());
                }
                map[i] = rowItems;
                ogMap[i] = ogRowItems;
            }

            Tile start = new Tile() { X = 0, Y = 0, MoveCost = ogMap[0][0] };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1, MoveCost = ogMap[mapSize - 1][mapSize - 1] };
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
                    int climbRounds = 0;
                    Tile currentTile = checkTile;
                    while (currentTile != null && currentTile.Parent != null)
                    {
                        climbRounds += Math.Abs(currentTile.MoveCost - currentTile.Parent.MoveCost);
                        currentTile = currentTile.Parent;

                        ogMap[currentTile.X][currentTile.Y] = -10;
                    }
                    return climbRounds;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    bool inActiveList = false;

                    for (int i = 0; i < openList.Count; i++)
                    {
                        openList.
                        if (openList[i].X == walkableTile.X && openList[i].Y == walkableTile.Y)
                        {
                            inActiveList = true;
                            break;
                        }
                    }

                    if (inActiveList)
                    {
                        foreach (Tile tile in walkableTiles)
                        {
                            if (tile.X == walkableTile.X && tile.Y == walkableTile.Y)
                            {
                                Tile existingTile = tile;

                                if (existingTile.CostDistance > checkTile.CostDistance)
                                {
                                    openList.Remove(existingTile);
                                    openList.Add(walkableTile);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Its a new tile
                        openList.Add(walkableTile);
                    }
                }
            }

            return -1;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(int[][] map, Tile currentTile, Tile targetTile)
            {
                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Cost = currentTile.Cost + 1},
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
                        walkable.Add(tile);
                }

                return walkable;
            }
        }

        [Test]
        public void TestBasic()
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

            Assert.AreEqual(4, Finder.PathFinder(a));
            Assert.AreEqual(-1, Finder.PathFinder(b));
            Assert.AreEqual(10, Finder.PathFinder(c));
            Assert.AreEqual(-1, Finder.PathFinder(d));
        }
    }
}