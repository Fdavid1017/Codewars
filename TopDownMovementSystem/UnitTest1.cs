using System.Collections.Generic;
using System.Threading.Channels;

namespace TopDownMovementSystem
{
    // https://www.codewars.com/kata/59315ad28f0ebeebee000159

    public class TopDownMovement
    {
        //public enum Direction { Up = 8, Down = 2, Left = 4, Right = 6 }
        public enum Direction { Up = 1, Down = 2, Left = 3, Right = 4 }

        public struct Tile
        {
            public int X { get; }
            public int Y { get; }

            public Tile(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        public class PlayerMovement
        {
            public Tile Position { get; private set; }
            public Direction Direction { get; private set; }

            private List<Direction> pressedDirections = new List<Direction>();

            private Dictionary<Direction, bool> previousFrameStates = new Dictionary<Direction, bool>()
            {
                { Direction.Up, false },
                { Direction.Right, false },
                { Direction.Down, false },
                { Direction.Left, false },
            };

            public PlayerMovement(int x, int y)
            {
                this.Position = new Tile(x, y);
            }

            public void Update()
            {
                Dictionary<Direction, bool> pressedStateThisFrame = new Dictionary<Direction, bool>()
                {
                    { Direction.Up, Input.GetState(Direction.Up) },
                    { Direction.Right, Input.GetState(Direction.Right) },
                    { Direction.Down, Input.GetState(Direction.Down) },
                    { Direction.Left, Input.GetState(Direction.Left) },
                };

                Console.WriteLine("Pressed states this frame: ");
                foreach (var direction in pressedStateThisFrame)
                {
                    Console.WriteLine($"{direction.Key} - {direction.Value}");
                }

                // Keys pressed in this frame
                List<Direction> pressedInFrameDirections = pressedStateThisFrame
                    .Where(x => previousFrameStates[x.Key] != x.Value && x.Value)
                    .Select(x => x.Key)
                    .ToList();

                Console.WriteLine("\nPressed in this frame: ");
                foreach (var direction in pressedInFrameDirections)
                {
                    Console.WriteLine($"{direction}");
                }

                // Keys released in this frame
                List<Direction> releasedInFrameDirections = pressedStateThisFrame
                    .Where(x => previousFrameStates[x.Key] != x.Value && !x.Value)
                    .Select(x => x.Key)
                    .ToList();

                Console.WriteLine("\nReleased in this frame: ");
                foreach (var direction in releasedInFrameDirections)
                {
                    Console.WriteLine($"{direction}");
                }

                releasedInFrameDirections.ForEach(released =>
                {
                    pressedDirections.RemoveAll(pressed => pressed == released);
                });

                pressedInFrameDirections = pressedInFrameDirections.OrderBy(x =>
                {
                    switch (x)
                    {
                        case Direction.Up:
                            return 1;
                        case Direction.Down:
                            return 2;
                        case Direction.Left:
                            return 3;
                        case Direction.Right:
                            return 4;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(x), x, null);
                    }
                }).ToList();

                Console.WriteLine("\nPressed in frame ordered: ");
                foreach (var direction in pressedInFrameDirections)
                {
                    Console.WriteLine($"{direction}");
                }

                pressedDirections.InsertRange(0, pressedInFrameDirections);
                previousFrameStates = pressedStateThisFrame;

                Console.WriteLine("\nPressed list: ");
                foreach (var direction in pressedDirections)
                {
                    Console.WriteLine($"{direction}");
                }

                Console.WriteLine("==========");

                if (pressedDirections.Count > 0)
                    MoveCharacter(pressedDirections[0]);
            }

            private void MoveCharacter(Direction direction)
            {
                if (this.Direction == direction)
                {
                    // Move
                    // Up = { 0, +1 } , Down = { 0, -1 }, Left = { -1, 0 }, Right = { +1, 0 }
                    switch (Direction)
                    {
                        case Direction.Up:
                            Position = new Tile(Position.X, Position.Y + 1);
                            break;
                        case Direction.Down:
                            Position = new Tile(Position.X, Position.Y - 1);
                            break;
                        case Direction.Left:
                            Position = new Tile(Position.X - 1, Position.Y);
                            break;
                        case Direction.Right:
                            Position = new Tile(Position.X + 1, Position.Y);
                            break;
                    }
                }
                else
                {
                    // Rotate
                    Direction = direction;
                }
            }
        }

        public static class Input
        {
            static List<Direction> pressedDirections = new List<Direction>();
            static List<Direction> currentFrameDirections = new List<Direction>();

            // pressed = true, released = false
            public static bool GetState(Direction direction)
            {
                return pressedDirections.Contains(direction) || currentFrameDirections.Contains(direction);
            }

            public static void Clear()
            {
                pressedDirections.Clear();
            }

            public static void Release(Direction direction)
            {
                pressedDirections.Remove(direction);
                currentFrameDirections.Remove(direction);
            }

            public static void Press(Direction direction)
            {
                currentFrameDirections.Insert(0, direction);
            }

            public static Direction? GetCurrentInput()
            {
                return pressedDirections.Count > 0 ? pressedDirections[0] : null;
            }
            public static void Update()
            {
                currentFrameDirections = currentFrameDirections.OrderBy(x => x).ToList();
                pressedDirections.InsertRange(0, currentFrameDirections);
                currentFrameDirections.Clear();
            }
        }

        [TestFixture]
        public class SolutionTest
        {
            private PlayerMovement _player;

            private void TestEquality(Direction direction, int x, int y)
            {
                _player.Update();

                Assert.AreEqual(direction, _player.Direction);
                Assert.AreEqual(new Tile(x, y), _player.Position);
            }

            [Test(Description = "Basic Test 1")]
            public void BasicTest1()
            {
                _player = new PlayerMovement(0, 0);
                Input.Clear();

                Press(Direction.Down);

                TestEquality(Direction.Down, 0, 0);
                TestEquality(Direction.Down, 0, -1);
                TestEquality(Direction.Down, 0, -2);

                Press(Direction.Left);
                Press(Direction.Right);

                TestEquality(Direction.Left, 0, -2);
                TestEquality(Direction.Left, -1, -2);

                Release(Direction.Left);

                TestEquality(Direction.Right, -1, -2);

                Release(Direction.Right);

                TestEquality(Direction.Down, -1, -2);
                TestEquality(Direction.Down, -1, -3);

                Release(Direction.Down);

                TestEquality(Direction.Down, -1, -3);
            }

            [Test(Description = "All keys at once")]
            public void BasicTest2()
            {
                _player = new PlayerMovement(0, 0);
                Input.Clear();

                Press(Direction.Down);
                Press(Direction.Left);
                Press(Direction.Right);
                Press(Direction.Up);

                TestEquality(Direction.Up, 0, 0);
                TestEquality(Direction.Up, 0, 1);

                Release(Direction.Left);

                TestEquality(Direction.Up, 0, 2);

                Release(Direction.Up);

                TestEquality(Direction.Down, 0, 2);

                Release(Direction.Down);

                TestEquality(Direction.Right, 0, 2);
                TestEquality(Direction.Right, 1, 2);
                TestEquality(Direction.Right, 2, 2);

                Release(Direction.Right);

                TestEquality(Direction.Right, 2, 2);
            }

            private void Press(Direction dir) { Console.WriteLine("Pressed " + dir); Input.Press(dir); }
            private void Release(Direction dir) { Console.WriteLine("Released " + dir); Input.Release(dir); }
        }
    }
}