namespace MiniCraft.Gfx
{
    public class Color
    {
        public static int White => Get(-1, 555, 555, 555);
        public static int Black => Get(-1, 000, 000, 000);
        public static int Yello => Get(-1, 5, 5, 550);

        public static int Get(int a, int b, int c, int d)
        {
            return (Get(d) << 24) + (Get(c) << 16) + (Get(b) << 8) + (Get(a));
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
