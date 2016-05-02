using MiniCraft.Gfx;

namespace MiniCraft.Levels.Tiles
{
    public class StairsTile : Tile
    {
        private readonly bool _leadsUp;

        public StairsTile(TileId id, bool leadsUp)
            : base(id)
        {
            _leadsUp = leadsUp;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int color = Color.Get(level.DirtColor, 000, 333, 444);
            int xt = 0;
            if (_leadsUp) xt = 2;
            screen.Render(x * 16 + 0, y * 16 + 0, xt + 2 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, xt + 1 + 2 * 32, color, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, xt + 3 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, xt + 1 + 3 * 32, color, 0);
        }
    }
}
