using System.Numerics;
using NUnit.Framework.Constraints;

// https://www.codewars.com/kata/52b7ed099cdc285c300001cd

namespace SumOfIntervals
{
    using Interval = System.ValueTuple<int, int>;

    public class Intervals
    {
        public static int SumIntervals((int, int)[] intervals)
        {
            List<Interval> mergedIntervals = intervals.Select(x => new Interval(x.Item1, x.Item2)).ToList();

            bool changed = false;
            do
            {
                changed = false;
                for (int i = 0; i < mergedIntervals.Count - 1; i++)
                {
                    var firstItem = mergedIntervals[i];
                    for (int j = i + 1; j < mergedIntervals.Count; j++)
                    {
                        var secondItem = mergedIntervals[j];

                        if (firstItem.Item1 > secondItem.Item1 && firstItem.Item1 <= secondItem.Item2)
                        {
                            // A másik intervallum között van az 1. elem
                            firstItem.Item1 = secondItem.Item1;
                            firstItem.Item2 = firstItem.Item2 > secondItem.Item2 ? firstItem.Item2 : secondItem.Item2;

                            mergedIntervals.RemoveAt(j);
                            j--;
                            changed = true;
                        }
                        else if (firstItem.Item2 >= secondItem.Item1 && firstItem.Item2 < secondItem.Item2)
                        {
                            // A másik intervallum között van a 2. elem
                            firstItem.Item1 = firstItem.Item1 < secondItem.Item1 ? firstItem.Item1 : secondItem.Item1;
                            firstItem.Item2 = firstItem.Item2 > secondItem.Item2 ? firstItem.Item2 : secondItem.Item2;

                            mergedIntervals.RemoveAt(j);
                            j--;
                            changed = true;
                        }
                        else if (
                            // A 2. elem az 1. belül van
                            firstItem.Item1 <= secondItem.Item1 && firstItem.Item1 < secondItem.Item2
                            && firstItem.Item2 > secondItem.Item1 && firstItem.Item2 >= secondItem.Item2
                           )
                        {
                            mergedIntervals.RemoveAt(j);
                            j--;
                            changed = true;
                        }
                    }

                    mergedIntervals[i] = firstItem;
                }
            } while (changed);

            int sum = 0;
            mergedIntervals.ForEach(x =>
            {
                sum += x.Item2 - x.Item1;
            });

            return sum;
        }



        [Test]
        public void ShouldHandleEmptyIntervals()
        {
            Assert.AreEqual(0, Intervals.SumIntervals(new Interval[] { }));
            Assert.AreEqual(0, Intervals.SumIntervals(new Interval[] { (4, 4), (6, 6), (8, 8) }));
        }

        [Test]
        public void ShouldAddDisjoinedIntervals()
        {
            Assert.AreEqual(9, Intervals.SumIntervals(new Interval[] { (1, 2), (6, 10), (11, 15) }));
            Assert.AreEqual(11, Intervals.SumIntervals(new Interval[] { (4, 8), (9, 10), (15, 21) }));
            Assert.AreEqual(7, Intervals.SumIntervals(new Interval[] { (-1, 4), (-5, -3) }));
            Assert.AreEqual(78, Intervals.SumIntervals(new Interval[] { (-245, -218), (-194, -179), (-155, -119) }));
        }

        [Test]
        public void ShouldAddAdjacentIntervals()
        {
            Assert.AreEqual(54, Intervals.SumIntervals(new Interval[] { (1, 2), (2, 6), (6, 55) }));
            Assert.AreEqual(23, Intervals.SumIntervals(new Interval[] { (-2, -1), (-1, 0), (0, 21) }));
        }

        [Test]
        public void ShouldAddOverlappingIntervals()
        {
            Assert.AreEqual(7, Intervals.SumIntervals(new Interval[] { (1, 4), (7, 10), (3, 5) }));
            Assert.AreEqual(6, Intervals.SumIntervals(new Interval[] { (5, 8), (3, 6), (1, 2) }));
            Assert.AreEqual(19, Intervals.SumIntervals(new Interval[] { (1, 5), (10, 20), (1, 6), (16, 19), (5, 11) }));
        }

        [Test]
        public void ShouldHandleMixedIntervals()
        {
            Assert.AreEqual(13, Intervals.SumIntervals(new Interval[] { (2, 5), (-1, 2), (-40, -35), (6, 8) }));
            Assert.AreEqual(1234, Intervals.SumIntervals(new Interval[] { (-7, 8), (-2, 10), (5, 15), (2000, 3150), (-5400, -5338) }));
            Assert.AreEqual(158, Intervals.SumIntervals(new Interval[] { (-101, 24), (-35, 27), (27, 53), (-105, 20), (-36, 26) }));
        }

        [Test]
        public void ShouldHandleLargeIntervals()
        {
            Assert.AreEqual(2_000_000_000, Intervals.SumIntervals(new Interval[] { (-1_000_000_000, 1_000_000_000) }));
            Assert.AreEqual(100_000_030, Intervals.SumIntervals(new Interval[] { (0, 20), (-100_000_000, 10), (30, 40) }));
        }
    }
}