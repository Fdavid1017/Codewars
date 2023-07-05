namespace ConstructingACar_3_v2;

public class OnBoardComputerDisplay : IOnBoardComputerDisplay
{
    private OnBoardComputer onBoardComputer;

    public int TripRealTime => onBoardComputer.TripRealTime;
    public int TripDrivingTime => onBoardComputer.TripDrivingTime;
    public double TripDrivenDistance => Math.Round(onBoardComputer.TripDrivenDistanceDouble, 2);
    public int TotalRealTime => onBoardComputer.TotalRealTime;
    public int TotalDrivingTime => onBoardComputer.TotalDrivingTime;
    public double TotalDrivenDistance => Math.Round(onBoardComputer.TotalDrivenDistanceDouble, 2);
    public int ActualSpeed => onBoardComputer.ActualSpeed;
    public double TripAverageSpeed => Math.Round(onBoardComputer.TripAverageSpeed, 1);
    public double TotalAverageSpeed => Math.Round(onBoardComputer.TotalAverageSpeed, 1);
    public double ActualConsumptionByTime => Math.Round(onBoardComputer.ActualConsumptionByTime, 5);
    public double ActualConsumptionByDistance => Math.Round(onBoardComputer.ActualConsumptionByDistance, 1);
    public double TripAverageConsumptionByTime => Math.Round(onBoardComputer.TripAverageConsumptionByTime, 5);
    public double TotalAverageConsumptionByTime => Math.Round(onBoardComputer.TotalAverageConsumptionByTime, 5);
    public double TripAverageConsumptionByDistance { get; }
    public double TotalAverageConsumptionByDistance { get; }
    public int EstimatedRange => onBoardComputer.EstimatedRange;

    public OnBoardComputerDisplay(OnBoardComputer onBoardComputer)
    {
        this.onBoardComputer = onBoardComputer;
    }

    public void TripReset()
    {
        onBoardComputer.TripReset();
    }

    public void TotalReset()
    {
        onBoardComputer.TotalReset();
    }
}