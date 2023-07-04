using System.Linq;
using System.Text;

namespace TrafficLightsMultipleCars
{
    // https://www.codewars.com/kata/5d230e119dd9860028167fa5

    public class Dinglemouse
    {
        public static string CAR_CHARACTER = "C";
        public static string ROAD_CHARACTER = ".";

        public class Light
        {
            private static int GREEN_TICKS = 5;
            private static int ORANGE_TICKS = 1;
            private static int RED_TICKS = 5;

            public static string GREEN_CHARACTER = "G";
            public static string ORANGE_CHARACTER = "O";
            public static string RED_CHARACTER = "R";

            public int position = 0;
            public string state = GREEN_CHARACTER;

            private string previousState = GREEN_CHARACTER;
            private int elapsedTicks = 0;

            public Light(int position, string state)
            {
                this.position = position;
                this.state = state;
                this.previousState = state;
            }

            public string AddTick()
            {
                elapsedTicks++;

                if (state == GREEN_CHARACTER && elapsedTicks >= GREEN_TICKS)
                {
                    previousState = state;
                    state = ORANGE_CHARACTER;
                    elapsedTicks = 0;
                }
                else if (state == ORANGE_CHARACTER && elapsedTicks >= ORANGE_TICKS)
                {
                    if (previousState == RED_CHARACTER)
                    {
                        state = GREEN_CHARACTER;
                    }
                    else if (previousState == GREEN_CHARACTER)
                    {
                        state = RED_CHARACTER;
                    }

                    previousState = ORANGE_CHARACTER;
                    elapsedTicks = 0;
                }
                else if (state == RED_CHARACTER && elapsedTicks >= RED_TICKS)
                {
                    previousState = state;
                    state = GREEN_CHARACTER;
                    elapsedTicks = 0;
                }

                return state;
            }
        }

        public static string[] TrafficLights(string road, int n)
        {
            string[] results = new string[n + 1];
            results[0] = road;

            Console.WriteLine("Initial state:");
            Console.WriteLine(road);
            Console.WriteLine();

            List<int> carsPosition = new List<int>();

            // Lights position and state
            List<Light> lights = new List<Light>();

            // Set values
            for (int i = 0; i < road.Length; i++)
            {
                string currentCharacter = road[i].ToString();
                if (currentCharacter == CAR_CHARACTER)
                {
                    carsPosition.Add(i);
                }
                else if (currentCharacter == Light.GREEN_CHARACTER)
                {
                    lights.Add(new Light(i, Light.GREEN_CHARACTER));
                }
                else if (currentCharacter == Light.ORANGE_CHARACTER)
                {
                    lights.Add(new Light(i, Light.ORANGE_CHARACTER));
                }
                else if (currentCharacter == Light.RED_CHARACTER)
                {
                    lights.Add(new Light(i, Light.RED_CHARACTER));
                }
            }

            // Run simulation

            for (int i = 1; i < n + 1; i++)
            {
                StringBuilder currentState = new StringBuilder(results[i - 1]);

                foreach (Light light in lights)
                {
                    currentState[light.position] = char.Parse(light.AddTick());
                }

                for (int j = carsPosition.Count - 1; j >= 0; j--)
                {
                    if (CanCarMove(carsPosition[j], currentState.ToString()))
                    {
                        int position = carsPosition[j];
                        string replaceCharacter = ROAD_CHARACTER;

                        Light? lightAtPosition = lights.FirstOrDefault(x => x.position == position) ?? null;
                        if (lightAtPosition != null)
                        {
                            replaceCharacter = lightAtPosition.state;
                        }

                        if (position < currentState.Length)
                        {
                            currentState[position] = char.Parse(replaceCharacter);
                        }

                        if (carsPosition[j] < currentState.Length - 1)
                        {
                            currentState[carsPosition[j] + 1] = char.Parse(CAR_CHARACTER);
                        }

                        carsPosition[j]++;

                        Light lightAtPreviousPosition = lights.FirstOrDefault(x => x.position == carsPosition[j] - 1, null);

                        if (lightAtPreviousPosition != null)
                        {
                            currentState[carsPosition[j] - 1] = char.Parse(lightAtPreviousPosition.state);
                        }
                    }
                }

                Console.WriteLine(currentState);

                results[i] = currentState.ToString();
            }

            return results;
        }

        public static bool CanCarMove(int carPosition, string road)
        {
            // before road end
            if (carPosition < road.Length - 1)
            {
                // next is road
                if (road[carPosition + 1].ToString() == ROAD_CHARACTER)
                {
                    return true;
                }

                // next is car or red/orange light
                if (
                    road[carPosition + 1].ToString() == CAR_CHARACTER
                    || road[carPosition + 1].ToString() == Light.RED_CHARACTER
                    || road[carPosition + 1].ToString() == Light.ORANGE_CHARACTER
                    )
                {
                    return false;
                }

                // next is green
                if (road[carPosition + 1].ToString() == Light.GREEN_CHARACTER)
                {
                    if (carPosition + 2 >= road.Length)
                    {
                        return true;
                    }

                    return carPosition + 2 < road.Length && road[carPosition + 2].ToString() == ROAD_CHARACTER;
                }
                //return road[carPosition + 1].ToString() == ROAD_CHARACTER ||
                //       (
                //           road[carPosition + 1].ToString() == Light.GREEN_CHARACTER
                //           && road[carPosition + 2].ToString() != CAR_CHARACTER
                //       );
            }

            // end of the road
            return true;
        }

        private static void DoTest(string initial, string[] expected, int n)
        {
            var got = Dinglemouse.TrafficLights(initial, n);
            Console.WriteLine($"Expected: ");
            for (int i = 0; i < expected.Length; i++)
                Console.WriteLine($"{i,3} {expected[i]}");
            Console.WriteLine("\nYour result:");
            for (int i = 0; i < got.Length; i++)
                Console.WriteLine($"{i,3} {got[i]}");
            Assert.AreEqual(expected, got);
        }

        [Test]
        public void Example()
        {
            int n = 16;
            string[] sim =
            {
                "CCC.G...R...", // 0 initial state as passed
                ".CCCG...R...", // 1
                "..CCC...R...", // 2 show 1st car, not the green light
                "..CCGC..R...", // 3 2nd car cannot enter intersection because 1st car blocks the exit
                "...CC.C.R...", // 4 show 2nd car, not the green light
                "...COC.CG...", // 5 3rd car stops for the orange light
                "...CR.C.C...", // 6
                "...CR..CGC..", // 7
                "...CR...C.C.", // 8
                "...CR...GC.C", // 9
                "...CR...O.C.", // 10
                "....C...R..C", // 11 3rd car can proceed
                "....GC..R...", // 12
                "....G.C.R...", // 13
                "....G..CR...", // 14
                "....G..CR...", // 15
                "....O...C..."  // 16
            };
            DoTest(sim[0], sim, n);
        }

        [Test]
        public void CarFallOff()
        {
            int n = 6;
            string[] sim =
            {
                "CCC.G",//0
                "..CCC",//1
                "...CC",//2
                "....C",//3
                "....G",//4
                "....O",//5 -- O
                "....R",//6 -- R
            };
            DoTest(sim[0], sim, n);
        }
    }
}