using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace Clones;

public class Node<T>
{
    public Node(T data)
    {
        Data = data;
    }
    public T Data { get; set; }
    public Node<T> Next { get; set; }
}

public class NodeStack<T>
{
    Node<T> head;
    int count;

    public bool IsEmpty
    {
        get { return count == 0; }
    }
    public int Count
    {
        get { return count; }
    }

    public void Push(T item)
    {
        Node<T> node = new Node<T>(item);
        node.Next = head;
        head = node;
        count++;
    }
    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Стек пуст");
        Node<T> temp = head;
        head = head.Next;
        count--;
        return temp.Data;
    }
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Стек пуст");
        return head.Data;
    }
}

public class Clone
{
    public Stack<int> LessonsStack { get; set; }
    public Stack<int> RollbackStack { get; set; }
    unsafe public Clone CopyClone()
    {
        return new Clone
        {
            LessonsStack = new Stack<int>(LessonsStack),
            RollbackStack = new Stack<int>(RollbackStack)
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
            new Clone() {LessonsStack= new Stack<int>(), RollbackStack= new Stack<int>() }
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
                currentClone.RollbackStack = new Stack<int>();
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