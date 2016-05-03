using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Entities.Particles;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{

    public class OreTile : Tile
    {
        private readonly Resource _toDrop;
        private int _color;

        public OreTile(TileId id, Resource toDrop)
            : base(id)
        {
            _toDrop = toDrop;
            _color = toDrop.Color & 0xffff00;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            //TODO: check is casting to int is ok
            _color = (int)(_toDrop.Color & 0xffffff00) + Color.Get(level.DirtColor);
            screen.Render(x * 16 + 0, y * 16 + 0, 17 + 1 * 32, _color, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, 18 + 1 * 32, _color, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, 17 + 2 * 32, _color, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, 18 + 2 * 32, _color, 0);
        }

        public override bool MayPass(Level level, int x, int y, Entity e) => false;

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            Hurt(level, x, y, 0);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem != null)
            {
                ToolItem tool = toolItem;
                if (tool.Type == ToolType.Pickaxe)
                {
                    if (player.PayStamina(6 - tool.Level))
                    {
                        Hurt(level, xt, yt, 1);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Hurt(Level level, int x, int y, int dmg)
        {
            int damage = level.GetData(x, y) + 1;
            level.Add(new SmashParticle(x * 16 + 8, y * 16 + 8));
            level.Add(new TextParticle("" + dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
            if (dmg > 0)
            {
                int count = Random.NextInt(2);
                if (damage >= Random.NextInt(10) + 3)
                {
                    level.SetTile(x, y, Dirt, 0);
                    count += 2;
                }
                else
                {
                    level.SetData(x, y, damage);
                }
                for (int i = 0; i < count; i++)
                {
                    level.Add(new ItemEntity(new ResourceItem(_toDrop), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
                }
            }
        }

        public override void BumpedInto(Level level, int x, int y, Entity entity)
        {
            entity.Hurt(this, x, y, 3);
        }
    }
}
