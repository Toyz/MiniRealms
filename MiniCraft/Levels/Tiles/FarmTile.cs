using MiniRealms.Engine;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;

namespace MiniRealms.Levels.Tiles
{
    public class FarmTile : Tile
    {
        public FarmTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = Color.Get(level.DirtColor - 121, level.DirtColor - 11, level.DirtColor, level.DirtColor + 111);
            screen.Render(x * 16 + 0, y * 16 + 0, 2 + 32, col, 1);
            screen.Render(x * 16 + 8, y * 16 + 0, 2 + 32, col, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, 2 + 32, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 8, 2 + 32, col, 1);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.ObjectType != ToolType.Shovel) return false;
            if (!player.PayStamina(4 - tool.Level)) return false;
            level.SetTile(xt, yt, Dirt, 0);
            return true;
        }

        public override void Tick(Level level, int xt, int yt)
        {
            int age = level.GetData(xt, yt);
            if (age < 5) level.SetData(xt, yt, age + 1);
        }

        public override void SteppedOn(Level level, int xt, int yt, Entity entity)
        {
            if (Random.NextInt(60) != 0) return;
            if (level.GetData(xt, yt) < 5) return;
            level.SetTile(xt, yt, Dirt, 0);
        }
    }
}
