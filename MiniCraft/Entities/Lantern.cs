using MiniCraft.Gfx;

namespace MiniCraft.Entities
{
    public class Lantern : Furniture
    {
        public Lantern()
            : base("Lantern")
        {
            Col = ColorHelper.Get(-1, 000, 111, 555);
            Sprite = 5;
            Xr = 3;
            Yr = 2;
        }

        public override int GetLightRadius()
        {
            return 8;
        }
    }
}
