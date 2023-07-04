using NUnit.Compatibility;

namespace ConstructingACar_3
{
    // https://www.codewars.com/kata/57961d4e4be9121ec90001bd
    public class Tests
    {
        public class Car : ICar
        {
            private readonly FuelTank fuelTank;
            private readonly Engine engine;
            private readonly DrivingProcessor drivingProcessor;
            private OnBoardComputer onBoardComputer;

            public readonly FuelTankDisplay fuelTankDisplay;
            public DrivingInformationDisplay drivingInformationDisplay;
            public OnBoardComputerDisplay onBoardComputerDisplay;

            public bool EngineIsRunning => this.engine.IsRunning;
            public Car()
            {
                fuelTank = new FuelTank();
                engine = new Engine(fuelTank);
                fuelTankDisplay = new FuelTankDisplay(fuelTank);

                drivingProcessor = new DrivingProcessor();
                drivingInformationDisplay = new DrivingInformationDisplay(drivingProcessor);

                onBoardComputer = new OnBoardComputer(drivingProcessor, fuelTank);
                onBoardComputerDisplay = new OnBoardComputerDisplay(onBoardComputer);
            }

            public Car(double fuelLevel) : this()
            {
                this.engine.fuelTank.FillLevel = fuelLevel;
            }

            public Car(double fuelLevel, int maxAcceleration) : this(fuelLevel)
            {
                drivingProcessor.MaxAcceleration = maxAcceleration;
            }

            public void BrakeBy(int speed)
            {
                drivingProcessor.ReduceSpeed(speed);
                onBoardComputer.ElapseSecond();
            }

            public void Accelerate(int speed)
            {

                if (!EngineIsRunning) return;

                if (speed < drivingProcessor.ActualSpeed)
                {
                    FreeWheel();
                    return;
                }

                drivingProcessor.AccelerateTo(speed);
                engine.Consume(drivingProcessor.ActualConsumption);

                onBoardComputer.ElapseSecond();
            }

            public void EngineStart()
            {
                engine.Start();
                drivingProcessor.EngineStart();

                onBoardComputer.TripReset();
                onBoardComputer.ElapseSecond();
            }

            public void EngineStop()
            {
                engine.Stop();
                drivingProcessor.EngineStop();
                onBoardComputer.ElapseSecond();
            }

            public void FreeWheel()
            {
                if (drivingProcessor.ActualSpeed == 0)
                {
                    RunningIdle();
                }
                else
                {
                    drivingProcessor.ReduceSpeed(1);
                    onBoardComputer.ElapseSecond();
                }
            }

            public void Refuel(double liters)
            {
                this.fuelTank.Refuel(liters);
            }

            public void RunningIdle()
            {
                this.engine.Consume(Engine.IDLE_FUEL_CONSUMPTION_PER_SEC);
                onBoardComputer.ElapseSecond();
            }
        }

        public class OnBoardComputer : IOnBoardComputer
        {
            private readonly DrivingProcessor drivingProcessor;
            private readonly FuelTank fuelTank;

            private int tripSpeedSum = 0;
            private int totalSpeedSum = 0;

            private double tripConsumptionSum = 0;
            private double totalConsumptionSum = 0;

            public OnBoardComputer(DrivingProcessor drivingProcessor, FuelTank fuelTank)
            {
                this.drivingProcessor = drivingProcessor;
                this.fuelTank = fuelTank;
            }

            public int TripRealTime { get; set; } = 0;
            public int TripDrivingTime { get; set; } = 0;
            public int TripDrivenDistance { get; } = 0;
            public int TotalRealTime { get; set; } = 0;
            public int TotalDrivingTime { get; set; } = 0;
            public int TotalDrivenDistance { get; } = 0;
            public double TripAverageSpeed => (double)tripSpeedSum / TripDrivingTime;

            public double TotalAverageSpeed => (double)totalSpeedSum / TotalDrivingTime;
            public int ActualSpeed { get; }
            public double ActualConsumptionByTime { get; }
            public double ActualConsumptionByDistance { get; }
            public double TripAverageConsumptionByTime => tripConsumptionSum / TripRealTime;

