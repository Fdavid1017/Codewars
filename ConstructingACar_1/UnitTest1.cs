namespace ConstructingACar_1
{
    public class Tests
    {
        public class Car : ICar
        {
            private readonly FuelTank fuelTank;
            private readonly Engine engine;

            public readonly FuelTankDisplay fuelTankDisplay;

            public Car()
            {
                fuelTank= new FuelTank();
                engine= new Engine(fuelTank);
                fuelTankDisplay= new FuelTankDisplay(fuelTank);
            }

            public Car(double fuelLevel) : this()
            {
                this.engine.fuelTank.FillLevel=fuelLevel;
            }

            public bool EngineIsRunning => this.engine.IsRunning;

            public void EngineStart()
            {
                this.engine.Start();
            }

            public void EngineStop()
            {
                this.engine.Stop();
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
                this.FillLevel = this.FillLevel - liters;
            }

            public void Refuel(double liters)
            {
                this.FillLevel = this.FillLevel + liters;
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
        public class Car1ExampleTests
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
            public void TestFuelConsumptionOnIdle()
            {
                var car = new Car(1);

                car.EngineStart();

                Enumerable.Range(0, 3000).ToList().ForEach(s => car.RunningIdle());

                Assert.AreEqual(0.10, car.fuelTankDisplay.FillLevel, "Wrong fuel tank fill level!");
            }

            [Test]
            public void TestFuelTankDisplayIsComplete()
            {
                var car = new Car(60);

                Assert.IsTrue(car.fuelTankDisplay.IsComplete, "Fuel tank must be complete!");
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
        }
    }
}