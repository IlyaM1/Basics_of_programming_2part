using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Program
{
    enum Brackets
    {
        Left,
        Right
    }

    class Program
    {
        private static int FindOppositeBracket(string s, int index, Brackets bracket)
        {
            var bracketsStack = new Stack<int>();
            bracketsStack.Push(index);

            var oppositeBracketIndex = 0;

            var i = (bracket == Brackets.Left) ? index + 1 : index - 1;
            while (bracketsStack.Count > 0)
            {
                if (s[i] == ((bracket == Brackets.Left) ? '[' : ']'))
                    bracketsStack.Push(i);
                else if (s[i] == ((bracket == Brackets.Left) ? ']' : '['))
                {
                    oppositeBracketIndex = i;
                    bracketsStack.Pop();
                }
                    
                i = (bracket == Brackets.Left) ? i + 1 : i - 1;
            }
            return oppositeBracketIndex;
        }

        static void Main(string[] args)
        {
            var testString = "++[+]";
            Console.WriteLine(FindOppositeBracket(testString, 4, Brackets.Right));
        }
    }
}