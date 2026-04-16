using System;

public class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int minutes, int laps)
        : base(date, minutes)
    {
        _laps = laps;
    }

    // Distance in miles: laps * 50 meters / 1000 (to km) * 0.62 (to miles)
    public override double GetDistance() => _laps * 50.0 / 1000.0 * 0.62;

    public override double GetSpeed() => (GetDistance() / GetMinutes()) * 60;

    public override double GetPace() => GetMinutes() / GetDistance();
}
