namespace ConstructingACar_3_v2;
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
