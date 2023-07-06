namespace ConstructingACar_3_v2;

public class OnBoardComputer : IOnBoardComputer
{
    private const double ONE_KMH_IN_MPERS = 0.278;

    private DrivingProcessor drivingProcessor;
    private FuelTank fuelTank;
    private int tripSpeedSum = 0;
    private int totalSpeedSum = 0;
    private double tripConsumptionSum = 0;
    private double totalConsumptionSum = 0;

    public int TripRealTime { get; set; } = 0;
    public int TripDrivingTime { get; set; } = 0;
    public int TripDrivenDistance { get; set; } = 0;
    public double TripDrivenDistanceDouble { get; set; } = 0;

    public int TotalRealTime { get; set; } = 0;
    public int TotalDrivingTime { get; set; } = 0;
    public int TotalDrivenDistance { get; set; } = 0;
    public double TotalDrivenDistanceDouble { get; set; } = 0;

    public double TripAverageSpeed => (double)tripSpeedSum / TripDrivingTime;
    public double TotalAverageSpeed => (double)totalSpeedSum / TotalDrivingTime;

    public int ActualSpeed => drivingProcessor.ActualSpeed;
    public double ActualConsumptionByTime => drivingProcessor.ActualConsumption;

    public double ActualConsumptionByDistance
    {
        get
        {
            if (drivingProcessor.ActualSpeed > 0)
            {
                // Fuel consumption = Fuel used / Distance traveled
                // 0,0014/0,278*1000
                //  double kmTraveled = Math.Round(drivingProcessor.ActualSpeed * ONE_KMH_IN_MPERS) / 1000d;
                //   return drivingProcessor.PreviousConsumption / kmTraveled * 100;
                return drivingProcessor.ActualConsumption / drivingProcessor.ActualSpeed * 3600 * 100;
            }

            return double.NaN;
        }
    }

    public double TripAverageConsumptionByTime
    {
        get
        {
            if (tripConsumptionSum == 0 || TripRealTime == 0) return 0;
            return tripConsumptionSum / TripRealTime;
        }
    }

    public double TotalAverageConsumptionByTime
    {
        get
        {
            if (totalConsumptionSum == 0 || TotalRealTime == 0) return 0;
            return totalConsumptionSum / TotalRealTime;
        }
    }

    public double TripAverageConsumptionByDistance { get; set; }
    public double TotalAverageConsumptionByDistance { get; set; }

    public int EstimatedRange
    {
        get
        {
            double kmPerLiter = ActualConsumptionByDistance / 100;
            return (int)Math.Round(fuelTank.FillLevel / kmPerLiter);
        }
    }

    public OnBoardComputer(DrivingProcessor drivingProcessor, FuelTank fuelTank)
    {
        this.fuelTank = fuelTank;
        this.drivingProcessor = drivingProcessor;
        this.drivingProcessor.engineStartEvent += this.TripReset;
        this.drivingProcessor.engineStartEvent += this.ElapseSecond;

        this.drivingProcessor.DrivingEvent += this.ElapseSecond;
    }

    public void ElapseSecond()
    {
        if (!drivingProcessor.EngineIsRunning) return;

        TripRealTime++;
        TotalRealTime++;

        tripConsumptionSum += drivingProcessor.ActualConsumption;
        totalConsumptionSum += drivingProcessor.ActualConsumption;

        if (drivingProcessor.ActualSpeed > 0)
        {
            TripDrivingTime++;
            TotalDrivingTime++;

            tripSpeedSum += drivingProcessor.ActualSpeed;
            totalSpeedSum += drivingProcessor.ActualSpeed;

            double drivenDistance = drivingProcessor.ActualSpeed * 0.000278;
            TripDrivenDistanceDouble += drivenDistance;
            TotalDrivenDistanceDouble += drivenDistance;

            TripAverageConsumptionByDistance -= ((TripAverageConsumptionByDistance - ActualConsumptionByDistance) / TripDrivingTime);
            TotalAverageConsumptionByDistance -= ((TotalAverageConsumptionByDistance - ActualConsumptionByDistance) / TotalDrivingTime);
        }
    }

    public void TripReset()
    {
        TripRealTime = 0;
        TripDrivingTime = 0;

        tripSpeedSum = 0;
        TripDrivenDistance = 0;
        TripDrivenDistanceDouble = 0;

        tripConsumptionSum = 0;

        TripAverageConsumptionByDistance = 0;
    }

    public void TotalReset()
    {
        TotalRealTime = 0;
        TotalDrivingTime = 0;

        totalSpeedSum = 0;
        TotalDrivenDistanceDouble = 0;

        totalConsumptionSum = 0;

        TotalAverageConsumptionByDistance = 0;
    }
}