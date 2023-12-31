﻿namespace ConstructingACar_3_v2;

public delegate void EngineStartEventHandler();
public delegate void DrivingEventHandler();

public class DrivingProcessor : IDrivingProcessor
{
    private const int MAX_BREAKING = 10;
    private const int MAX_SPEED = 250;

    private int maxAcceleration = 10;
    private int actualSpeed = 0;
    private bool noFuelUsage = false;

    public event EngineStartEventHandler engineStartEvent;
    public event DrivingEventHandler DrivingEvent;

    public double ActualConsumption
    {
        get
        {
            if (noFuelUsage) return 0;

            switch (ActualSpeed)
            {
                case < 1:
                    return Engine.IDLE_FUEL_CONSUMPTION_PER_SEC;
                case >= 1 and <= 60:
                    return 0.0020;
                case >= 61 and <= 100:
                    return 0.0014;
                case >= 101 and <= 140:
                    return 0.0020;
                case >= 141 and <= 200:
                    return 0.0025;
                case >= 201 and <= 250:
                    return 0.0030;
                default:
                    return 0.0020;
            }
        }
    }
    
    public bool EngineIsRunning
    {
        get;
        set;
    }

    public int ActualSpeed
    {
        get => actualSpeed;
        set
        {
            actualSpeed = Math.Clamp(value, 0, MAX_SPEED);
            DrivingEvent();
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
        EngineIsRunning = true;
        noFuelUsage = true;
        engineStartEvent();
    }

    public void EngineStop()
    {
        EngineIsRunning = false;
    }

    public void IncreaseSpeedTo(int speed)
    {
        ActualSpeed = speed;
    }

    public void ReduceSpeed(int speed)
    {
        noFuelUsage = true;
        speed = Math.Clamp(speed, 0, MAX_BREAKING);
        ActualSpeed -= speed;
    }

    public void AccelerateTo(int speed)
    {
        noFuelUsage = false;
        speed -= ActualSpeed;
        speed = Math.Clamp(speed, 0, MaxAcceleration);
        IncreaseSpeedTo(ActualSpeed + speed);
    }

    public void RunningIdle()
    {
        noFuelUsage = false;
        DrivingEvent();
    }
}