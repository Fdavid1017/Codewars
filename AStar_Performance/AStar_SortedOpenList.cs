using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AStar_Performance.AStar;

namespace AStar_Performance
{
    public class AStar_SortedOpenList
    {
        class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Cost { get; set; } // Tiles traveled from to start to get to the current tile
            public int Distance { get; set; } // Distance to the destination without walls
            public int CostDistance { get; set; }

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
            }
        }

        /// <summary>
        /// Comparer for comparing two keys, handling equality as beeing greater
        /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
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

            SortedList<int, Tile> openList = new SortedList<int, Tile>(new DuplicateKeyComparer<int>()) { { start.CostDistance, start } };

            while (openList.Count > 0)
            {
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
    }
}
