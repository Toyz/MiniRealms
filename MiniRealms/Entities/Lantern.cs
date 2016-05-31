using MiniRealms.Engine.Gfx;

namespace MiniRealms.Entities
{
    public class Lantern : Furniture
    {
        public Lantern()
            : base("Lantern")
        {
            Col = Color.Get(-1, 000, 111, 555);
            Sprite = 5;
            Xr = 3;
            Yr = 2;
        }

        public override int GetLightRadius() => 8;
    }
}
