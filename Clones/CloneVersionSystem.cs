using System;
using System.Collections.Generic;

namespace Clones;

public class Node<T>
{
    public Node(T data)
    {
        Data = data;
    }

    public Node(T data, Node<T> next) : this(data)
    {
        Next = next;
    }

    public T Data { get; set; }
    public Node<T> Next { get; set; }
}

public class NodeStack<T>
{
    Node<T> head;
    Node<T> tail;
    int _count;

    public bool IsEmpty => _count == 0;
    public int Count => _count;

    public void Push(T item)
    {
        Node<T> node = new Node<T>(item);
        Push(node);
    }

    public void Push(Node<T> node)
    {
        node.Next = head;
        head = node;
        if (tail == null)
            tail = node;
        _count++;
    }

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        Node<T> temp = head;
        head = head.Next;
        if (Count == 1)
            tail = null;
        _count--;
        return temp.Data;
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        return head.Data;
    }

    public NodeStack<T> CopyNodeStack()
    {
        var newNodeStack = new NodeStack<T>();
        var headNode = new Node<T>(head.Data, head.Next);
        var tailNode = new Node<T>(tail.Data, tail.Next);
        newNodeStack.head = headNode;
        newNodeStack.tail = tailNode;
        newNodeStack._count = Count;

        return newNodeStack;
    }
}

public class Clone
{
    public NodeStack<int> LessonsStack { get; set; }
    public NodeStack<int> RollbackStack { get; set; }
    
    public Clone()
    {
        LessonsStack = new NodeStack<int>();
        RollbackStack = new NodeStack<int>();
    }

    public Clone CopyClone()
    {
        var lessonsStackCopy = (LessonsStack is null || LessonsStack.IsEmpty) ? new NodeStack<int>() : LessonsStack.CopyNodeStack();
        var rollbackStackCopy = (RollbackStack is null || RollbackStack.IsEmpty) ? new NodeStack<int>() : RollbackStack.CopyNodeStack();

        return new Clone
        {
            LessonsStack = lessonsStackCopy,
            RollbackStack = rollbackStackCopy
        };
    }
}

public class CloneVersionSystem : ICloneVersionSystem
{
    public List<Clone> Clones { get; set; }

    public CloneVersionSystem()
    {
        Clones = new List<Clone>
        {
            new Clone()
        };
    }

    public string Execute(string query)
    {
        var splittedQuery = query.Split(' ');
        var command = splittedQuery[0];
        var cloneIndex = int.Parse(splittedQuery[1]) - 1;
        var currentClone = Clones[cloneIndex];
        switch (command)
        {
            case "learn":
                var program = int.Parse(splittedQuery[2]);
                currentClone.LessonsStack.Push(program);
                currentClone.RollbackStack = new NodeStack<int>();
                return null;
            case "rollback":
                var deletedProgram = currentClone.LessonsStack.Pop();
                currentClone.RollbackStack.Push(deletedProgram);
                return null;
            case "relearn":
                currentClone.LessonsStack.Push(currentClone.RollbackStack.Pop());
                return null;
            case "clone":
                Clones.Add(currentClone.CopyClone());
                return null;
            case "check":
                if (currentClone.LessonsStack.Count == 0)
                    return "basic";
                return currentClone.LessonsStack.Peek().ToString();
        }
        return null;
    }
}