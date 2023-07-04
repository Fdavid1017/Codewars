namespace ConstructingACar_3_v2;

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