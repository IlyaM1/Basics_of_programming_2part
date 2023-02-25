using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	private LimitedSizeStack<TItem> UndoActionsStack { get; set; }
	private LimitedSizeStack<int> RemovedItemsIndexes { get; set; }
        
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoActionsStack = new LimitedSizeStack<TItem>(undoLimit);
        RemovedItemsIndexes = new LimitedSizeStack<int>(undoLimit);
    }

	public void AddItem(TItem item)
	{
        UndoActionsStack.Push(item);
        Items.Add(item);
	}

	public void RemoveItem(int index)
	{
        UndoActionsStack.Push(Items[index]);
        Items.RemoveAt(index);
		RemovedItemsIndexes.Push(index);
    }

	public bool CanUndo()
	{
		return UndoActionsStack.Count > 0;
	}

	public void Undo()
	{
		var lastActionItem = UndoActionsStack.Pop();
		if (Items.Contains(lastActionItem))
            Items.RemoveAt(Items.IndexOf(lastActionItem));
		else
            Items.Insert(RemovedItemsIndexes.Pop(), lastActionItem);
	}
}