using UnityEngine;

public class Ticker
{
    private readonly float _timeDelay;
    private float _timer;

    public Ticker(float timeDelay)
    {
        _timeDelay = timeDelay;
        _timer = 0f;
    }

    public bool Tick()
    {
        if (_timer > 0f)
            _timer -= Time.deltaTime;
        else
        {
            _timer = _timeDelay;
            return true;
        }

        return false;
    }

    public void Reset() => _timer = 0f;

    public float GetProgress()
    {
        var current = _timer;
        return current > 0f
            ? (_timeDelay - current) / _timeDelay
            : 0f;
    }
}