using MiniRealms.Entities;
using MiniRealms.Gfx;

namespace MiniRealms.Levels.Tiles
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

        public override bool MayPass(Level level, int x, int y, Entity e) => e is AirWizard;
    }
}
