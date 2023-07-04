using System.Text;

namespace TrafficLightsOneCar
{
    // https://www.codewars.com/kata/5d0ae91acac0a50232e8a547
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

                for (int j = 0; j < carsPosition.Count; j++)
                {
                    if (CanCarMove(carsPosition[j], currentState.ToString()))
                    {
                        currentState[carsPosition[j]] = char.Parse(ROAD_CHARACTER);

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
            if (carPosition < road.Length - 1)
            {
                return road[carPosition + 1].ToString() == ROAD_CHARACTER ||
                       road[carPosition + 1].ToString() == Light.GREEN_CHARACTER;
            }

            return carPosition == road.Length - 1;
        }

        private static void DoTest(string initial, string[] expected, int n)
        {
            var got = Dinglemouse.TrafficLights(initial, n);
            Console.WriteLine();
            Console.WriteLine();
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
            int n = 10;
            string[] sim =
            {
                "C...R............G......",  // 0
                ".C..R............G......",  // 1
                "..C.R............G......",  // 2
                "...CR............G......",  // 3
                "...CR............G......",  // 4
                "....C............O......",  // 5
                "....GC...........R......",  // 6
                "....G.C..........R......",  // 7
                "....G..C.........R......",  // 8
                "....G...C........R......",  // 9
                "....O....C.......R......"   // 10
            };
            DoTest(sim[0], sim, n);
        }

        [Test]
        public void FallOf()
        {
            int n = 7;
            string[] sim =
            {
                "CG...",  // 0
                ".C...",  // 1
                ".GC..",  // 2
                ".G.C.",  // 3
                ".O..C",  // 4
                ".R...",  // 5
                ".R...",  // 6
            };
            DoTest(sim[0], sim, n);
        }
    }
}