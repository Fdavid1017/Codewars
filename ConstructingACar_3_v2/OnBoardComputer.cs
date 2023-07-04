namespace ConstructingACar_3_v2;

public class OnBoardComputer : IOnBoardComputer
{
    private DrivingProcessor drivingProcessor;

    public int TripRealTime { get; }
    public int TripDrivingTime { get; }
    public int TripDrivenDistance { get; }
    public int TotalRealTime { get; }
    public int TotalDrivingTime { get; }
    public int TotalDrivenDistance { get; }
    public double TripAverageSpeed { get; }
    public double TotalAverageSpeed { get; }
    public int ActualSpeed { get; }
    public double ActualConsumptionByTime { get; }
    public double ActualConsumptionByDistance { get; }
    public double TripAverageConsumptionByTime { get; }
    public double TotalAverageConsumptionByTime { get; }
    public double TripAverageConsumptionByDistance { get; }
    public double TotalAverageConsumptionByDistance { get; }
    public int EstimatedRange { get; }

    public OnBoardComputer(DrivingProcessor drivingProcessor)
    {
        this.drivingProcessor = drivingProcessor;
    }

    public void ElapseSecond()
    {
        throw new NotImplementedException();
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