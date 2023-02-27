using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        var previousElementSmoothedY = double.NaN;

        foreach (var point in data)
        {
            if (double.IsNaN(previousElementSmoothedY))
            {
                previousElementSmoothedY = point.OriginalY;
                yield return point.WithExpSmoothedY(previousElementSmoothedY);
            }
            else
            {
                var smoothedPoint = point.WithExpSmoothedY(previousElementSmoothedY +
                    alpha * (point.OriginalY - previousElementSmoothedY));
                previousElementSmoothedY = smoothedPoint.ExpSmoothedY;
                yield return smoothedPoint;
            }
        }
    }
}