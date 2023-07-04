using System.Text;

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
            public Tile Parent { get; set; }

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
            }
        }

        public static bool PathFinder1(string maze)
        {
            if (maze.Length <= 1)
            {
                return true;
            }

            string[] map = maze.Split("\n");
            int mapSize = map.Length;

            Tile start = new Tile() { X = 0, Y = 0 };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1 };
            start.SetDistance(finish);

            List<Tile> activeTiles = new List<Tile>() { start };
            List<Tile> visitedTiles = new List<Tile>();

            while (activeTiles.Count() > 0)
            {
                Tile checkTile = activeTiles.OrderBy(x => x.CostDistance).First();
                activeTiles.Remove(checkTile);
                visitedTiles.Add(checkTile);

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a shorter path to it
                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        // Its a new tile
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(string[] map, Tile currentTile, Tile targetTile)
            {
                const char wallCharacter = 'W';

                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                    };

                possibleTiles.ForEach(tile => tile.SetDistance(targetTile));

                return possibleTiles
                        .Where(tile =>
                        (
                            tile.X >= 0 && tile.X < mapSize)
                            && (tile.Y >= 0 && tile.Y < mapSize)
                            && map[tile.Y][tile.X] != wallCharacter
                        )
                        .ToList();
            }
        }

        public static bool PathFinder2(string maze)
        {
            if (maze.Length <= 1)
            {
                return true;
            }

            string[] map = maze.Split("\n");
            int mapSize = map.Length;

            Tile start = new Tile() { X = 0, Y = 0 };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1 };
            start.SetDistance(finish);

            List<Tile> activeTiles = new List<Tile>() { start };
            List<Tile> visitedTiles = new List<Tile>();

            while (activeTiles.Count() > 0)
            {
                //Tile checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                Tile checkTile = activeTiles[0];
                for (int i = 1; i < activeTiles.Count; i++)
                {
                    if (activeTiles[i].CostDistance < checkTile.CostDistance)
                    {
                        checkTile = activeTiles[i];
                    }
                }

                activeTiles.Remove(checkTile);
                visitedTiles.Add(checkTile);

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again
                    //if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    //    continue;

                    bool visited = false;
                    for (int i = 0; i < visitedTiles.Count && !visited; i++)
                    {
                        visited = visitedTiles[i].X == walkableTile.X && visitedTiles[i].Y == walkableTile.Y;
                    }

                    if (visited)
                    {
                        continue;
                    }

                    //It's already in the active list, but that's OK, maybe this new tile has a shorter path to it

                    bool inActiveList = false;

                    for (int i = 0; i < activeTiles.Count; i++)
                    {
                        if (activeTiles[i].X == walkableTile.X && activeTiles[i].Y == walkableTile.Y)
                        {
                            inActiveList = true;
                            break;
                        }
                    }

                    //if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    if (inActiveList)
                    {
                        //var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        //if (existingTile.CostDistance > checkTile.CostDistance)
                        //{
                        //    activeTiles.Remove(existingTile);
                        //    activeTiles.Add(walkableTile);
                        //}

                        foreach (Tile tile in walkableTiles)
                        {
                            if (tile.X == walkableTile.X && tile.Y == walkableTile.Y)
                            {
                                Tile existingTile = tile;

                                if (existingTile.CostDistance > checkTile.CostDistance)
                                {
                                    activeTiles.Remove(existingTile);
                                    activeTiles.Add(walkableTile);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Its a new tile
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(string[] map, Tile currentTile, Tile targetTile)
            {
                const char wallCharacter = 'W';

                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                    };

                List<Tile> walkable = new List<Tile>();

                foreach (Tile tile in possibleTiles)
                {
                    if (
                        (tile.X >= 0 && tile.X < mapSize)
                        && (tile.Y >= 0 && tile.Y < mapSize)
                        && map[tile.Y][tile.X] != wallCharacter
                    )
                        walkable.Add(tile);
                }

                return walkable;

                //possibleTiles.ForEach(tile => tile.SetDistance(targetTile));

                //return possibleTiles
                //        .Where(tile =>
                //        (
                //            tile.X >= 0 && tile.X < mapSize)
                //            && (tile.Y >= 0 && tile.Y < mapSize)
                //            && map[tile.Y][tile.X] != wallCharacter
                //        )
                //        .ToList();
            }
        }

        public static bool PathFinder3(string maze)
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
                    return true;
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

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(int[][] map, Tile currentTile, Tile targetTile)
            {
                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
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

        public static bool PathFinder4(string maze)
        {
            const int visitedCharacter = -1;
            const int wallCharacter = 1;

            if (maze.Length <= 1)
            {
                return true;
            }

            // Convert map (CHANGED)
            int mapSize = (int)Math.Sqrt(maze.Length);
            int[,] map = new int[mapSize, mapSize];

            for (int i = 0; i < maze.Length; i++)
            {
                int row = Math.Clamp(i / mapSize, 0, mapSize - 1);
                int col = Math.Clamp(i % mapSize, 0, mapSize - 1);
                map[row, col] = maze[i] == 'W' ? wallCharacter : 0;
            }

            Tile start = new Tile() { X = 0, Y = 0 };
            Tile finish = new Tile() { X = mapSize - 1, Y = mapSize - 1 };
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
                map[checkTile.Y, checkTile.X] = visitedCharacter;

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
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

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(int[,] map, Tile currentTile, Tile targetTile)
            {
                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                    };

                List<Tile> walkable = new List<Tile>();

                foreach (Tile tile in possibleTiles)
                {
                    if (
                        (tile.X >= 0 && tile.X < mapSize)
                        && (tile.Y >= 0 && tile.Y < mapSize)
                        && map[tile.Y, tile.X] != wallCharacter
                        && map[tile.Y, tile.X] != visitedCharacter
                    )
                        walkable.Add(tile);
                }

                return walkable;
            }
        }

        /// <summary>
        /// Comparer for comparing two keys, handling equality as beeing greater
        /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        public class DuplicateKeyComparer<TKey>
            :
                IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1; // Handle equality as being greater. Note: this will break Remove(key) or
                else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                    return result;
            }

            #endregion
        }

        public static bool PathFinder5(string maze)
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

            SortedList<int, Tile> openList = new SortedList<int, Tile>(new DuplicateKeyComparer<int>()) { { start.CostDistance, start } };

            while (openList.Count > 0)
            {
                //for (int i = 1; i < openList.Count; i++)
                //{
                //    if (openList[i].CostDistance < checkTile.CostDistance)
                //    {
                //        checkTile = openList[i];
                //    }
                //}

                Tile checkTile = openList.Values[0];
                openList.RemoveAt(0);
                map[checkTile.Y][checkTile.X] = visitedCharacter;

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //It's already in the active list, but that's OK, maybe this new tile has a shorter path to it
                    bool inActiveList = false;

                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList.Values[i].X == walkableTile.X && openList.Values[i].Y == walkableTile.Y)
                        {
                            inActiveList = true;
                            break;
                        }
                    }

                    if (inActiveList)
                    {
                        int i = 0;
                        foreach (Tile tile in walkableTiles)
                        {
                            if (tile.X == walkableTile.X && tile.Y == walkableTile.Y)
                            {
                                if (tile.CostDistance > checkTile.CostDistance)
                                {
                                    openList.RemoveAt(i);
                                    openList.Add(walkableTile.CostDistance, walkableTile);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Its a new tile
                        openList.Add(walkableTile.CostDistance, walkableTile);
                    }
                }
            }

            return false;


            // ========== LOCAL FUNCTIONS ==========

            List<Tile> GetWalkableTiles(int[][] map, Tile currentTile, Tile targetTile)
            {
                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
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
                       "....W.",

                   e = ".",

                   f = "..\nW.",

                   g = "...\nW.W\n.W.",

                   k = ".W...\n.W...\n.W.W.\n...W.\n...W.",

                   l = ".W...\n.W...\n.W.W.\n...WW\n...W.";

            Assert.AreEqual(true, Finder.PathFinder(a));
            Assert.AreEqual(false, Finder.PathFinder(b));
            Assert.AreEqual(true, Finder.PathFinder(c));
            Assert.AreEqual(false, Finder.PathFinder(d));
            Assert.AreEqual(true, Finder.PathFinder(e));
            Assert.AreEqual(true, Finder.PathFinder(f));
            Assert.AreEqual(false, Finder.PathFinder(g));
            Assert.AreEqual(true, Finder.PathFinder(k));
            Assert.AreEqual(false, Finder.PathFinder(l));
        }

        [TestFixture]
        public class SolutionTest
        {

            [Test]
            public void fixedTests()
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


            [Test]
            public void moreTests()
            {

                string a =
                    ".W...\n" +
                    ".W...\n" +
                    ".W.W.\n" +
                    "...W.\n" +
                    "...W.";
                Assert.AreEqual(true, Finder.PathFinder(a));

                a = ".W...\n" +
                    ".W...\n" +
                    ".W.W.\n" +
                    "...WW\n" +
                    "...W.";
                Assert.AreEqual(false, Finder.PathFinder(a));

                a = "..W\n" +
                    ".W.\n" +
                    "W..";
                Assert.AreEqual(false, Finder.PathFinder(a));

                a = ".WWWW\n" +
                    ".W...\n" +
                    ".W.W.\n" +
                    ".W.W.\n" +
                    "...W.";
                Assert.AreEqual(true, Finder.PathFinder(a));

                a = ".W...\n" +
                    "W....\n" +
                    ".....\n" +
                    ".....\n" +
                    ".....";
                Assert.AreEqual(false, Finder.PathFinder(a));

                a = ".W\n" +
                    "W.";
                Assert.AreEqual(false, Finder.PathFinder(a));

                a = ".";
                Assert.AreEqual(true, Finder.PathFinder(a));
            }




            private static bool DEBUG = false;

            private Random rnd = new Random();

            private int rand(int n) { return rnd.Next(n); }
            private int rand(int n, int m) { return n + rnd.Next(m - n + 1); }

            [Test]
            public void randomTests()
            {

                int count = 0,
                    TIMES = 20,
                    RNG = 100;

                var sw = System.Diagnostics.Stopwatch.StartNew();
                for (int nTimes = 0; nTimes < TIMES; nTimes++)
                {
                    for (int n = 1; n < RNG + 1; n++)
                    {
                        string maze = generateMaze(n);

                        bool expected = KikicestKiTrouveLaSolution.PathFinder(maze);
                        Assert.AreEqual(expected, Finder.PathFinder(maze));

                        count += expected ? 1 : 0;
                    }
                }
                sw.Stop();

                Console.WriteLine("Elapsed: " + sw.Elapsed);

                if (DEBUG) Console.WriteLine("" + count + "/" + (RNG * TIMES));
            }


            private string generateMaze(int n)
            {

                int size = n * n,
                    nWalls = rand(size / 4, size / 3);

                ISet<int> posWalls = new HashSet<int>();
                while (posWalls.Count < nWalls)
                    posWalls.Add(rand(size));

                StringBuilder[] sbArr = Enumerable.Range(0, n)
                                        .Select(x => new StringBuilder(new string('.', n)))
                                        .ToArray();

                foreach (var w in posWalls)
                {
                    int x = w / n, y = w % n;
                    sbArr[x][y] = 'W';
                };

                sbArr[n - 1][n - 1] = '.';        // first and last tile always free
                sbArr[0][0] = '.';

                return string.Join("\n", sbArr.Select(sb => sb.ToString()));
            }

            private sealed class KikicestKiTrouveLaSolution
            {
                private class Point
                {
                    public int x, y;

                    public Point(int x, int y) { this.x = x; this.y = y; }

                    public override bool Equals(object obj)
                    {
                        if (obj == null || GetType() != obj.GetType())
                        {
                            return false;
                        }

                        Point other = (Point)obj;
                        return x == other.x && y == other.y;

                    }

                    public override int GetHashCode() { return x * 31 + y; }
                }


                readonly private static List<Point> MOVES = new List<Point>() { new Point(1, 0), new Point(0, 1), new Point(0, -1), new Point(-1, 0) };

                public static bool PathFinder(string maze)
                {

                    int S = (int)Math.Sqrt(maze.Length) - 1;
                    if (S == 0) return true;

                    ISet<Point> bag = new HashSet<Point>();
                    int x = -1;

                    foreach (string line in maze.Split('\n'))
                    {
                        x++;
                        for (int y = 0; y < line.Length; y++)
                            if (line[y] == '.') bag.Add(new Point(x, y));
                    }
                    bag.Remove(new Point(0, 0));

                    Point end = new Point(S, S);
                    bool[] hasEnd = { false };
                    ISet<Point> look = new HashSet<Point>(new List<Point>() { new Point(0, 0) });

                    while (look.Any())
                    {
                        if (hasEnd[0]) return true;
                        look = new HashSet<Point>(
                                    look.SelectMany(p => MOVES.Select(d => new Point(p.x + d.x, p.y + d.y)))
                                       .Distinct()
                                       .Where(p =>
                                       {
                                           if (p.Equals(end)) hasEnd[0] = true;
                                           if (bag.Contains(p))
                                           {
                                               bag.Remove(p);
                                               return true;
                                           }
                                           else return false;
                                       })
                                    );
                    }
                    return false;
                }
            }
        }
    }
}