using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;

namespace MiniRealms.Engine
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

        public static void RemoveAll<T>(this List<T> list, IEnumerable<T> other)
        {
            foreach (var item in other)
                list.Remove(item);
        }

        public static int Length(this string s) => s.Length;

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1)).Take(pageSize);
        }

        public static void Save(this Texture2D texture, ImageFormat imageFormat, string filename)
        {
            int width = texture.Bounds.Width;
            int height = texture.Bounds.Height;

            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                Rectangle rect = new Rectangle(0, 0, width, height);
                byte[] textureData = new byte[4 * width * height];

                texture.GetData(textureData);
                for (int i = 0; i < textureData.Length; i += 4)
                {
                    var blue = textureData[i];
                    textureData[i] = textureData[i + 2];
                    textureData[i + 2] = blue;
                }
                var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                var safePtr = bitmapData.Scan0;
                Marshal.Copy(textureData, 0, safePtr, textureData.Length);
                bitmap.UnlockBits(bitmapData);
                bitmap.Save(filename, imageFormat);
            }
        }
    }
}
