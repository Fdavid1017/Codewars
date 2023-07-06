namespace ConstructingACar_3_v2
{
    // https://www.codewars.com/kata/57961d4e4be9121ec90001bd
    public class Tests
    {
        [TestFixture]
        public class Car1RealTests
        {
            [Test]
            public void TestMotorStartAndStop()
            {
                var car = new Car();

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");

                car.EngineStart();

                Assert.IsTrue(car.EngineIsRunning, "Engine should be running.");

                car.EngineStop();

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestMotorStartAgainAndStopAgain()
            {
                var car = new Car();

                car.EngineStart();

                car.EngineStart();

                Assert.IsTrue(car.EngineIsRunning, "Engine should be running.");

                car.EngineStop();

                car.EngineStop();

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestMotorDoesntStartWithEmptyFuelTank()
            {
                var car = new Car(0);

                car.EngineStart();

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestFuelConsumptionOnIdle()
            {
                var car = new Car(1);

                car.EngineStart();

                Enumerable.Range(0, 3000).ToList().ForEach(s => car.RunningIdle());

                Assert.AreEqual(0.10, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestEngineStopsCauseOfNoFuelExactly()
            {
                var car = new Car(6);

                car.EngineStart();

                Enumerable.Range(0, 20000).ToList().ForEach(s => car.RunningIdle());

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestEngineStopsCauseOfNoFuelOver()
            {
                var car = new Car(1);

                car.EngineStart();

                Enumerable.Range(0, 10000).ToList().ForEach(s => car.RunningIdle());

                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestNoConsumptionWhenEngineNotRunning()
            {
                var car = new Car(1);

                Enumerable.Range(0, 1000).ToList().ForEach(s => car.RunningIdle());

                Assert.AreEqual(1, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestFuelTankDisplayIsNotComplete()
            {
                var car = new Car();

                Assert.IsFalse(car.fuelTankDisplay.IsComplete, "Fuel tank must be not complete!");
            }

            [Test]
            public void TestFuelTankDisplayIsComplete()
            {
                var car = new Car(60);

                Assert.IsTrue(car.fuelTankDisplay.IsComplete, "Fuel tank must be complete!");
            }

            [Test]
            public void TestFuelTankDisplayIsNotOnReserve()
            {
                var car = new Car();

                Assert.IsFalse(car.fuelTankDisplay.IsOnReserve, "Fuel tank must be not on reserve!");
            }

            [Test]
            public void TestFuelTankDisplayIsOnReserve()
            {
                var car = new Car(4);

                Assert.IsTrue(car.fuelTankDisplay.IsOnReserve, "Fuel tank must be on reserve!");
            }

            [Test]
            public void TestRefuel()
            {
                var car = new Car(5);

                car.Refuel(40);

                Assert.AreEqual(45, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestRefuelOverMaximum()
            {
                var car = new Car(5);

                car.Refuel(80);

                Assert.AreEqual(60, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestNoNegativeFuelLevelAllowed()
            {
                var car = new Car(-5);

                Assert.AreEqual(0, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestFuelLevelAllowedUpTo60()
            {
                var car = new Car(65);

                Assert.AreEqual(60, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void Car1RandomTests()
            {
                var rand = new Random();

                for (int i = 0; i < 20; i++)
                {
                    var car1 = new Car(5);

                    var refuelLiter = rand.Next(60);

                    car1.Refuel(refuelLiter);

                    double expectedFuelLevel = Math.Min(5 + refuelLiter, 60);
                    Assert.AreEqual(expectedFuelLevel, car1.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");

                    var car2 = new Car(5);
                    car2.EngineStart();

                    var runningIdleSeconds = rand.Next(7);

                    Enumerable.Range(0, runningIdleSeconds * 10001 / 3).ToList().ForEach(s => { car2.RunningIdle(); });

                    expectedFuelLevel = Math.Max(5 - runningIdleSeconds, 0);

                    Assert.AreEqual(expectedFuelLevel, car2.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
                    if (expectedFuelLevel == 0)
                    {
                        Assert.IsFalse(car2.EngineIsRunning, "Engine could not be running.");
                    }
                }
            }
        }

        [TestFixture]
        public class Car2RealTests
        {
            [Test]
            public void TestStartSpeed()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestFreeWheelNoSpeedReduceWhenNoMoving()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car.FreeWheel();

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestFreeWheelSpeed()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                Assert.AreEqual(100, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car.FreeWheel();
                car.FreeWheel();
                car.FreeWheel();

                Assert.AreEqual(97, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestMaxAccelerationOutOfRange()
            {
                var car = new Car(20, 25);

                car.EngineStart();

                car.Accelerate(21);

                Assert.AreEqual(20, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car = new Car(20, 0);

                car.EngineStart();

                car.Accelerate(6);

                Assert.AreEqual(5, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car = new Car(20, -10);

                car.EngineStart();

                car.Accelerate(6);

                Assert.AreEqual(5, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestAccelerateBy10()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                car.Accelerate(160);
                Assert.AreEqual(110, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(160);
                Assert.AreEqual(120, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(160);
                Assert.AreEqual(130, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(160);
                Assert.AreEqual(140, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(145);
                Assert.AreEqual(145, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestAccelerateBy5()
            {
                var car = new Car(30, 5);

                car.EngineStart();

                Enumerable.Range(0, 20).ToList().ForEach(s => car.Accelerate(100));

                car.Accelerate(112);
                Assert.AreEqual(105, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(112);
                Assert.AreEqual(110, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                car.Accelerate(112);
                Assert.AreEqual(112, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestBrakeOnlyOver0()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 11).ToList().ForEach(c => car.BrakeBy(10));

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestAccelerateOnlyUntil250()
            {
                var car = new Car(20, 20);

                car.EngineStart();

                Enumerable.Range(0, 13).ToList().ForEach(c => car.Accelerate(260));

                Assert.AreEqual(250, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestAccelerateLowerThanActualSpeed()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                car.Accelerate(30);

                Assert.AreEqual(99, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestBraking()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                car.BrakeBy(20);

                Assert.AreEqual(90, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car.BrakeBy(10);

                Assert.AreEqual(80, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestFreeWheelEndingAtZero()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(5);

                Enumerable.Range(0, 6).ToList().ForEach(s => car.FreeWheel());

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestNoAccelerationWhenEngineNotRunning()
            {
                var car = new Car();

                car.Accelerate(5);

                Assert.AreEqual(0, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
            }

            [Test]
            public void TestConsumptionSpeedUpTo30()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                Assert.AreEqual(0.98, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestConsumptionSpeedUpTo80()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                car.Accelerate(80);
                car.Accelerate(80);
                car.Accelerate(80);
                car.Accelerate(80);

                Enumerable.Range(0, 17).ToList().ForEach(s => car.Accelerate(80));

                Assert.AreEqual(0.97, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestConsumptionSpeedUpTo120()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                car.Accelerate(120);
                car.Accelerate(120);
                car.Accelerate(120);
                car.Accelerate(120);
                car.Accelerate(120);
                car.Accelerate(120);

                Enumerable.Range(0, 20).ToList().ForEach(s => car.Accelerate(120));

                Assert.AreEqual(0.95, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestConsumptionSpeedUpTo250()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                car.Accelerate(250); // 20
                car.Accelerate(250); // 40
                car.Accelerate(250); // 60
                car.Accelerate(250); // 80
                car.Accelerate(250); // 100
                car.Accelerate(250); // 120
                car.Accelerate(250); // 140
                car.Accelerate(250); // 160
                car.Accelerate(250); // 180
                car.Accelerate(250); // 200
                car.Accelerate(250); // 220
                car.Accelerate(250); // 240
                car.Accelerate(250); // 250

                Assert.AreEqual(0.97, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestConsumptionLeadsToStopEngine()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                car.Accelerate(250); // 20
                car.Accelerate(250); // 40
                car.Accelerate(250); // 60
                car.Accelerate(250); // 80
                car.Accelerate(250); // 100
                car.Accelerate(250); // 120
                car.Accelerate(250); // 140
                car.Accelerate(250); // 160
                car.Accelerate(250); // 180
                car.Accelerate(250); // 200
                car.Accelerate(250); // 220
                car.Accelerate(250); // 240
                car.Accelerate(250); // 250

                Enumerable.Range(0, 325).ToList().ForEach(s => car.Accelerate(250));

                Assert.AreEqual(0, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
                Assert.IsFalse(car.EngineIsRunning, "Engine could not be running.");
            }

            [Test]
            public void TestConsumptionAsRunIdleWhenFreeWheelingAt0()
            {
                var car = new Car(1, 20);

                car.EngineStart();

                Enumerable.Range(0, 200).ToList().ForEach(s => car.FreeWheel());

                Assert.AreEqual(0.94, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void Car2RandomTests()
            {
                var rand = new Random();

                for (int i = 0; i < 20; i++)
                {
                    var maxAcceleration = rand.Next(5, 20);
                    var expectedSpeed = 0;
                    double expectedFuelLevel = 20;

                    var car = new Car(20, maxAcceleration);

                    car.EngineStart();

                    Enumerable.Range(0, 10).ToList().ForEach(s =>
                    {
                        car.Accelerate(250);
                        expectedSpeed += maxAcceleration;
                        expectedFuelLevel -= GetConsumption(expectedSpeed);
                    });

                    var brakeBySpeed = rand.Next(5, 16);

                    car.BrakeBy(brakeBySpeed);

                    expectedSpeed -= Math.Min(brakeBySpeed, 10);

                    var freeWheelSeconds = rand.Next(10, 20);
                    Enumerable.Range(0, freeWheelSeconds).ToList().ForEach(c =>
                    {
                        car.FreeWheel();
                        if (expectedSpeed > 0)
                        {
                            expectedSpeed--;
                        }
                        if (expectedSpeed == 0)
                        {
                            expectedFuelLevel -= 0.0003;
                        }
                    });

                    var accelerateSpeed = rand.Next(5, 12);

                    car.Accelerate(expectedSpeed + accelerateSpeed);

                    expectedSpeed = expectedSpeed + Math.Min(maxAcceleration, accelerateSpeed);

                    Assert.AreEqual(expectedSpeed, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
                    Assert.AreEqual(Math.Round(expectedFuelLevel, 2), car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
                }
            }

            private double GetConsumption(int speed)
            {
                double consumption = 0.0020;

                if ((speed > 61) && (speed <= 100))
                {
                    consumption = 0.0014;
                }
                if ((speed > 141) && (speed <= 200))
                {
                    consumption = 0.0025;
                }
                if ((speed > 201) && (speed <= 250))
                {
                    consumption = 0.0030;
                }

                return consumption;
            }
        }

        [TestFixture]
        public class Car3ExampleTests
        {
            [Test]
            public void TestRealAndDrivingTimeBeforeStarting()
            {
                var car = new Car();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeAfterDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);

                car.Accelerate(30);

                Assert.AreEqual(11, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(8, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(11, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(8, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestActualSpeedBeforeDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualSpeed, "Wrong actual speed.");
            }

            [Test]
            public void TestAverageSpeed1()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(18, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(18, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestAverageSpeedAfterTripReset()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                car.onBoardComputerDisplay.TripReset();

                car.Accelerate(20);

                car.Accelerate(20);

                Assert.AreEqual(15, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(20, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestActualConsumptionEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionRunningIdle()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();

                Assert.AreEqual(0.0003, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionAccelerateTo100()
            {
                var car = new Car(40, 20);

                car.EngineStart();

                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);

                Assert.AreEqual(0.0014, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(5, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionFreeWheel()
            {
                var car = new Car(40, 20);

                car.EngineStart();

                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);

                car.FreeWheel();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterAccelerating()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                Assert.AreEqual(0.0015, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.0015, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(44, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(44, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterBraking()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(0.0009, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.0009, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(26.4, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(26.4, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestDrivenDistancesAfterEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterAccelerating()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 30).ToList().ForEach(c => car.Accelerate(30));

                Assert.AreEqual(0.24, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0.24, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingOptimumSpeedForMoreThan100Seconds()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 150).ToList().ForEach(c => car.Accelerate(100));

                Assert.AreEqual(393, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }
        }

        [TestFixture]
        public class Car3RealTests
        {
            [Test]
            public void TestRealAndDrivingTimeBeforeStarting()
            {
                var car = new Car();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeBeforeStartingAfterRefueling()
            {
                var car = new Car();

                car.Refuel(10);

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeBeforeStartingAccelerateTo10()
            {
                var car = new Car();

                car.Accelerate(10);

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeBeforeStartingBrakeBy10()
            {
                var car = new Car();

                car.BrakeBy(10);

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeBeforeStartingFreeWheel()
            {
                var car = new Car();

                car.FreeWheel();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeBeforeStartingRunningIdl()
            {
                var car = new Car();

                car.RunningIdle();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeAfterRunningIdle()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                Assert.AreEqual(3, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(3, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestRealAndDrivingTimeAfterDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);

                car.Accelerate(30);

                Assert.AreEqual(11, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(8, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(11, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(8, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestDrivingTimeAfterDrivingWithEngineRestart()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(9, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(5, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(9, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(5, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");

                car.EngineStop();

                car.Refuel(10);

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);

                car.Accelerate(30);

                Assert.AreEqual(6, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(4, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(16, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(9, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestDrivingTimeAfterDrivingAfterReset()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                car.onBoardComputerDisplay.TripReset();
                car.onBoardComputerDisplay.TotalReset();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");
            }

            [Test]
            public void TestActualSpeedBeforeDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualSpeed, "Wrong actual speed.");
            }

            [Test]
            public void TestActualSpeedWhenDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                Assert.AreEqual(30, car.onBoardComputerDisplay.ActualSpeed, "Wrong actual speed.");
            }

            [Test]
            public void TestAverageSpeed1()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(18, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(18, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestAverageSpeed2()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(21.4, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(21.4, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestAverageSpeedAfterEngineRestart()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                car.EngineStop();

                car.EngineStart();

                car.Accelerate(20);

                car.Accelerate(20);

                Assert.AreEqual(15, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(20, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestAverageSpeedAfterTripReset()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();
                car.RunningIdle();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                car.onBoardComputerDisplay.TripReset();

                car.Accelerate(20);

                car.Accelerate(20);

                Assert.AreEqual(15, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                Assert.AreEqual(20, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");
            }

            [Test]
            public void TestActualConsumptionEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionEngineStartAfterDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(10);

                car.BrakeBy(10);

                car.EngineStop();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionEngineStopAfterDriving()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(10);

                car.BrakeBy(10);

                car.EngineStop();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionRunningIdle()
            {
                var car = new Car();

                car.EngineStart();

                car.RunningIdle();

                Assert.AreEqual(0.0003, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(double.NaN, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionAccelerateTo10()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(10);

                Assert.AreEqual(0.0020, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(72, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionAccelerateTo100()
            {
                var car = new Car(40, 20);

                car.EngineStart();

                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);

                Assert.AreEqual(0.0014, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(5, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionBrakeBy()
            {
                var car = new Car(40, 20);

                car.EngineStart();

                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);

                car.BrakeBy(10);

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestActualConsumptionFreeWheel()
            {
                var car = new Car(40, 20);

                car.EngineStart();

                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);
                car.Accelerate(100);

                car.FreeWheel();

                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsBeforeEngineStart()
            {
                var car = new Car();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterAccelerating()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                Assert.AreEqual(0.0015, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.0015, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(44, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(44, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterAcceleratingAndReset()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.onBoardComputerDisplay.TripReset();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.0015, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(44, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.onBoardComputerDisplay.TotalReset();

                Assert.AreEqual(0.002, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(24, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterBraking()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(30);
                car.Accelerate(30);
                car.Accelerate(30);

                car.BrakeBy(10);
                car.BrakeBy(10);
                car.BrakeBy(10);

                Assert.AreEqual(0.0009, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.0009, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(26.4, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(26.4, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestAverageConsumptionsAfterRunningIdle()
            {
                var car = new Car();

                car.EngineStart();

                car.Accelerate(10);
                car.BrakeBy(5);
                car.BrakeBy(5);

                car.RunningIdle();
                car.RunningIdle();
                car.RunningIdle();

                Assert.AreEqual(0.00046, car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                Assert.AreEqual(0.00046, car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");
                Assert.AreEqual(36, car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                Assert.AreEqual(36, car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");
            }

            [Test]
            public void TestDrivenDistancesBeforeEngineStart()
            {
                var car = new Car();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterEngineStart()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterRunningIdle()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 200).ToList().ForEach(c => car.RunningIdle());

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterAccelerating()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 30).ToList().ForEach(c => car.Accelerate(30));

                Assert.AreEqual(0.24, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0.24, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterDriving()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 30).ToList().ForEach(c => car.Accelerate(100));

                car.BrakeBy(10);
                car.BrakeBy(10);

                car.Accelerate(100);
                car.Accelerate(100);

                Assert.AreEqual(0.81, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0.81, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestDrivenDistancesAfterDrivingAndResets()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 30).ToList().ForEach(c => car.Accelerate(100));

                car.BrakeBy(10);
                car.BrakeBy(10);

                car.Accelerate(100);
                car.Accelerate(100);

                car.BrakeBy(10);

                car.onBoardComputerDisplay.TripReset();

                Assert.AreEqual(0, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0.83, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");

                Enumerable.Range(0, 10).ToList().ForEach(c => car.Accelerate(100));

                car.onBoardComputerDisplay.TotalReset();

                Assert.AreEqual(0.28, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                Assert.AreEqual(0, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
            }

            [Test]
            public void TestEstimatedRangeBeforeDriving()
            {
                var car = new Car();

                car.EngineStart();

                Assert.AreEqual(417, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingSlowSpeedForLowerThan100Seconds()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 50).ToList().ForEach(c => car.Accelerate(30));

                Assert.AreEqual(133, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingOptimumSpeedForLowerThan100Seconds()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 50).ToList().ForEach(c => car.Accelerate(100));

                Assert.AreEqual(310, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingOptimumSpeedForMoreThan100Seconds() // Example Test
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 150).ToList().ForEach(c => car.Accelerate(100));

                Assert.AreEqual(393, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingMaxSpeed()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 150).ToList().ForEach(c => car.Accelerate(250));

                Assert.AreEqual(453, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void TestEstimatedRangeAfterDrivingMaxSpeedAndReset()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 75).ToList().ForEach(c => car.Accelerate(100));

                car.onBoardComputerDisplay.TripReset();
                car.onBoardComputerDisplay.TotalReset();

                Enumerable.Range(0, 75).ToList().ForEach(c => car.Accelerate(100));

                Assert.AreEqual(393, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");
            }

            [Test]
            public void Car3RandomTests()
            {
                var rand = new Random();

                for (int i = 0; i < 20; i++)
                {
                    var car = new Car();

                    car.EngineStart();

                    double fuelLevel = 20;

                    double expectedDrivenDistance = 0;

                    double expectedTripAverageConsumptionByTime = 0;

                    double expectedTotalAverageConsumptionByTime = 0;

                    double expectedTripAverageConsumptionByDistance = 0;

                    double expectedTotalAverageConsumptionByDistance = 0;

                    var consumptionLast100Seconds = new Queue<double>();
                    Enumerable.Range(0, 100).ToList().ForEach(r => consumptionLast100Seconds.Enqueue(4.8));

                    var countAcceleratingSeconds = rand.Next(4, 20);
                    var accelerateSpeed = rand.Next(21, 31);

                    Enumerable.Range(0, countAcceleratingSeconds).ToList().ForEach(
                        r =>
                        {
                            car.Accelerate(accelerateSpeed);
                            consumptionLast100Seconds.Dequeue();
                            int speed = accelerateSpeed;
                            if (r == 0)
                            {
                                speed = 10;
                            }

                            if (r == 1)
                            {
                                speed = 20;
                            }

                            expectedDrivenDistance += speed;
                            consumptionLast100Seconds.Enqueue(((double)200) / speed * 3.6);
                            fuelLevel -= 0.002;

                            expectedTripAverageConsumptionByTime = expectedTripAverageConsumptionByTime
                                    - ((expectedTripAverageConsumptionByTime - 0.002) / (r + 2));

                            expectedTotalAverageConsumptionByTime = expectedTotalAverageConsumptionByTime
                                        - ((expectedTotalAverageConsumptionByTime - 0.002) / (r + 2));

                            var actualConsumptionByDistance = 0.002 / speed * 3600 * 100;

                            expectedTripAverageConsumptionByDistance = expectedTripAverageConsumptionByDistance
                                        - ((expectedTripAverageConsumptionByDistance - actualConsumptionByDistance) / (r + 1));

                            expectedTotalAverageConsumptionByDistance = expectedTotalAverageConsumptionByDistance
                                                        - ((expectedTotalAverageConsumptionByDistance - actualConsumptionByDistance) / (r + 1));
                        });

                    Assert.AreEqual(accelerateSpeed, car.onBoardComputerDisplay.ActualSpeed, "Wrong actual speed");
                    Assert.AreEqual(0.002, car.onBoardComputerDisplay.ActualConsumptionByTime, "Wrong Actual-Consumption-By-Time");

                    var expectetActualConsumptionByDistance = Math.Round(0.002 / accelerateSpeed * 3600 * 100, 1);
                    Assert.AreEqual(expectetActualConsumptionByDistance, car.onBoardComputerDisplay.ActualConsumptionByDistance, "Wrong Actual-Consumption-By-Distance");

                    Assert.AreEqual(Math.Round(expectedTripAverageConsumptionByTime, 5), car.onBoardComputerDisplay.TripAverageConsumptionByTime, "Wrong Trip-Average-Consumption-By-Time");
                    Assert.AreEqual(Math.Round(expectedTotalAverageConsumptionByTime, 5), car.onBoardComputerDisplay.TotalAverageConsumptionByTime, "Wrong Total-Average-Consumption-By-Time");

                    Assert.AreEqual(Math.Round(expectedTripAverageConsumptionByDistance, 1), car.onBoardComputerDisplay.TripAverageConsumptionByDistance, "Wrong Trip-Average-Consumption-By-Distance");
                    Assert.AreEqual(Math.Round(expectedTotalAverageConsumptionByDistance, 1), car.onBoardComputerDisplay.TotalAverageConsumptionByDistance, "Wrong Total-Average-Consumption-By-Distance");

                    var expectedAverageSpeed = Math.Round(((((double)countAcceleratingSeconds - 2) * accelerateSpeed) + 30) / countAcceleratingSeconds, 1);

                    Assert.AreEqual(expectedAverageSpeed, car.onBoardComputerDisplay.TripAverageSpeed, "Wrong Trip-Average-Speed.");
                    Assert.AreEqual(expectedAverageSpeed, car.onBoardComputerDisplay.TotalAverageSpeed, "Wrong Total-Average-Speed.");

                    var expectedEstimatedRange = Math.Round(fuelLevel / (consumptionLast100Seconds.Sum() / 100) * 100);

                    Assert.AreEqual(expectedEstimatedRange, car.onBoardComputerDisplay.EstimatedRange, "Wrong Estimated-Range.");

                    Assert.AreEqual(countAcceleratingSeconds + 1, car.onBoardComputerDisplay.TripRealTime, "Wrong Trip-Real-Time!");
                    Assert.AreEqual(countAcceleratingSeconds, car.onBoardComputerDisplay.TripDrivingTime, "Wrong Trip-Driving-Time!");
                    Assert.AreEqual(countAcceleratingSeconds + 1, car.onBoardComputerDisplay.TotalRealTime, "Wrong Total-Real-Time!");
                    Assert.AreEqual(countAcceleratingSeconds, car.onBoardComputerDisplay.TotalDrivingTime, "Wrong Total-Driving-Time!");

                    expectedDrivenDistance = Math.Round(expectedDrivenDistance / 1000 / 3.6, 2);

                    Assert.AreEqual(expectedDrivenDistance, car.onBoardComputerDisplay.TripDrivenDistance, "Wrong Trip-Driven-Distance.");
                    Assert.AreEqual(expectedDrivenDistance, car.onBoardComputerDisplay.TotalDrivenDistance, "Wrong Total-Driven-Distance.");
                }
            }
        }
    }
}