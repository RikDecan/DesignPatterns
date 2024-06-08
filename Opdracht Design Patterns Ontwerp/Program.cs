using System;
using System.Collections.Generic;

// Memento Pattern: Memento class to store the state of the WeatherForecast
class WeatherForecastMemento
{
    public string Description { get; }
    public double DayTemperature { get; }
    public double NightTemperature { get; }
    public double Humidity { get; }

    public WeatherForecastMemento(string description, double dayTemperature, double nightTemperature, double humidity)
    {
        Description = description;
        DayTemperature = dayTemperature;
        NightTemperature = nightTemperature;
        Humidity = humidity;
    }
}



// Originator class which creates and restores the memento
class WeatherForecast
{
    public string Description { get; set; }
    public double DayTemperature { get; set; }
    public double NightTemperature { get; set; }
    public double Humidity { get; set; }

    public WeatherForecastMemento Save()
    {
        return new WeatherForecastMemento(Description, DayTemperature, NightTemperature, Humidity);
    }

    public void Restore(WeatherForecastMemento memento)
    {
        Description = memento.Description;
        DayTemperature = memento.DayTemperature;
        NightTemperature = memento.NightTemperature;
        Humidity = memento.Humidity;
    }
}

// Observer Pattern: Observer interface
interface IWeatherObserver
{
    void Update(double temperature);
}

// Observer Pattern: Concrete Observer
class TemperatureAlert : IWeatherObserver
{
    public void Update(double temperature)
    {
        if (temperature > 15)
        {
            Console.WriteLine("Temperature is predicted to be over 15 degrees!");
        }
    }
}

// Strategy Pattern: Strategy interface
interface IWeatherForecastStrategy
{
    void Forecast(WeatherForecast forecast);
}

// Strategy Pattern: Concrete Strategy 1
class SimpleForecastStrategy : IWeatherForecastStrategy
{
    public void Forecast(WeatherForecast forecast)
    {
        Console.WriteLine($"Simple Weather Forecast: {forecast.Description}, Day Temp: {forecast.DayTemperature}, Night Temp: {forecast.NightTemperature}, Humidity: {forecast.Humidity}");
    }
}

// Strategy Pattern: Concrete Strategy 2
class DetailedForecastStrategy : IWeatherForecastStrategy
{
    public void Forecast(WeatherForecast forecast)
    {
        Console.WriteLine($"Detailed Weather Forecast: {forecast.Description}, Day Temp: {forecast.DayTemperature}, Night Temp: {forecast.NightTemperature}, Humidity: {forecast.Humidity}");
    }
}

// Prototype Pattern: Concrete Prototype
class WeatherForecastPrototype
{
    public string Description { get; set; }
    public double DayTemperature { get; set; }
    public double NightTemperature { get; set; }
    public double Humidity { get; set; }

    public WeatherForecastPrototype Clone()
    {
        return new WeatherForecastPrototype
        {
            Description = this.Description,
            DayTemperature = this.DayTemperature,
            NightTemperature = this.NightTemperature,
            Humidity = this.Humidity
        };
    }
}

// Decorator Pattern: Component interface
interface IWeatherTemperature
{
    string Display();
}

// Decorator Pattern: Concrete Component
class CelsiusTemperature : IWeatherTemperature
{
    private readonly double _temperature;

    public CelsiusTemperature(double temperature)
    {
        _temperature = temperature;
    }

    public string Display()
    {
        return $"{_temperature}°C";
    }
}

// Decorator Pattern: Concrete Decorator
class FahrenheitTemperatureDecorator : IWeatherTemperature
{
    private readonly IWeatherTemperature _temperatureComponent;

    public FahrenheitTemperatureDecorator(IWeatherTemperature temperatureComponent)
    {
        _temperatureComponent = temperatureComponent;
    }

    public string Display()
    {
        return $"{ConvertToFahrenheit(_temperatureComponent.Display())}°F";
    }

    private double ConvertToFahrenheit(string celsius)
    {
        double celsiusValue = double.Parse(celsius.TrimEnd('°', 'C'));
        return (celsiusValue * 9 / 5) + 32;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a WeatherForecast instance
        WeatherForecast weatherForecast = new WeatherForecast();

        // Create a SimpleForecastStrategy instance
        SimpleForecastStrategy simpleStrategy = new SimpleForecastStrategy();

        // Create a DetailedForecastStrategy instance
        DetailedForecastStrategy detailedStrategy = new DetailedForecastStrategy();

        // Set the initial weather forecast
        weatherForecast.Description = "Sunny";
        weatherForecast.DayTemperature = 20;
        weatherForecast.NightTemperature = 10;
        weatherForecast.Humidity = 60;

        // Create a list of observers
        var observers = new List<IWeatherObserver>();

        // Add TemperatureAlert observer
        observers.Add(new TemperatureAlert());

        // Forecast using SimpleForecastStrategy
        simpleStrategy.Forecast(weatherForecast);

        // Notify observers
        foreach (var observer in observers)
        {
            observer.Update(weatherForecast.DayTemperature);
        }

        // Create a prototype of the weather forecast
        var prototype = new WeatherForecastPrototype
        {
            Description = weatherForecast.Description,
            DayTemperature = weatherForecast.DayTemperature,
            NightTemperature = weatherForecast.NightTemperature,
            Humidity = weatherForecast.Humidity
        };

        // Decorate the temperature display with CelsiusTemperature
        var celsiusTemperature = new CelsiusTemperature(weatherForecast.DayTemperature);
        Console.WriteLine($"Temperature: {celsiusTemperature.Display()}");

        // Decorate the temperature display with FahrenheitTemperatureDecorator
        var fahrenheitTemperature = new FahrenheitTemperatureDecorator(celsiusTemperature);
        Console.WriteLine($"Temperature: {fahrenheitTemperature.Display()}");

        // Save the current state of the weather forecast
        var memento = weatherForecast.Save();

        // Change the weather forecast
        weatherForecast.Description = "Cloudy";
        weatherForecast.DayTemperature = 18;
        weatherForecast.NightTemperature = 8;
        weatherForecast.Humidity = 70;

        // Forecast using DetailedForecastStrategy
        detailedStrategy.Forecast(weatherForecast);

        // Notify observers
        foreach (var observer in observers)
        {
            observer.Update(weatherForecast.DayTemperature);
        }

        // Restore the previous state of the weather forecast
        weatherForecast.Restore(memento);

        // Forecast using SimpleForecastStrategy after restoration
        simpleStrategy.Forecast(weatherForecast);

        // Notify observers after restoration
        foreach (var observer in observers)
        {
            observer.Update(weatherForecast.DayTemperature);
        }
    }
}
