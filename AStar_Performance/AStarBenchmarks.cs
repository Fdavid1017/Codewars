using BenchmarkDotNet.Attributes;
using System.Text;

namespace AStar_Performance
{
    [RankColumn]
    [MemoryDiagnoser]
    public class AStar
    {
        string maze = ".......WWW\n....W..WW.\n..W..W..W.\n..W.W.....\n......W.W.\n...WW.....\nW.........\nW...W.W..W\nWWW.......\nW....WW...";

        public AStar()
        {
            // this.maze = GenerateMap();
        }

        public string GenerateMap()
        {
            int n = 10;
            Random rnd = new Random();

            int size = n * n,
                   nWalls = rnd.Next(size / 4, size / 3);

            ISet<int> posWalls = new HashSet<int>();
            while (posWalls.Count < nWalls)
                posWalls.Add(rnd.Next(size));

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

        [Benchmark(Baseline = true)]
        public bool Base()
        {
            return AStar_Base.PathFinder(this.maze);
        }

        [Benchmark]
        public bool WithoutLinq()
        {
            return AStar_WithoutLinq.PathFinder(this.maze);
        }

        [Benchmark]
        public bool ClosedListAndMapConvert()
        {
            return AStar_ClosedListAndMapConvert.PathFinder(this.maze);
        }

        [Benchmark]
        public bool Map2dArray()
        {
            return AStar_2dArray.PathFinder(this.maze);
        }

        [Benchmark]
        public bool HashSet()
        {
            return AStar_HashSet.PathFinder(this.maze);
        }

        [Benchmark]
        public bool MinorChanges()
        {
            return AStar_HashSet.MinorChanges(this.maze);
        }

        [Benchmark]
        public bool SortedOpenList()
        {
            return AStar_SortedOpenList.PathFinder(this.maze);
        }

        [Benchmark]
        public bool PriorityQueue()
        {
            return AStar_PriorityQueue.PathFinder(this.maze);
        }
    }
}
