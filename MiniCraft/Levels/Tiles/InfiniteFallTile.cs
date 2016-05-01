using MiniCraft.Entities;
using MiniCraft.Gfx;

namespace MiniCraft.Levels.Tiles
{
    public class InfiniteFallTile : Tile
    {
        public InfiniteFallTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
        }

        public override void Tick(Level level, int xt, int yt)
        {
        }

        public override bool MayPass(Level level, int x, int y, Entity e)
        {
            return e is AirWizard;
        }
    }
}
