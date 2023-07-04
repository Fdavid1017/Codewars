using static ConstructingACar_2.Tests.Car;

namespace ConstructingACar_2
{
    // https://www.codewars.com/kata/578df8f3deaed98fcf0001e9/train/csharp

    public class Tests
    {
        public class Car : ICar
        {
            private readonly FuelTank fuelTank;
            private readonly Engine engine;

            public readonly FuelTankDisplay fuelTankDisplay;

            private DrivingProcessor drivingProcessor;
            public DrivingInformationDisplay drivingInformationDisplay;

            public bool EngineIsRunning => this.engine.IsRunning;
            public Car()
            {
                fuelTank= new FuelTank();
                engine= new Engine(fuelTank);
                fuelTankDisplay= new FuelTankDisplay(fuelTank);

                drivingProcessor = new DrivingProcessor();
                drivingInformationDisplay = new DrivingInformationDisplay(drivingProcessor);
            }

            public Car(double fuelLevel) : this()
            {
                this.engine.fuelTank.FillLevel=fuelLevel;
            }

            public Car(double fuelLevel, int maxAcceleration) : this(fuelLevel)
            {
                drivingProcessor.MaxAcceleration = maxAcceleration;
            }

            public void BrakeBy(int speed)
            {
                drivingProcessor.ReduceSpeed(speed);
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

                //The consumption for a driving car is be taken from these ranges:
                //1 - 60 km/h-> 0.0020 liter/second
                //61 - 100 km/h-> 0.0014 liter/second
                //101 - 140 km/h-> 0.0020 liter/second
                //141 - 200 km/h-> 0.0025 liter/second
                //201 - 250 km/h-> 0.0030 liter/second
                double fuelConsumption;
                switch (drivingProcessor.ActualSpeed)
                {
                    case <1:
                        fuelConsumption = Engine.IDLE_FUEL_CONSUMPTION_PER_SEC;
                        break;
                    case >= 1 and <= 60:
                        fuelConsumption = 0.0020;
                        break;
                    case >= 61 and <= 100:
                        fuelConsumption = 0.0014;
                        break;
                    case >= 101 and <= 140:
                        fuelConsumption = 0.0020;
                        break;
                    case >= 141 and <= 200:
                        fuelConsumption = 0.0025;
                        break;
                    case >= 201 and <= 250:
                        fuelConsumption = 0.0030;
                        break;
                    default:
                        fuelConsumption = 0.0020;
                        break;
                }

                engine.Consume(fuelConsumption);
            }

            public void EngineStart()
            {
                this.engine.Start();
            }

            public void EngineStop()
            {
                this.engine.Stop();
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
                }
            }

            public void Refuel(double liters)
            {
                this.fuelTank.Refuel(liters);
            }

            public void RunningIdle()
            {
                this.engine.Consume(Engine.IDLE_FUEL_CONSUMPTION_PER_SEC);
            }
        }

        public class DrivingInformationDisplay : IDrivingInformationDisplay
        {
            private DrivingProcessor drivingProcessor;

            public int ActualSpeed
            {
                get => drivingProcessor.ActualSpeed;
            }

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

            public int ActualSpeed
            {
                get => actualSpeed;
                set
                {
                    actualSpeed = Math.Clamp(value, 0, MAX_SPEED);
                }
            }

            public int MaxAcceleration
            {
                get => maxAcceleration;
                set => maxAcceleration=Math.Clamp(value, 5, 20);
            }


            public DrivingProcessor()
            {
            }

            public DrivingProcessor(int maxAcceleration)
            {
                MaxAcceleration = maxAcceleration;
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
                speed -=  ActualSpeed;
                speed = Math.Clamp(speed, 0, MaxAcceleration);
                IncreaseSpeedTo(ActualSpeed+speed);
            }
        }

        public class Engine : IEngine
        {
            public const double IDLE_FUEL_CONSUMPTION_PER_SEC = 0.0003;

            public FuelTank fuelTank;

            public bool IsRunning { get; set; }

            public Engine(FuelTank fuelTank)
            {
                this.fuelTank=fuelTank;
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
                IsRunning=fuelTank.FillLevel>0;
            }

            public void Stop()
            {
                IsRunning=false;
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
                set => fillLevel=Math.Clamp(value, 0, MAX_FUEL_LEVEL);
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

            public double FillLevel
            {
                get => Math.Round(fuelTank.FillLevel, 2);
            }
            public bool IsOnReserve
            {
                get => fuelTank.IsOnReserve;
            }
            public bool IsComplete
            {
                get => fuelTank.FillLevel == FuelTank.MAX_FUEL_LEVEL;
            }

            public FuelTankDisplay(FuelTank fuelTank)
            {
                this.fuelTank = fuelTank;
            }
        }


        [TestFixture]
        public class Car2ExampleTests
        {
            [Test]
            public void TestStartSpeed()
            {
                var car = new Car();

                car.EngineStart();

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
                var car = new Car(60, 5);

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                car.Accelerate(112);
                Assert.AreEqual(105, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
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
            public void TestAccelerateLowerThanActualSpeed()
            {
                var car = new Car();

                car.EngineStart();

                Enumerable.Range(0, 10).ToList().ForEach(s => car.Accelerate(100));

                car.Accelerate(30);

                Assert.AreEqual(99, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");

                car.Accelerate(30);

                Assert.AreEqual(98, car.drivingInformationDisplay.ActualSpeed, "Wrong actual speed!");
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
        }
    }
}