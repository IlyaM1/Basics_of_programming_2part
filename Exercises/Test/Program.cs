using System.Diagnostics;

namespace Program
{
    public static class IsLazyCheckClass
    {
        private static int CollectionElementsLimit => 100;
        private static int IterationsCheckLimit => 100;

        private static bool IsLazyImmutable<T>(IEnumerable<T> collection)
        {
            var firstEnumerator = collection.GetEnumerator();
            var secondEnumerator = collection.GetEnumerator();

            firstEnumerator.MoveNext();
            secondEnumerator.MoveNext();

            return !object.ReferenceEquals(firstEnumerator.Current, secondEnumerator.Current);
        }

        private static double GetTime<T>(IEnumerable<T> collection)
        {
            var sw = new Stopwatch();
            sw.Start();
            var enumerator = collection.GetEnumerator();

            var moveNextNumber = 0;
            while (enumerator.MoveNext())
            {
                moveNextNumber++;
                if (moveNextNumber >= CollectionElementsLimit)
                    break;
            }

            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }

        private static List<T> GetFilledList<T>(IEnumerable<T> collection)
        {
            var collectionElementsList = new List<T>();
            var enumerator = collection.GetEnumerator();
            var moveNextNumber = 0;
            while (enumerator.MoveNext())
            {
                collectionElementsList.Add(enumerator.Current);
                moveNextNumber++;
                if (moveNextNumber >= CollectionElementsLimit)
                    break;
            }
            return collectionElementsList;
        }

        private static bool IsLazyMutable<T>(IEnumerable<T> collection)
        {
            var collectionElementsList = GetFilledList<T>(collection);

            var listTotalTime = 0.0;
            var enumeratorTotalTime = 0.0;

            for (int i = 0; i < IterationsCheckLimit; i++)
            {
                listTotalTime += GetTime<T>(collectionElementsList);
                enumeratorTotalTime += GetTime(collection);
            }

            return enumeratorTotalTime > listTotalTime;
        }

        public static bool IsLazy<T>(IEnumerable<T> collection)
        {
            var type = typeof(T);

            if (type is int || type is double || type is double || type is char || type is bool || type is string || type.IsArray)
                return IsLazyImmutable(collection);

            return IsLazyMutable(collection);
        }
    }

    public class LazyCollectionsGenerators
    {
        public static IEnumerable<int> ReturnLazyCollection(IEnumerable<int> collection)
        {
            var enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var smt = enumerator.Current;

                smt *= 2;

                yield return smt;
            }
        }

        public static IEnumerable<string[]> GenerateLazyClassCollection()
        {
            var i = 0;
            while (true)
            {
                string[] a = new string[1];
                a[0] = (i++).ToString();
                yield return a;
            }
        }

        public static IEnumerable<int> GenerateLazyStructCollection()
        {
            var i = 0;
            while (true)
                yield return i++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var intArray = new int[] { 1, 2, 22, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 32, 542, 5, 3245, 34, 6467, 5, 46, 46, 456, 234, 5, 345, 325, 342, 54, 2453, 2453, 25, 325, 543, 34 };
            var strArray = new string[] { "12", "123", "1234" };
            Console.WriteLine(IsLazyCheckClass.IsLazy(intArray));
            Console.WriteLine();
            Console.WriteLine(IsLazyCheckClass.IsLazy(strArray));
            Console.WriteLine();
            Console.WriteLine(IsLazyCheckClass.IsLazy(LazyCollectionsGenerators.GenerateLazyStructCollection()));
            Console.WriteLine();
            Console.WriteLine(IsLazyCheckClass.IsLazy(LazyCollectionsGenerators.GenerateLazyClassCollection()));
            Console.WriteLine();
            Console.WriteLine(IsLazyCheckClass.IsLazy(LazyCollectionsGenerators.ReturnLazyCollection(LazyCollectionsGenerators.GenerateLazyStructCollection())));
        }
    }
}