            public double TotalAverageConsumptionByTime => totalConsumptionSum / TripRealTime;
            public double TripAverageConsumptionByDistance { get; }
            public double TotalAverageConsumptionByDistance { get; }

            public int EstimatedRange => 0;

            public void ElapseSecond()
            {
                TripRealTime++;
                TotalRealTime++;

                if (drivingProcessor.ActualSpeed > 0)
                {
                    TripDrivingTime++;
                    TotalDrivingTime++;

                    tripSpeedSum += drivingProcessor.ActualSpeed;
                    totalSpeedSum += drivingProcessor.ActualSpeed;

                    double kmPerSec = drivingProcessor.ActualSpeed / 0.000278;
                    TripDrivenDistance += kmPerSec;
                    TripDrivenDistance += kmPerSec;
                }

                if (drivingProcessor.IsEngineRunning)
                {
                    tripConsumptionSum += drivingProcessor.ActualConsumption;
                    totalConsumptionSum += drivingProcessor.ActualConsumption;
                }
            }

            public void TripReset()
            {
                TripRealTime = 0;
                TripDrivingTime = 0;

                tripSpeedSum = 0;
                tripConsumptionSum = 0;
            }

            public void TotalReset()
            {
                TotalRealTime = 0;
                TotalDrivingTime = 0;

                totalSpeedSum = 0;
                totalConsumptionSum = 0;
            }
        }

        public class OnBoardComputerDisplay : IOnBoardComputerDisplay
        {
            private readonly OnBoardComputer onBoardComputer;

            public OnBoardComputerDisplay(OnBoardComputer onBoardComputer)
            {
                this.onBoardComputer = onBoardComputer;
            }

            public int TripRealTime => onBoardComputer.TripRealTime;
            public int TripDrivingTime => onBoardComputer.TripDrivingTime;
            public double TripDrivenDistance => Math.Round((float)onBoardComputer.TripDrivenDistance, 2);
            public int TotalRealTime => onBoardComputer.TotalRealTime;
            public int TotalDrivingTime => onBoardComputer.TotalDrivingTime;
            public double TotalDrivenDistance => Math.Round((float)onBoardComputer.TotalDrivenDistance, 2);
            public int ActualSpeed { get; }
            public double TripAverageSpeed => Math.Round(onBoardComputer.TripAverageSpeed, 1);
            public double TotalAverageSpeed => Math.Round(onBoardComputer.TotalAverageSpeed, 1);
            public double ActualConsumptionByTime { get; }
            public double ActualConsumptionByDistance { get; }
            public double TripAverageConsumptionByTime => Math.Round(onBoardComputer.TripAverageConsumptionByTime, 5);
            public double TotalAverageConsumptionByTime => Math.Round(onBoardComputer.TotalAverageConsumptionByTime, 5);
            public double TripAverageConsumptionByDistance { get; }
            public double TotalAverageConsumptionByDistance { get; }
            public int EstimatedRange => onBoardComputer.EstimatedRange;

            public void TripReset()
            {
                onBoardComputer.TripReset();
            }

            public void TotalReset()
            {
                onBoardComputer.TotalReset();
            }
        }

        public class DrivingInformationDisplay : IDrivingInformationDisplay
        {
            private DrivingProcessor drivingProcessor;

            public int ActualSpeed => drivingProcessor.ActualSpeed;

            public DrivingInformationDisplay(DrivingProcessor drivingProcessor)
            {
                this.drivingProcessor = drivingProcessor;
            }
        }

        public class DrivingProcessor : IDrivingProcessor
        {
            private const int MAX_BREAKING = 10;
            private const int MAX_SPEED = 250;

            private int maxAcceleration = 10;
            private int actualSpeed = 0;
            private bool isEngineRunning = false;

            public bool IsEngineRunning => isEngineRunning;

