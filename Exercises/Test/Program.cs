using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Action>();
            var numbers = new List<int> { 0, 1, 2, 3, 4 };

            foreach (var number in numbers)
            {
                list.Add(() => Console.WriteLine(number));
            }

            foreach (var func in list)
                func();
        }
    }
}