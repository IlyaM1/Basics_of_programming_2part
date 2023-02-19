using System;

namespace LimitedSizeStack;

public class StackItem<T>
{
    public T Value { get; set; }
    public StackItem<T> Next { get; set; }
    public StackItem<T> Previous { get; set; }
}

public class LimitedSizeStack<T>
{
    public int ElementsLimit { get; private set; }
    public bool IsEmpty { get { return Count == 0; } }
    private int _length;

    StackItem<T> _top;
    StackItem<T> _bottom;

    public LimitedSizeStack(int undoLimit)
    {
        ElementsLimit = undoLimit;
    }

    public void Push(T item)
    {
        if (ElementsLimit == 0)
            return;

        if (IsEmpty || ElementsLimit == 1)
        {
            _bottom = _top = new StackItem<T> { Value = item, Next = null, Previous = null };
            Count = 1;
        }
        else
        {
            if (Count == ElementsLimit)
            {
                _bottom = _bottom.Next;
                _bottom.Previous = null;
            }
            else
                Count += 1;

            _top = new StackItem<T> { Value = item, Next = null, Previous = _top };
            _top.Previous.Next = _top;
        }
    }

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();

        var result = _top.Value;
        if (Count == 1)
            _top = _bottom = null;
        else
            _top = _top.Previous;

        Count -= 1;
        return result;
    }

    public int Count
    {
        get { return _length; }
        set
        {
            if (value < 0) throw new ArgumentException();
            _length = value;
        }
    }
}