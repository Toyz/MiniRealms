using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{
    public class FlowerTile : GrassTile
    {
        private readonly int _c1;
        private readonly int _c2;
        private readonly Resource _drop;

        public FlowerTile(TileId id, int c1, int c2, Resource drop)
            : base(id)
        {
            _c1 = c1;
            _c2 = c2;
            _drop = drop;
            Tiles[(byte)id] = this;
            ConnectsToGrass = true;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            base.Render(screen, level, x, y);

            int data = level.GetData(x, y);
            int shape = (data / 16) % 2;
            int flowerCol = Color.Get(10, level.GrassColor, _c1, _c2);

            if (shape == 0) screen.Render(x * 16 + 0, y * 16 + 0, 1 + 1 * 32, flowerCol, 0);
            if (shape == 1) screen.Render(x * 16 + 8, y * 16 + 0, 1 + 1 * 32, flowerCol, 0);
            if (shape == 1) screen.Render(x * 16 + 0, y * 16 + 8, 1 + 1 * 32, flowerCol, 0);
            if (shape == 0) screen.Render(x * 16 + 8, y * 16 + 8, 1 + 1 * 32, flowerCol, 0);
        }

        public override bool Interact(Level level, int x, int y, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.Type != ToolType.Shovel) return false;
            if (!player.PayStamina(4 - tool.Level)) return false;
            level.Add(new ItemEntity(new ResourceItem(_drop), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            level.Add(new ItemEntity(new ResourceItem(_drop), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            level.SetTile(x, y, Grass, 0);
            return true;
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
            int count = Random.NextInt(2) + 1;
            for (int i = 0; i < count; i++)
            {
                level.Add(new ItemEntity(new ResourceItem(_drop), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            }
            level.SetTile(x, y, Grass, 0);
        }
    }
}
