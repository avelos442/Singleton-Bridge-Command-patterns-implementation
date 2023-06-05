using System;
using System.Collections.Generic;

// Singleton
public class DeviceManager
{
    private static DeviceManager instance;
    private Dictionary<string, Device> devices;

    private DeviceManager()
    {
        devices = new Dictionary<string, Device>();
    }

    public static DeviceManager GetInstance()
    {
        if (instance == null)
        {
            instance = new DeviceManager();
        }
        return instance;
    }

    public void AddDevice(string deviceId, Device device)
    {
        devices.Add(deviceId, device);
    }

    public void RemoveDevice(string deviceId)
    {
        devices.Remove(deviceId);
    }

    public Device GetDevice(string deviceId)
    {
        if (devices.TryGetValue(deviceId, out Device device))
        {
            return device;
        }
        return null;
    }
}

// Command 
public interface ICommand
{
    void Execute();
}

public class TurnOnCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Command executed: Turn On");
    }
}

public class TurnOffCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Command executed: Turn Off");
    }
}

// Bridge
public abstract class Device
{
    protected IDeviceImplementation implementation;
    protected ICommand onCommand;
    protected ICommand offCommand;

    public Device(IDeviceImplementation implementation)
    {
        this.implementation = implementation;
    }

    public void SetOnCommand(ICommand onCommand)
    {
        this.onCommand = onCommand;
    }

    public void SetOffCommand(ICommand offCommand)
    {
        this.offCommand = offCommand;
    }

    public void TurnOn()
    {
        onCommand.Execute();
    }

    public void TurnOff()
    {
        offCommand.Execute();
    }
}

public interface IDeviceImplementation
{
    void TurnOn();
    void TurnOff();
}

public class LightDevice : Device
{
    public LightDevice(IDeviceImplementation implementation) : base(implementation)
    {
    }
}

public class LightDeviceImplementation : IDeviceImplementation
{
    public void TurnOn()
    {
        Console.WriteLine("Light device turned on.");
    }

    public void TurnOff()
    {
        Console.WriteLine("Light device turned off.");
    }
}

public class FanDevice : Device
{
    public FanDevice(IDeviceImplementation implementation) : base(implementation)
    {
    }
}

public class FanDeviceImplementation : IDeviceImplementation
{
    public void TurnOn()
    {
        Console.WriteLine("Fan device turned on.");
    }

    public void TurnOff()
    {
        Console.WriteLine("Fan device turned off.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        // Create device implementations
        IDeviceImplementation lightImplementation = new LightDeviceImplementation();
        IDeviceImplementation fanImplementation = new FanDeviceImplementation();

        // Create devices
        Device lightDevice = new LightDevice(lightImplementation);
        Device fanDevice = new FanDevice(fanImplementation);

        // Add devices to the manager
        DeviceManager deviceManager = DeviceManager.GetInstance();
        deviceManager.AddDevice("Light1", lightDevice);
        deviceManager.AddDevice("Fan1", fanDevice);

        // Create and set commands for devices
        ICommand turnOnLightCommand = new TurnOnCommand();
        ICommand turnOffLightCommand = new TurnOffCommand();
        ICommand turnOnFanCommand = new TurnOnCommand();
        ICommand turnOffFanCommand = new TurnOffCommand();

        lightDevice.SetOnCommand(turnOnLightCommand);
        lightDevice.SetOffCommand(turnOffLightCommand);

        fanDevice.SetOnCommand(turnOnFanCommand);
        fanDevice.SetOffCommand(turnOffFanCommand);

        // Execute commands
        lightDevice.TurnOn();
        fanDevice.TurnOff();
    }
}
