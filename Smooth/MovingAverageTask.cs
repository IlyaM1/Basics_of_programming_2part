using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var dataEnumerator = data.GetEnumerator();
        var isAtLeastOneElementInData = dataEnumerator.MoveNext();

        if (!isAtLeastOneElementInData)
            yield break;

        var currentPoint = dataEnumerator.Current;
        var sum = currentPoint.OriginalY;
        var queue = new Queue<double>();
        queue.Enqueue(currentPoint.OriginalY);

        yield return currentPoint.WithAvgSmoothedY(currentPoint.OriginalY);

        while (dataEnumerator.MoveNext())
        {
            var point = dataEnumerator.Current;
            var value = point.OriginalY;

            queue.Enqueue(value);
            sum += value;

            if (queue.Count > windowWidth)
                sum -= queue.Dequeue();

            var newPoint = point.WithAvgSmoothedY(sum / queue.Count);
            yield return newPoint;
        }
    }
}