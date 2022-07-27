using UnityEngine;

public class DayCycle : MonoBehaviour
{
    private readonly float[] _nightColor = {
        51 / 255.0f,
        26 / 255.0f,
        15 / 255.0f,
    };
    private readonly float[] _dayColor = {
        110 / 255.0f,
        96 / 255.0f,
        72 / 255.0f,
    };

    private Color _backgroundColor;
    private float _timeOfDay = 600F;
    private sbyte direction = -1;

    private void Start()
    {
        _backgroundColor = Camera.main.backgroundColor;
        _backgroundColor.r = _dayColor[0];
        _backgroundColor.g = _dayColor[1];
        _backgroundColor.b = _dayColor[2];
        Camera.main.backgroundColor = _backgroundColor;
    }

    private void DayUpdate()
    {
        _backgroundColor.r += (_nightColor[0] - _dayColor[0]) / _timeOfDay * 2 * direction * Time.deltaTime;
        _backgroundColor.g += (_nightColor[1] - _dayColor[1]) / _timeOfDay * 2 * direction * Time.deltaTime;
        _backgroundColor.b += (_nightColor[2] - _dayColor[2]) / _timeOfDay * 2 * direction * Time.deltaTime;

        if(direction <= -1 && _backgroundColor.r >= _dayColor[0] && _backgroundColor.g >= _dayColor[1] && _backgroundColor.b >= _dayColor[2])
        {
            //Perfect Day
            _backgroundColor.r = _dayColor[0];
            _backgroundColor.g = _dayColor[1];
            _backgroundColor.b = _dayColor[2];
            direction = 1;
        }
        else if(direction >= 1 && _backgroundColor.r <= _nightColor[0] && _backgroundColor.g <= _nightColor[1] && _backgroundColor.b <= _nightColor[2])
        {
            //Perfect Night
            _backgroundColor.r = _nightColor[0];
            _backgroundColor.g = _nightColor[1];
            _backgroundColor.b = _nightColor[2];
            direction = -1;
        }

        Camera.main.backgroundColor = _backgroundColor;
    }

    private void Update()
    {
        DayUpdate();
    }
}