            public double ActualConsumption
            {
                get
                {
                    //The consumption for a driving car is be taken from these ranges:
                    //1 - 60 km/h-> 0.0020 liter/second
                    //61 - 100 km/h-> 0.0014 liter/second
                    //101 - 140 km/h-> 0.0020 liter/second
                    //141 - 200 km/h-> 0.0025 liter/second
                    //201 - 250 km/h-> 0.0030 liter/second
                    switch (ActualSpeed)
                    {
                        case < 1:
                            return Engine.IDLE_FUEL_CONSUMPTION_PER_SEC;
                            break;
                        case >= 1 and <= 60:
                            return 0.0020;
                            break;
                        case >= 61 and <= 100:
                            return 0.0014;
                            break;
                        case >= 101 and <= 140:
                            return 0.0020;
                            break;
                        case >= 141 and <= 200:
                            return 0.0025;
                            break;
                        case >= 201 and <= 250:
                            return 0.0030;
                            break;
                        default:
                            return 0.0020;
                            break;
                    }
                }
            }

            public int ActualSpeed
            {
                get => actualSpeed;
                set => actualSpeed = Math.Clamp(value, 0, MAX_SPEED);
            }

            public int MaxAcceleration
            {
                get => maxAcceleration;
                set => maxAcceleration = Math.Clamp(value, 5, 20);
            }

            public DrivingProcessor()
            {
            }

            public DrivingProcessor(int maxAcceleration)
            {
                MaxAcceleration = maxAcceleration;
            }

            public void EngineStart()
            {
                isEngineRunning = true;
            }

            public void EngineStop()
            {
                isEngineRunning = false;
            }

            public void IncreaseSpeedTo(int speed)
            {
                ActualSpeed = speed;
            }

            public void ReduceSpeed(int speed)
            {
                speed = Math.Clamp(speed, 0, MAX_BREAKING);
                ActualSpeed -= speed;
            }

            public void AccelerateTo(int speed)
            {
                speed -= ActualSpeed;
                speed = Math.Clamp(speed, 0, MaxAcceleration);
                IncreaseSpeedTo(ActualSpeed + speed);
            }
        }

        public class Engine : IEngine
        {
            public const double IDLE_FUEL_CONSUMPTION_PER_SEC = 0.0003;

            public FuelTank fuelTank;

            public bool IsRunning { get; set; }

            public Engine(FuelTank fuelTank)
            {
                this.fuelTank = fuelTank;
            }

            public void Consume(double liters)
            {
                if (IsRunning)
                {
                    if (fuelTank.FillLevel <= liters)
                    {
                        Stop();
                    }
                    else
                    {
                        fuelTank.Consume(liters);
                    }
                }
            }

            public void Start()
            {
                IsRunning = fuelTank.FillLevel > 0;
            }

            public void Stop()
            {
                IsRunning = false;
            }
        }

        public class FuelTank : IFuelTank
        {
            public const double MAX_FUEL_LEVEL = 60;
            public const double FUEL_TANK_RESERVE_LIMIT = 5;

            private double fillLevel = 20;
            public double FillLevel
            {
                get => fillLevel;
                set => fillLevel = Math.Clamp(value, 0, MAX_FUEL_LEVEL);
            }

            public bool IsOnReserve => this.FillLevel < FUEL_TANK_RESERVE_LIMIT;

            public bool IsComplete { get; }

            public void Consume(double liters)
            {
                this.FillLevel -= liters;
            }

            public void Refuel(double liters)
            {
                this.FillLevel += liters;
            }
        }

        public class FuelTankDisplay : IFuelTankDisplay
        {
            private FuelTank fuelTank;

            public double FillLevel => Math.Round(fuelTank.FillLevel, 2);

            public bool IsOnReserve => fuelTank.IsOnReserve;

            public bool IsComplete => fuelTank.FillLevel == FuelTank.MAX_FUEL_LEVEL;

            public FuelTankDisplay(FuelTank fuelTank)
            {
                this.fuelTank = fuelTank;
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
    }
}