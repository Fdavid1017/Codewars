using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar_Performance
{
    public class AStar_Base
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

        public static bool PathFinder(string maze)
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

            List<Tile> openList = new List<Tile>() { start };
            List<Tile> closedList = new List<Tile>();

            while (openList.Count() > 0)
            {
                Tile checkTile = openList.OrderBy(x => x.CostDistance).First();
                openList.Remove(checkTile);
                closedList.Add(checkTile);

                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // Finish reached
                    return true;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again
                    if (closedList.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a shorter path to it
                    if (openList.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = openList.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            openList.Remove(existingTile);
                            openList.Add(walkableTile);
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

            List<Tile> GetWalkableTiles(string[] map, Tile currentTile, Tile targetTile)
            {
                const char wallCharacter = 'W';

                var possibleTiles = new List<Tile>()
                    {
                        new Tile { X = currentTile.X, Y = currentTile.Y - 1, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X, Y = currentTile.Y + 1, Cost = currentTile.Cost + 1},
                        new Tile { X = currentTile.X - 1, Y = currentTile.Y, Cost = currentTile.Cost + 1 },
                        new Tile { X = currentTile.X + 1, Y = currentTile.Y, Cost = currentTile.Cost + 1 },
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

    }
}
