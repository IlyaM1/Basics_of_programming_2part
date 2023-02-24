using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var dataEnumerator = data.GetEnumerator();

        if (windowWidth == 1)
            while (dataEnumerator.MoveNext())
                yield return dataEnumerator.Current.WithMaxY(dataEnumerator.Current.OriginalY);

        var isAtLeastOneElementInData = dataEnumerator.MoveNext();

        if (!isAtLeastOneElementInData)
            yield break;


        var currentPoint = dataEnumerator.Current;

        var maxNumbersList = new LinkedList<double>();
        var windowNumbersList = new LinkedList<double>();

        windowNumbersList.AddLast(currentPoint.OriginalY);
        maxNumbersList.AddLast(currentPoint.OriginalY);

        yield return currentPoint.WithMaxY(currentPoint.OriginalY);


        while (dataEnumerator.MoveNext())
        {
            var point = dataEnumerator.Current;
            var value = point.OriginalY;

            AddValueAndDeleteFirstELementIfNotInWindow(windowNumbersList, maxNumbersList, value, windowWidth);
            DeleteElementsWhileValueBiggerThanThem(maxNumbersList, value);
            maxNumbersList.AddLast(value);

            yield return point.WithMaxY(maxNumbersList.First.Value);
        }
    }

    private static void AddValueAndDeleteFirstELementIfNotInWindow(LinkedList<double> window,LinkedList<double> list, double value, int windowWidth)
    {
        window.AddLast(value);
        if (window.Count > windowWidth)
        {
            var deletedValue = window.First.Value;
            window.RemoveFirst();
            if (deletedValue == list.First.Value)
                list.RemoveFirst();
        }
    }

    private static void DeleteElementsWhileValueBiggerThanThem(LinkedList<double> list, double value)
    {
        var lastNumb = list.Last.Value;

        while (lastNumb < value && list.Count > 0)
        {
            list.RemoveLast();
            if (list.Count > 0)
                lastNumb = list.Last.Value;
        }
    }
}