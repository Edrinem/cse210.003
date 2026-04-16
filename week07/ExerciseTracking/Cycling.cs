using System;

public class Cycling : Activity
{
    private double _speed; // in mph

    public Cycling(DateTime date, int minutes, double speed)
        : base(date, minutes)
    {
        _speed = speed;
    }

    // distance = speed * (minutes / 60)
    public override double GetDistance() => _speed * (GetMinutes() / 60.0);

    public override double GetSpeed() => _speed;

    // pace = 60 / speed
    public override double GetPace() => 60.0 / _speed;
}
