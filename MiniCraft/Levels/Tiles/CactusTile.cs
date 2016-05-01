using MiniCraft.Entities;
using MiniCraft.Entities.Particles;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Levels.Tiles
{

    public class CactusTile : Tile
    {
        public CactusTile(TileId id)
            : base(id)
        {
            ConnectsToSand = true;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = ColorHelper.Get(20, 40, 50, level.SandColor);
            screen.Render(x * 16 + 0, y * 16 + 0, 8 + 2 * 32, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, 9 + 2 * 32, col, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, 8 + 3 * 32, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, 9 + 3 * 32, col, 0);
        }

        public override bool MayPass(Level level, int x, int y, Entity e)
        {
            return false;
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            int damage = level.GetData(x, y) + dmg;
            level.Add(new SmashParticle(x * 16 + 8, y * 16 + 8));
            level.Add(new TextParticle("" + dmg, x * 16 + 8, y * 16 + 8, ColorHelper.Get(-1, 500, 500, 500)));
            if (damage >= 10)
            {
                int count = Random.NextInt(2) + 1;
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.CactusFlower), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
                }
                level.SetTile(x, y, Sand, 0);
            }
            else
            {
                level.SetData(x, y, damage);
            }
        }

        public override void BumpedInto(Level level, int x, int y, Entity entity)
        {
            entity.Hurt(this, x, y, 1);
        }

        public override void Tick(Level level, int xt, int yt)
        {
            int damage = level.GetData(xt, yt);
            if (damage > 0) level.SetData(xt, yt, damage - 1);
        }
    }
}
