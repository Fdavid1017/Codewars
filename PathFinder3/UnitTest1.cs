namespace PathFinder3
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
                        c += Math.Abs(moveCost - Parent.moveCost);
                    }
                    return c;
                }
                set { cost = value; }
            } // Tiles traveled from to start to get to the current tile
            public int Distance { get; set; } // Distance to the destination without walls
            public int CostDistance => Cost + Distance;
            public Tile Parent { get; set; }
            public int moveCost { get; set; } = 0;

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

            Tile start = new Tile() { X = 0, Y = 0, moveCost = ogMap[0][0] };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1, moveCost = ogMap[0][0] };
            start.SetDistance(finish);

            List<Tile> openList = new List<Tile>() { start };

            while (openList.Count() > 0)
            {
                Tile checkTile = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].CostDistance < checkTile.CostDistance)
                    {
                        checkTile = openList[i];
                    }
                }

                openList.Remove(checkTile);
                map[checkTile.Y][checkTile.X] = visitedCharacter;

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    int climbRounds = 0;
                    Tile currentTile = checkTile;
                    while (currentTile != null && currentTile.Parent != null)
                    {
                        climbRounds += Math.Abs(currentTile.moveCost - currentTile.Parent.moveCost);
                        currentTile = currentTile.Parent;

                        ogMap[currentTile.X][currentTile.Y] = -10;
                    }
                    return climbRounds;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //It's already in the active list, but that's OK, maybe this new tile has a shorter path to it
                    bool inActiveList = false;

                    for (int i = 0; i < openList.Count; i++)
                    {
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
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                    };

                List<Tile> walkable = new List<Tile>();

                foreach (Tile tile in possibleTiles)
                {
                    if (
                        (tile.X >= 0 && tile.X < mapSize)
                        && (tile.Y >= 0 && tile.Y < mapSize)
                        && map[tile.Y][tile.X] != visitedCharacter
                    )
                    {
                        tile.moveCost = ogMap[tile.X][tile.Y];
                        walkable.Add(tile);
                    }
                }

                return walkable;
            }
        }

        public static void PrintMap(int[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    Console.Write(map[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        [Test]
        public void SampleTests()
        {

            string a = "000\n" +
                       "000\n" +
                       "000",

                   b = "010\n" +
                       "010\n" +
                       "010",

                   c = "010\n" +
                       "101\n" +
                       "010",

                   d = "0707\n" +
                       "7070\n" +
                       "0707\n" +
                       "7070",

                   e = "700000\n" +
                       "077770\n" +
                       "077770\n" +
                       "077770\n" +
                       "077770\n" +
                       "000007",

                   f = "777000\n" +
                       "007000\n" +
                       "007000\n" +
                       "007000\n" +
                       "007000\n" +
                       "007777",

                   g = "000000\n" +
                       "000000\n" +
                       "000000\n" +
                       "000010\n" +
                       "000109\n" +
                       "001010";

            Assert.AreEqual(0, Finder.PathFinder(a));
            Assert.AreEqual(2, Finder.PathFinder(b));
            Assert.AreEqual(4, Finder.PathFinder(c));
            Assert.AreEqual(42, Finder.PathFinder(d));
            Assert.AreEqual(14, Finder.PathFinder(e));
            Assert.AreEqual(0, Finder.PathFinder(f));
            Assert.AreEqual(4, Finder.PathFinder(g));
        }
    }
}