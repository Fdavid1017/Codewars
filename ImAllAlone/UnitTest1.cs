namespace ImAllAlone
{
    // https://www.codewars.com/kata/5c230f017f74a2e1c300004f

    public class Dinglemouse
    {
        public static bool AllAlone(char[][] house)
        {
            // # walls
            // X POTUS
            // o elves

            Location potus = new Location();
            List<Location> elves = new List<Location>();

            for (int i = 0; i < house.Length; i++)
            {
                for (int j = 0; j < house[i].Length; j++)
                {
                    if (house[i][j].Equals('X'))
                    {
                        potus.x = i;
                        potus.y = j;
                    }

                    if (house[i][j].Equals('o'))
                    {
                        elves.Add(new Location() { x = i, y = j });
                    }
                }
            }

            TestContext.Out.WriteLine(string.Format("Potus.X: {0}, Potus.Y: {1}, Elves count: {2}", potus.x, potus.y, elves.Count));

            foreach (Location elfLocation in elves)
            {
                if (PathFinder(house, potus, elfLocation))
                {
                    return false;
                }
            }

            return true;
        }

        public class Location
        {
            public int x;
            public int y;
            public int fCost;
            public int gCost;
            public int hCost;

            public static int ComputeHScore(int x, int y, int targetX, int targetY)
            {
                return Math.Abs(targetX - x) + Math.Abs(targetY - y);
            }
        }

        public static bool PathFinder(char[][] map, Location start, Location target)
        {
            int mapSize = map.Length;

            Location current = null;
            var openList = new List<Location>();
            var closedList = new List<Location>();
            int g = 0;

            openList.Add(start);

            while (openList.Count > 0)
            {
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

                for (int i = 0; i < closedList.Count; i++)
                {
                    if (closedList[i].x == target.x && closedList[i].y == target.y)
                    {
                        return true;
                    }
                }

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

                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        if (g + adjacentSquare.hCost < adjacentSquare.fCost)
                        {
                            adjacentSquare.gCost = g;
                            adjacentSquare.fCost = adjacentSquare.gCost + adjacentSquare.hCost;
                        }
                    }
                }
            }

            return false;
        }

        public static List<Location> GetWalkableAdjacentSquares(int x, int y, char[][] map)
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
                if (map[proposedLocations[i].x][proposedLocations[i].y] != '#')
                {
                    locs.Add(proposedLocations[i]);
                }
            }

            return locs;
        }

        [Test]
        public void Ex1()
        {
            char[][] house =
            {
                "  o                o        #######".ToCharArray(),
                "###############             #     #".ToCharArray(),
                "#             #        o    #     #".ToCharArray(),
                "#  X          ###############     #".ToCharArray(),
                "#                                 #".ToCharArray(),
                "###################################".ToCharArray()
            };
            Assert.AreEqual(true, Dinglemouse.AllAlone(house));
        }

        [Test]
        public void Ex2()
        {
            char[][] house =
            {
                "#################             ".ToCharArray(),
                "#     o         #   o         ".ToCharArray(),
                "#          ######        o    ".ToCharArray(),
                "####       #                  ".ToCharArray(),
                "   #       ###################".ToCharArray(),
                "   #                         #".ToCharArray(),
                "   #                  X      #".ToCharArray(),
                "   ###########################".ToCharArray()
            };
            Assert.AreEqual(false, Dinglemouse.AllAlone(house));
        }
    }
}