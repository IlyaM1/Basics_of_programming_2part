namespace Test
{
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
            if (tail == null)
                tail = node;
            count++;
        }

        public void Push(Node<T> node)
        {
            node.Next = head;
            head = node;
            if (tail == null)
                tail = node;
            count++;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            Node<T> temp = head;
            head = head.Next;
            if (Count == 1)
                tail = null;
            count--;
            return temp.Data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            return head.Data;
        }

        public NodeStack<T> CopyNodeStack()
        {
            var newNodeStack = new NodeStack<T>();
            var headNode = new Node<T>(head.Data, head.Next);
            var tailNode = new Node<T>(tail.Data, tail.Next);
            newNodeStack.head = headNode;
            newNodeStack.tail = tailNode;
            newNodeStack.count = Count;

            return newNodeStack;
        }
    }

    internal class Program
    {
        public static void PrintNodeStack(NodeStack<int> nodeStack)
        {
            var leng = nodeStack.Count;
            for (var i = 0; i < leng; i++)
            {
                Console.WriteLine(nodeStack.Pop());
            }
        }

        static void Main(string[] args)
        {
            var stack1 = new NodeStack<int>();
            for (var i = 1; i <= 3; i++)
            {
                stack1.Push(i);
            }
            var stack2 = stack1.CopyNodeStack();
            PrintNodeStack(stack2);
        }
    }
}