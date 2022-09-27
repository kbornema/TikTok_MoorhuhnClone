using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MusicComboService
{
    private float _timer;
    private float _onHitSecond;

    public MusicComboService(float bpm)
    {
        _timer = 0;
        _onHitSecond = 60.0f / bpm;
    }

    /// <summary>
    /// Returns a value between 1.0f - 0.0f.
    /// </summary>
    /// <returns> 1.0f means perfect hit. 0.0f means complete miss</returns>
    public float GetHitAccuracy()
    {
        var hitSecond = _timer % _onHitSecond;
        var distanceToPerfectHit = MathF.Max(_onHitSecond - hitSecond, hitSecond);
        distanceToPerfectHit *= 1 / _onHitSecond;
        distanceToPerfectHit -= 0.5f;

        return distanceToPerfectHit * 2;
    }

    /// <summary>
    /// Resets timer to zero
    /// </summary>
    public void Reset()
    {
        _timer = 0.0f;
    }

    /// <summary>
    /// Updates the time
    /// </summary>
    /// <param name="updateTime"></param>
    public void Update(float updateTime)
    {
        _timer += updateTime;
    }
}
