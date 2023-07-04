using NUnit.Framework.Constraints;

namespace TheLift
{
    // https://www.codewars.com/kata/58905bfa1decb981da00009e

    public class Dinglemouse
    {
        private static List<int> peopleInLift = new List<int>();
        private static int liftCapacity = 0;
        private static int currentFloor = 0;

        public static int[] TheLift(int[][] queues, int capacity)
        {
            liftCapacity = capacity;
            List<int> stoppedFloors = new List<int>() { 0 };

            // elsõ beszálás
            ManagePeopleOnFloor(queues, true);

            bool liftGoingUp = true;
            do
            {
                // Lift a következõ szintre megy és kiszálnak
                bool peopleLeft = MoveLift(liftGoingUp, queues.Length);

                // Új irány ellenõrzése
                if (liftGoingUp)
                {
                    // elõzõleg felfele ment

                    bool peopleInGoingUp = peopleInLift.Count(x => x - currentFloor > 0) > 0;
                    bool peopleCalledUpwards = queues.Skip(currentFloor) // Csak a fentebbi szinteket ellenõrzi
                        .Select((waiting, floor) =>
                            waiting.Count(x => x - floor > 0) > 0) // Felfele akar menni
                        .Count() > 0;

                    liftGoingUp = peopleInGoingUp || peopleCalledUpwards;
                }
                else
                {
                    // elõzõleg lefele ment

                    bool peopleInGoingDown = peopleInLift.Count(x => x - currentFloor < 0) > 0;
                    bool peopleCalledUpwards = queues.Take(currentFloor) // Csak a lentebbi szinteket ellenõrzi
                        .Select((waiting, floor) =>
                            waiting.Count(x => x - floor < 0) > 0) // Lefele akar menni
                        .Count() > 0;

                    liftGoingUp = !peopleInGoingDown || !peopleCalledUpwards;
                }

                // Emberek beszálnak
                bool peopleGotIn = ManagePeopleOnFloor(queues, liftGoingUp);

                if (peopleGotIn || peopleLeft)
                {
                    stoppedFloors.Add(currentFloor);
                }
            } while (PeopleAreWaiting(queues));

            stoppedFloors.Add(0);
            return stoppedFloors.ToArray();
        }

        public static bool PeopleAreWaiting(int[][] queue)
        {
            return queue.Where(x => x.Length > 0).Count() > 0 || peopleInLift.Count > 0;
        }

        public static bool MoveLift(bool up, int maxFloors)
        {
            if (up && currentFloor < maxFloors - 1)
            {
                currentFloor++;
            }
            else if (currentFloor > 0)
            {
                currentFloor--;
            }

            return peopleInLift.RemoveAll(x => x == currentFloor) > 0;
        }

        public static bool ManagePeopleOnFloor(int[][] queues, bool goingUp)
        {
            List<int> canGetIn = queues[currentFloor].Where(x =>
            {
                if (goingUp)
                {
                    return x - currentFloor > 0;
                }
                else
                {
                    return x - currentFloor < 0;
                }
            }).ToList();

            int availableSpace = liftCapacity - peopleInLift.Count;
            int[] getIn = canGetIn.Take(availableSpace).ToArray();
            peopleInLift.AddRange(getIn);
            queues[currentFloor] = queues[currentFloor].Except(getIn).ToArray();

            return getIn.Length > 0;
        }

        public static bool LiftGoesUp(int[][] queues, List<int> peopleInLift, int currentFloor)
        {
            return
                queues.Count(floor => floor.Count(x => x - currentFloor > 0) > 0) > 0 // felfele hívták
                || peopleInLift.Count(x => x - currentFloor > 0) > 0; // liftben van aki még felfele megy;
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
    }
}