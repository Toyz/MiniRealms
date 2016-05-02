using MiniCraft.Entities;
using MiniCraft.Entities.Particles;
using MiniCraft.Gfx;
using MiniCraft.Items;

namespace MiniCraft.Levels.Tiles
{
    public class CloudCactusTile : Tile
    {
        public CloudCactusTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int color = Color.Get(444, 111, 333, 555);
            screen.Render(x * 16 + 0, y * 16 + 0, 17 + 1 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, 18 + 1 * 32, color, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, 17 + 2 * 32, color, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, 18 + 2 * 32, color, 0);
        }

        public override bool MayPass(Level level, int x, int y, Entity e)
        {
            return e is AirWizard;
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            Hurt(level, x, y, 0);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.Type != ToolType.Pickaxe || !player.PayStamina(6 - tool.Level)) return false;
            Hurt(level, xt, yt, 1);
            return true;
        }

        public static void Hurt(Level level, int x, int y, int dmg)
        {
            int damage = level.GetData(x, y) + 1;
            level.Add(new SmashParticle(x * 16 + 8, y * 16 + 8));
            level.Add(new TextParticle("" + dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
            if (dmg <= 0) return;
            if (damage >= 10)
            {
                level.SetTile(x, y, Cloud, 0);
            }
            else
            {
                level.SetData(x, y, damage);
            }
        }

        public override void BumpedInto(Level level, int x, int y, Entity entity)
        {
            if (entity is AirWizard) return;
            entity.Hurt(this, x, y, 3);
        }
    }
}
