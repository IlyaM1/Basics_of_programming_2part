using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        var dataEnumerator = data.GetEnumerator();
        var isAtLeastOneElementInData = dataEnumerator.MoveNext();

        if (!isAtLeastOneElementInData)
            yield break;

        var previousElementSmoothedY = dataEnumerator.Current.OriginalY;
        yield return dataEnumerator.Current.WithExpSmoothedY(previousElementSmoothedY);

        while (dataEnumerator.MoveNext())
        {
            var point = dataEnumerator.Current;
            var smoothedPoint = point.WithExpSmoothedY(previousElementSmoothedY +
                alpha * (point.OriginalY - previousElementSmoothedY));
            yield return smoothedPoint;
            previousElementSmoothedY = smoothedPoint.ExpSmoothedY;
        }
    }
}