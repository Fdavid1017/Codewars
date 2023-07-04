namespace ConstructingACar_3_v2;

public class OnBoardComputerDisplay : IOnBoardComputerDisplay
{
    private OnBoardComputer onBoardComputer;

    public int TripRealTime { get; }
    public int TripDrivingTime { get; }
    public double TripDrivenDistance { get; }
    public int TotalRealTime { get; }
    public int TotalDrivingTime { get; }
    public double TotalDrivenDistance { get; }
    public int ActualSpeed { get; }
    public double TripAverageSpeed { get; }
    public double TotalAverageSpeed { get; }
    public double ActualConsumptionByTime { get; }
    public double ActualConsumptionByDistance { get; }
    public double TripAverageConsumptionByTime { get; }
    public double TotalAverageConsumptionByTime { get; }
    public double TripAverageConsumptionByDistance { get; }
    public double TotalAverageConsumptionByDistance { get; }
    public int EstimatedRange { get; }

    public OnBoardComputerDisplay(OnBoardComputer onBoardComputer)
    {
        this.onBoardComputer = onBoardComputer;
    }

    public void TripReset()
    {
        throw new NotImplementedException();
    }

    public void TotalReset()
    {
        throw new NotImplementedException();
    }
}