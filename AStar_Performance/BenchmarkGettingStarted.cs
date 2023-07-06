using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AStar_Performance.ConvertTest;

namespace AStar_Performance
{
    [RankColumn]
    [MemoryDiagnoser]
    public class Md5VsSha256
    {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly SHA256 sha256 = SHA256.Create();
        private readonly MD5 md5 = MD5.Create();

        public Md5VsSha256()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark(Baseline = true)]
        public byte[] Sha256() => sha256.ComputeHash(data);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);
    }

    [RankColumn]
    [MemoryDiagnoser]
    public class OtherTest
    {
        [Benchmark(Baseline = true)]
        [Arguments(1000000)]
        public void ListTest(int length)
        {
            List<int> t = new List<int>();
            for (int i = 0; i < length; i++)
            {
                t.Add(i);
            }

            for (int i = 0; i < length; i++)
            {
                t[i] = i + 1;
            }
        }

        [Benchmark]
        [Arguments(1000000)]
        public void ArrayTest(int length)
        {
            int[] t = new int[length];
            for (int i = 0; i < length; i++)
            {
                t[i] = i;
            }

            for (int i = 0; i < length; i++)
            {
                t[i] = i + 1;
            }
        }
    }


    [RankColumn]
    [MemoryDiagnoser]
    public class ConvertTest
    {
        public class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsWall { get; set; }
            public bool IsVisited { get; set; }
        }

        string mapString = ".WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.\n.WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW..WW.W.WWW.";
        int iterations = 10000;
        Random random = new Random();

        public int GetPosition(int max)
        {
            return (int)random.Next(0, max);
        }

        [Benchmark(Baseline = true)]
        public void Default()
        {
            string[] map = mapString.Split("\n");
            int mapSize = map.Length;


            // Search
            for (int i = 0; i < iterations; i++)
            {
                int x = GetPosition(mapSize);
                int y = GetPosition(map[x].Length);
                bool t = map[x][y] != 1;

                StringBuilder sb = new StringBuilder(map[x]);
                sb[y] = 'C';
                map[x] = sb.ToString();
            }
        }

        [Benchmark]
        public void ToIntArray()
        {
            string[] rows = mapString.Split("\n");

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

            // Search
            for (int i = 0; i < iterations; i++)
            {
                int x = GetPosition(mapSize);
                int y = GetPosition(map[x].Length);
                bool t = map[x][y] != 1;

                map[x][y] = -1;
            }
        }

        [Benchmark]
        public void ToTileArray()
        {
            string[] rows = mapString.Split("\n");

            Tile[][] map = new Tile[rows.Length][];
            int mapSize = map.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                Tile[] rowItems = new Tile[rows.Length];
                for (int j = 0; j < rows[i].Length; j++)
                {
                    rowItems[j] = new Tile
                    {
                        IsWall = rows[i][j] == 'W',
                        X = i,
                        Y = j
                    };
                }
                map[i] = rowItems;
            }

            // Search
            for (int i = 0; i < iterations; i++)
            {
                int x = GetPosition(mapSize);
                int y = GetPosition(map[x].Length - 1);
                bool t = map[x][y].IsWall;

                map[x][y].IsVisited = true;
            }
        }
    }


    [RankColumn]
    [MemoryDiagnoser]
    public class HashSetTest
    {
        class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Cost { get; set; } // Tiles traveled from to start to get to the current tile
            public int Distance { get; set; } // Distance to the destination without walls
            public int CostDistance { get; set; }
            public Tile Parent { get; set; }

            public override bool Equals(object? obj)
            {
                return obj is Tile tile &&
                       X == tile.X &&
                       Y == tile.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public void SetDistance(Tile target)
            {
                this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
            }


        }

        int iterations = 1000;
        int searchIterations = 500;
        Random rnd = new Random();

        public int GetCostDisctance()
        {
            return rnd.Next(0, 100);
        }

        [Benchmark(Baseline = true)]
        public void ListTest()
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < iterations; i++)
            {
                tiles.Add(new Tile() { X = i, Y = i, CostDistance = GetCostDisctance() });
            }

            //Contains
            for (int i = 0; i < searchIterations; i++)
            {
                int loc = rnd.Next(tiles.Count);
                bool contains = tiles.Contains(new Tile() { X = loc, Y = loc });
            }

            //Search
            for (int i = 0; i < searchIterations; i++)
            {
                Tile checkTile = tiles[0];
                for (int j = 1; j < tiles.Count; j++)
                {
                    if (tiles[j].CostDistance < checkTile.CostDistance)
                    {
                        checkTile = tiles[j];
                    }
                }
            }
        }

        [Benchmark]
        public void HashTest()
        {
            HashSet<Tile> tiles = new HashSet<Tile>();

            for (int i = 0; i < iterations; i++)
            {
                tiles.Add(new Tile() { X = i, Y = i });
            }

            //Contains
            for (int i = 0; i < searchIterations; i++)
            {
                int loc = rnd.Next(tiles.Count);
                bool contains = tiles.Contains(new Tile() { X = loc, Y = loc });
            }

            //Search
            for (int i = 0; i < searchIterations; i++)
            {
                Tile checkTile = tiles.OrderBy(x => x.CostDistance).First();
            }
        }

        public class TileQueueComparer : System.Collections.Generic.IComparer<Tile>
        {
            int IComparer<Tile>.Compare(Tile? x, Tile? y)
            {
                if (x.CostDistance == y.CostDistance)
                    return 0;
                else if (x.CostDistance > y.CostDistance)
                    return -1;
                else
                    return 1;
            }
        }
    }
}
