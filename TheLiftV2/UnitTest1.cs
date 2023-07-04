using NUnit.Framework.Constraints;

namespace TheLiftV2
{
    // https://www.codewars.com/kata/58905bfa1decb981da00009e

    public class Dinglemouse
    {
        public static void PrintQueues(int[][] queue)
        {
            foreach (var q in queue)
            {
                if (q.Length == 0)
                    Console.WriteLine("-");
                else
                    Console.WriteLine(string.Join(", ", q));
            }
        }

        public static int[] TheLift(int[][] queues, int capacity)
        {
            PrintQueues(queues);

            List<int> stoppedFloors = new List<int>();
            List<int> peopleInLift = new List<int>();
            bool goingUp = true;
            int currentFloor = 0;
            int iterations = 0;

            do
            {
                bool gotIn = ManagePeopleOnFloor(currentFloor);
                bool gotOut = ManagePeopleInLift();

                if (gotIn || iterations == 0 || gotOut)
                {
                    stoppedFloors.Add(currentFloor);
                }

                MoveLift();

                iterations++;
            } while (peopleInLift.Count > 0 || PeopleAreWaiting());


            // Go to floor if not already stopped there
            if (stoppedFloors.Last() != 0)
            {
                stoppedFloors.Add(0);
            }

            return stoppedFloors.ToArray();

            bool PeopleAreWaiting()
            {
                return queues.Count(x => x.Length > 0) > 0;
            }

            bool ManagePeopleOnFloor(int floorNumber)
            {
                int[] peopleOnFloor = queues[floorNumber];
                int availableSpaceInLift = capacity - peopleInLift.Count;
                bool goesUpNext = LiftGoesUpNext();
                int[] peopleToGetIn = peopleOnFloor.Where(x => goesUpNext ? x - floorNumber > 0 : x - floorNumber < 0).Take(availableSpaceInLift).ToArray();
                peopleInLift.AddRange(peopleToGetIn);
                queues[floorNumber] = queues[floorNumber].Except(peopleToGetIn).ToArray();

                return peopleToGetIn.Length > 0;
            }

            bool ManagePeopleInLift()
            {
                return peopleInLift.RemoveAll(x => x == currentFloor) > 0;
            }

            void MoveLift()
            {
                currentFloor += LiftGoesUpNext() ? 1 : -1;
            }

            bool LiftGoesUpNext()
            {
                int nextFloor = goingUp ? currentFloor + 1 : currentFloor - 1;
                bool goesUpBasedOnFloor = !(nextFloor > queues.Length - 1) || nextFloor < 0;
                bool wantsToGoUpFromCurrentFloor = queues[currentFloor].Count(x => x - currentFloor > 0) > 0;
                bool peopleUpwardsWaiting = queues.Skip(currentFloor + 1).Count(x => x.Length > 0) > 0;
                bool goesUpBasedOnPeopleInside =
                    peopleInLift.Count(x => goesUpBasedOnFloor ? x - currentFloor > 0 : x - currentFloor < 0) > 0;

                return goesUpBasedOnPeopleInside || peopleUpwardsWaiting || wantsToGoUpFromCurrentFloor;
            }
        }

        [Test]
        public void Custom()
        {
            int[][] queues =
            {
                new int[]{1,2,2,3}, // G
                new int[0], // 1
                new int[]{1,4,5,0,6}, // 2
                new int[0], // 3
                new int[0], // 4
                new int[0], // 5
                new int[0], // 6
            };
            var result = Dinglemouse.TheLift(queues, 2);
            Assert.AreEqual(new[] { 0, 2, 5, 0 }, result);
        }

        [Test]
        public void TestUp()
        {
            int[][] queues =
            {
                new int[0], // G
                new int[0], // 1
                new int[]{5,5,5}, // 2
                new int[0], // 3
                new int[0], // 4
                new int[0], // 5
                new int[0], // 6
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0, 2, 5, 0 }, result);
        }

        [Test]
        public void TestDown()
        {
            int[][] queues =
            {
                new int[0], // G
                new int[0], // 1
                new int[]{1,1}, // 2
                new int[0], // 3
                new int[0], // 4
                new int[0], // 5
                new int[0], // 6
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0, 2, 1, 0 }, result);
        }

        [Test]
        public void TestUpAndUp()
        {
            int[][] queues =
            {
                new int[0], // G
                new int[]{3}, // 1
                new int[]{4}, // 2
                new int[0], // 3
                new int[]{5}, // 4
                new int[0], // 5
                new int[0], // 6
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 0 }, result);
        }

        [Test]
        public void TestDownAndDown()
        {
            int[][] queues =
            {
                new int[0], // G
                new int[]{0}, // 1
                new int[0], // 2
                new int[0], // 3
                new int[]{2}, // 4
                new int[]{3}, // 5
                new int[0], // 6
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0, 5, 4, 3, 2, 1, 0 }, result);
        }

        [Test]
        public void TestEmpty()
        {
            int[][] queues =
            {
                new int[0],
                new int[0],
                new int[0],
                new int[0],
                new int[0],
                new int[0],
                new int[0],
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0 }, result);
        }

        [Test]
        public void TestEnterOnGroundFloor()
        {
            int[][] queues =
            {
                new int[]{1,2,3,4},
                new int[0],
                new int[0],
                new int[0],
                new int[0],
                new int[0],
                new int[0],
            };
            var result = Dinglemouse.TheLift(queues, 5);
            Assert.AreEqual(new[] { 0, 1, 2, 3, 4 }, result);
        }
    }
}