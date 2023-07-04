namespace ConstructingACar_3_v2;

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