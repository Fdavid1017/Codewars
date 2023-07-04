namespace PathFinder1
{
    //https://www.codewars.com/kata/5765870e190b1472ec0022a2
    //https://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
    public class Finder
    {
        public class Location
        {
            public int x;
            public int y;
            public int fCost;
            public int gCost;
            public int hCost;
            //public Location parent;

            public static int ComputeHScore(int x, int y, int targetX, int targetY)
            {
                return Math.Abs(targetX - x) + Math.Abs(targetY - y);
            }
        }

        public static bool PathFinder(string maze)
        {
            if (maze.Length == 1)
            {
                return true;
            }

            if (!maze.Contains("W"))
            {
                return true;
            }

            string[] map = maze.Split("\n");
            int mapSize = map.Length;
            int mapSize2 = map[0].Length;

            Location current = null;
            Location start = new Location { x = 0, y = 0 };
            Location target = new Location { x = mapSize - 1, y = mapSize2 - 1 };
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;

            openList.Add(start);

            while (openList.Count > 0)
            {
                //var lowest = openList.Min(l => l.fCost);
                //current = openList.First(l => l.fCost == lowest);

                int lowest = openList[0].fCost;
                current = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < lowest)
                    {
                        lowest = openList[i].fCost;
                        current = openList[i];
                    }
                }

                closedList.Add(current);
                openList.Remove(current);

                if (current.x == target.x && current.y == target.y)
                {
                    return true;
                }


                //for (int i = 0; i < closedList.Count; i++)
                //{
                //    if (closedList[i].x == target.x && closedList[i].y == target.y)
                //    {
                //        return true;
                //    }
                //}

                // Location finish = closedList.FirstOrDefault(l => l.x == target.x && l.y == target.y);
                // if (finish != null)
                // {
                //     return true;
                //finish.parent = current;
                // current = finish;
                //break;
                // }

                var adjacentSquares = GetWalkableAdjacentSquares(current.x, current.y, map);
                g++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    Location adLocation = null;
                    for (int i = 0; i < closedList.Count; i++)
                    {
                        if (closedList[i].x == adjacentSquare.x && closedList[i].y == adjacentSquare.y)
                        {
                            adLocation = closedList[i];
                            break;
                        }
                    }

                    if (adLocation != null)
                    {
                        continue;
                    }

                    Location openLocation = null;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].x == adjacentSquare.x && openList[i].y == adjacentSquare.y)
                        {
                            openLocation = openList[i];
                            break;
                        }
                    }

                    if (openLocation == null)
                    {
                        adjacentSquare.gCost = g;
                        adjacentSquare.hCost = Location.ComputeHScore(adjacentSquare.x, adjacentSquare.y, target.x, target.y);
                        adjacentSquare.fCost = adjacentSquare.gCost + adjacentSquare.hCost;
                        //adjacentSquare.parent = current;

                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        if (g + adjacentSquare.hCost < adjacentSquare.fCost)
                        {
                            adjacentSquare.gCost = g;
                            adjacentSquare.fCost = adjacentSquare.gCost + adjacentSquare.hCost;
                            //adjacentSquare.parent = current;
                        }
                    }
                }
            }

            return false;
            //return current.x == target.x && current.y == target.y;
        }

        public static List<Location> GetWalkableAdjacentSquares(int x, int y, string[] map, List<Location> closedLocations)
        {
            var proposedLocations = new List<Location>();

            if (y - 1 >= 0)
            {
                proposedLocations.Add(new Location { x = x, y = y - 1 });
            }
            if (y + 1 < map[0].Length)
            {
                proposedLocations.Add(new Location { x = x, y = y + 1 });
            }
            if (x - 1 >= 0)
            {
                proposedLocations.Add(new Location { x = x - 1, y = y });
            }
            if (x + 1 < map.Length)
            {
                proposedLocations.Add(new Location { x = x + 1, y = y });
            }

            var locs = new List<Location>();
            for (int i = 0; i < proposedLocations.Count; i++)
            {
                if (map[proposedLocations[i].x][proposedLocations[i].y] == '.')
                {
                    locs.Add(proposedLocations[i]);
                }
            }

            return locs;
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