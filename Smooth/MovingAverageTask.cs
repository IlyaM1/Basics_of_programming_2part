using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var sum = 0.0;
        var window = new Queue<double>();

        foreach (var point in data)
        {
            if (window.Count == 0)
            {
                window.Enqueue(point.OriginalY);
                sum += point.OriginalY;
                yield return point.WithAvgSmoothedY(sum / window.Count); ;
            }
            else
            {
                var value = point.OriginalY;

                window.Enqueue(value);
                sum += value;

                if (window.Count > windowWidth)
                    sum -= window.Dequeue();

                yield return point.WithAvgSmoothedY(sum / window.Count);
            }
        }
    }
}