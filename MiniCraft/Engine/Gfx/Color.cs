namespace MiniRealms.Engine.Gfx
{
    public static class Color
    {
        public static int White => Get(-1, 555, 555, 555);
        public static int Black => Get(-1, 000, 000, 000);
        public static int Yellow => Get(-1, 5, 5, 550);
        public static int DarkGrey => Get(-1, 222, 222, 222);
        public static int Grey => Get(-1, 333, 333, 333);
        public static int Green => Get(-1, 10, 252, 050);

        public static int Get(int a, int b, int c, int d)
        {
            return (Get(d) << 24) + (Get(c) << 16) + (Get(b) << 8) + (Get(a));
        }

        /// <summary>
        /// Doesn't work yet so don't even try
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetHex(int a, int b, int c, int d)
        {
            return Get(a, b, c, d).ToString("X");
        }

        public static int Get(int d)
        {
            if (d < 0) return 255;
            int r = d/100%10;
            int g = d/10%10;
            int b = d%10;
            return r*36 + g*6 + b;
        }
    }
}
