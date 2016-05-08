using System;
using System.Collections.Generic;

namespace MiniRealms
{
    public static class Extensions
    {
        public static float NextFloat(this Random random) => (float)random.NextDouble();

        public static float NextGaussian(this Random random) => (float)random.NextDouble();

        public static int NextInt(this Random random, int max) => random.Next(max);

        public static bool Nextbool(this Random random) => random.NextDouble() >= 0.5;

        public static int Size<T>(this List<T> list) => list.Count;

        public static T Get<T>(this List<T> list, int index) => list[index];

        public static void Add<T>(this List<T> list, int index, T item) => list?.Insert(index, item);

        public static void Add<T>(this List<T> list, T item) => list.Add(item);

        public static void Remove<T>(this List<T> list, T item) => list.Remove(item);

        public static T Remove<T>(this List<T> list, int index)
        {
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static void Clear<T>(this List<T> list) => list.Clear();

        public static void AddAll<T>(this List<T> list, IEnumerable<T> items) => list.AddRange(items);

        public static void RemoveAll<T>(this List<T> list, List<T> other)
        {
            foreach (var item in other)
                list.Remove(item);
        }

        public static int Length(this string s) => s.Length;
    }
}
