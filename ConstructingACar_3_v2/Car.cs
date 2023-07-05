namespace ConstructingACar_3_v2;

public class Car : ICar
{
    private readonly Engine engine;
    private readonly FuelTank fuelTank;
    public readonly FuelTankDisplay fuelTankDisplay;

    private DrivingProcessor drivingProcessor;
    public DrivingInformationDisplay drivingInformationDisplay;

    public OnBoardComputerDisplay onBoardComputerDisplay;
    private OnBoardComputer onBoardComputer;

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
        engine.Consume(drivingProcessor.ActualConsumption);
    }

    public void EngineStart()
    {
        this.engine.Start();
        this.drivingProcessor.EngineStart();
    }

    public void EngineStop()
    {
        this.engine.Stop();
        this.drivingProcessor.EngineStop();
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
        this.drivingProcessor.RunningIdle();
    }
}