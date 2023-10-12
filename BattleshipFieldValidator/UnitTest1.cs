namespace BattleshipFieldValidator
{
    public class Tests
    {
        public class BattleshipField
        {
            enum Ship
            {
                BATTLESHIP, CRUISER, DESTROYER, SUBMARINE
            }

            public static bool ValidateBattlefield(int[,] field)
            {
                const int SHIP_MARKER = 1;
                const int FOUND_SHIP_MARKER = 2;
                const int MAX_SHIP_COUNT = 20;

                Console.WriteLine("Original map:");
                PrintMap();


                Dictionary<Ship, int> shipsFound = new Dictionary<Ship, int>()
                {
                    {Ship.BATTLESHIP, 0},  // 4 cell / 1
                    {Ship.CRUISER, 0},     // 3 cell / 2
                    {Ship.DESTROYER, 0},   // 2 cell / 3
                    {Ship.SUBMARINE, 0},   // 1 cell / 4
                };
                int totalShipSize = 0;


                for (int y = 0; y < field.GetLength(0); y++)
                {
                    for (int x = 0; x < field.GetLength(1); x++)
                    {
                        if (field[y, x] != SHIP_MARKER) continue;

                        bool hasTouchingShip;
                        int horizontalLength = GetShipHorizontalLength(y, x, out hasTouchingShip);

                        if (hasTouchingShip)
                            return false;

                        int verticalLength = GetShipVerticalLength(y, x, out hasTouchingShip);

                        if (hasTouchingShip)
                            return false;

                        int shipSize = Math.Max(horizontalLength, verticalLength);

                        totalShipSize += shipSize;

                        if (totalShipSize > MAX_SHIP_COUNT)
                            return false; // More ship placed than possible

                        bool canBePlaced = false;
                        switch (shipSize)
                        {
                            case 1:
                                canBePlaced = SetFoundShip(Ship.SUBMARINE);
                                break;
                            case 2:
                                canBePlaced = SetFoundShip(Ship.DESTROYER);
                                break;
                            case 3:
                                canBePlaced = SetFoundShip(Ship.CRUISER);
                                break;
                            case 4:
                                canBePlaced = SetFoundShip(Ship.BATTLESHIP);
                                break;
                        }

                        if (!canBePlaced)
                            return false; // More placed from this type of ship than possible

                        // Mark found ship
                        for (int shipVertical = y; shipVertical < y + verticalLength; shipVertical++)
                        {
                            for (int shipHorizontal = x; shipHorizontal < x + horizontalLength; shipHorizontal++)
                            {
                                field[shipVertical, shipHorizontal] = FOUND_SHIP_MARKER;
                            }
                        }
                    }
                }

                Console.WriteLine("Map after checks:");
                PrintMap();

                return totalShipSize == MAX_SHIP_COUNT;


                /* ========== LOCAL FUNCTIONS ========== */

                void PrintMap()
                {
                    for (int y = 0; y < field.GetLength(0); y++)
                    {
                        for (int x = 0; x < field.GetLength(1); x++)
                        {
                            Console.Write($"{field[y, x]} ");
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine("\n\n");
                }

                bool SetFoundShip(Ship ship)
                {
                    /*
                     { Ship.BATTLESHIP, 0},  // 4 cell / 1
                     { Ship.CRUISER, 0},     // 3 cell / 2
                     { Ship.DESTROYER, 0},   // 2 cell / 3
                     { Ship.SUBMARINE, 0},   // 1 cell / 4
                    */

                    shipsFound[ship]++;

                    switch (ship)
                    {
                        case Ship.BATTLESHIP:
                            return shipsFound[ship] <= 1;
                        case Ship.CRUISER:
                            return shipsFound[ship] <= 2;
                        case Ship.DESTROYER:
                            return shipsFound[ship] <= 3;
                        case Ship.SUBMARINE:
                            return shipsFound[ship] <= 4;
                        default:
                            return false;
                    }
                }

                int GetShipHorizontalLength(int y, int x, out bool hasTouchingShip)
                {
                    int length = 0;
                    hasTouchingShip = false;

                    for (; x < field.GetLength(1); x++)
                    {
                        if (field[y, x] != SHIP_MARKER)
                            break;

                        if (y + 1 < field.GetLength(0))
                        {
                            hasTouchingShip = hasTouchingShip || field[y + 1, x] == SHIP_MARKER;
                        }
                        hasTouchingShip = hasTouchingShip || DiagonalTouch(y, x);

                        length++;
                    }

                    if (hasTouchingShip)
                    {
                        hasTouchingShip = length > 1;
                    }

                    return length;
                }

                int GetShipVerticalLength(int y, int x, out bool hasTouchingShip)
                {
                    int length = 0;
                    hasTouchingShip = false;

                    for (; y < field.GetLength(0); y++)
                    {
                        if (field[y, x] != SHIP_MARKER)
                            break;

                        if (x + 1 < field.GetLength(1))
                        {
                            hasTouchingShip = hasTouchingShip || field[y, x + 1] == SHIP_MARKER;
                        }
                        hasTouchingShip = hasTouchingShip || DiagonalTouch(y, x);

                        length++;
                    }

                    if (hasTouchingShip)
                    {
                        hasTouchingShip = length > 1;
                    }

                    return length;
                }

                bool DiagonalTouch(int y, int x)
                {
                    if (y - 1 >= 0 && x - 1 >= 0 && field[y - 1, x - 1] == SHIP_MARKER)
                        return true;

                    if (y - 1 >= 0 && x + 1 >= 0 && field[y - 1, x + 1] == SHIP_MARKER)
                        return true;

                    if (y + 1 >= 0 && x - 1 >= 0 && field[y + 1, x - 1] == SHIP_MARKER)
                        return true;

                    if (y + 1 >= 0 && x + 1 >= 0 && field[y + 1, x + 1] == SHIP_MARKER)
                        return true;

                    return false;
                }
            }
        }

        [TestFixture]
        public class SolutionTest
        {
            [Test]
            public void DebugTestCase()
            {
                int[,] field = new int[10, 10]
                {
                    {1, 1, 1, 0, 0, 1, 1, 0, 0, 0},
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                    {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsTrue(BattleshipField.ValidateBattlefield(field));
            }
            [Test]
            public void TestCase()
            {
                int[,] field = new int[10, 10]
                {
                    {1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                    {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsTrue(BattleshipField.ValidateBattlefield(field));
            }

            [Test]
            public void TouchingTestCase()
            {
                int[,] field = new int[10, 10]
                {
                    {1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    {1, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {1, 1, 1, 0, 1, 1, 1, 0, 1, 0},
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsFalse(BattleshipField.ValidateBattlefield(field));
            }

            [Test]
            public void NotEnoughShipCase()
            {
                int[,] field = new int[10, 10]
                {
                    {0, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                    {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsFalse(BattleshipField.ValidateBattlefield(field));
            }

            [Test]
            public void TooManyShipCase()
            {
                int[,] field = new int[10, 10]
                {
                    {0, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                    {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    {1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                    {1, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsFalse(BattleshipField.ValidateBattlefield(field));
            }

            [Test]
            public void DiagonalTouchCase()
            {
                int[,] field = new int[10, 10]
                {
                    { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
                    { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
                    { 1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                    { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
                    { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
                Assert.IsFalse(BattleshipField.ValidateBattlefield(field));
            }
        }
    }
}