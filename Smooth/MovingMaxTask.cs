using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var maxNumbersList = new LinkedList<double>();
        var windowNumbersList = new Queue<double>();

        foreach (var point in data)
        {
            var value = point.OriginalY;

            AddValueAndDeleteFirstELementIfNotInWindow(windowNumbersList, maxNumbersList, value, windowWidth);
            DeleteElementsWhileValueBiggerThanThem(maxNumbersList, value);
            maxNumbersList.AddLast(value);

            yield return point.WithMaxY(maxNumbersList.First.Value);
        }
    }

    private static void AddValueAndDeleteFirstELementIfNotInWindow
        (Queue<double> window,LinkedList<double> list, double value, int windowWidth)
    {
        window.Enqueue(value);
        if (window.Count > windowWidth)
        {
            var deletedValue = window.Dequeue();
            if (deletedValue == list.First.Value)
                list.RemoveFirst();
        }
    }

    private static void DeleteElementsWhileValueBiggerThanThem(LinkedList<double> list, double value)
    {
        if (list.Count == 0)
            return;

        var lastNumb = list.Last.Value;
        while (lastNumb < value && list.Count > 0)
        {
            list.RemoveLast();
            if (list.Count > 0)
                lastNumb = list.Last.Value;
        }
    }
}