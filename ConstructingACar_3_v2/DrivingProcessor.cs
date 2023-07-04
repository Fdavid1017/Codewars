namespace ConstructingACar_3_v2;
public class DrivingProcessor : IDrivingProcessor
{
    private const int MAX_BREAKING = 10;
    private const int MAX_SPEED = 250;

    private int maxAcceleration = 10;
    private int actualSpeed = 0;

    public double ActualConsumption { get; }

    public int ActualSpeed
    {
        get => actualSpeed;
        set
        {
            actualSpeed = Math.Clamp(value, 0, MAX_SPEED);
        }
    }

    public int MaxAcceleration
    {
        get => maxAcceleration;
        set => maxAcceleration = Math.Clamp(value, 5, 20);
    }

    public DrivingProcessor()
    {
    }

    public DrivingProcessor(int maxAcceleration)
    {
        MaxAcceleration = maxAcceleration;
    }
    public void EngineStart()
    {
        throw new NotImplementedException();
    }

    public void EngineStop()
    {
        throw new NotImplementedException();
    }

    public void IncreaseSpeedTo(int speed)
    {
        ActualSpeed = speed;
    }

    public void ReduceSpeed(int speed)
    {
        speed = Math.Clamp(speed, 0, MAX_BREAKING);
        ActualSpeed -= speed;
    }

    public void AccelerateTo(int speed)
    {
        speed -= ActualSpeed;
        speed = Math.Clamp(speed, 0, MaxAcceleration);
        IncreaseSpeedTo(ActualSpeed + speed);
    }
}