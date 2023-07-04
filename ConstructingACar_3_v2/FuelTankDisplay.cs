namespace ConstructingACar_3_v2;

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