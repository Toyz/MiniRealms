using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Levels.Tiles
{

    public class CloudTile : Tile
    {
        public CloudTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = Color.Get(444, 444, 555, 555);
            int transitionColor = Color.Get(333, 444, 555, -1);

            bool u = level.GetTile(x, y - 1) == InfiniteFall;
            bool d = level.GetTile(x, y + 1) == InfiniteFall;
            bool l = level.GetTile(x - 1, y) == InfiniteFall;
            bool r = level.GetTile(x + 1, y) == InfiniteFall;

            bool ul = level.GetTile(x - 1, y - 1) == InfiniteFall;
            bool dl = level.GetTile(x - 1, y + 1) == InfiniteFall;
            bool ur = level.GetTile(x + 1, y - 1) == InfiniteFall;
            bool dr = level.GetTile(x + 1, y + 1) == InfiniteFall;

            if (!u && !l)
            {
                if (!ul)
                    screen.Render(x * 16 + 0, y * 16 + 0, 17, col, 0);
                else
                    screen.Render(x * 16 + 0, y * 16 + 0, 7 + 0 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 0, (l ? 6 : 5) + (u ? 2 : 1) * 32, transitionColor, 3);

            if (!u && !r)
            {
                if (!ur)
                    screen.Render(x * 16 + 8, y * 16 + 0, 18, col, 0);
                else
                    screen.Render(x * 16 + 8, y * 16 + 0, 8 + 0 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 0, (r ? 4 : 5) + (u ? 2 : 1) * 32, transitionColor, 3);

            if (!d && !l)
            {
                if (!dl)
                    screen.Render(x * 16 + 0, y * 16 + 8, 20, col, 0);
                else
                    screen.Render(x * 16 + 0, y * 16 + 8, 7 + 1 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 8, (l ? 6 : 5) + (d ? 0 : 1) * 32, transitionColor, 3);
            if (!d && !r)
            {
                if (!dr)
                    screen.Render(x * 16 + 8, y * 16 + 8, 19, col, 0);
                else
                    screen.Render(x * 16 + 8, y * 16 + 8, 8 + 1 * 32, transitionColor, 3);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 8, (r ? 4 : 5) + (d ? 0 : 1) * 32, transitionColor, 3);
        }

        public override bool MayPass(Level level, int x, int y, Entity e) => true;

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.Type != ToolType.Shovel || !player.PayStamina(5)) return false;
            // level.setTile(xt, yt, Tile.infiniteFall, 0);
            int count = Random.NextInt(2) + 1;
            for (int i = 0; i < count; i++)
            {
                level.Add(new ItemEntity(new ResourceItem(Resource.Cloud), xt*16 + Random.NextInt(10) + 3,
                    yt*16 + Random.NextInt(10) + 3));
            }
            return true;
        }

        /*
         * public override bool interact(Level level, int xt, int yt, Player player, Item item, int attackDir) { if (item instanceof ToolItem) { ToolItem tool = (ToolItem) item; if (tool.type == ToolType.pickaxe) { if (player.payStamina(4 - tool.level)) { Hurt(level, xt, yt, random.nextInt(10) + (tool.level) * 5 + 10); return true; } } } return false; }
         */
    }
}
