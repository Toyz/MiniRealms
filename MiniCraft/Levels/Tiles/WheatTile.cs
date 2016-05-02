using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Levels.Tiles
{

    public class WheatTile : Tile
    {
        public WheatTile(TileId id)
            : base(id)
        {
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int age = level.GetData(x, y);
            int col = Color.Get(level.DirtColor - 121, level.DirtColor - 11, level.DirtColor, 50);
            int icon = age / 10;
            if (icon >= 3)
            {
                col = Color.Get(level.DirtColor - 121, level.DirtColor - 11, 50 + (icon) * 100, 40 + (icon - 3) * 2 * 100);
                if (age == 50)
                {
                    col = Color.Get(0, 0, 50 + (icon) * 100, 40 + (icon - 3) * 2 * 100);
                }
                icon = 3;
            }

            screen.Render(x * 16 + 0, y * 16 + 0, 4 + 3 * 32 + icon, col, 0);
            screen.Render(x * 16 + 8, y * 16 + 0, 4 + 3 * 32 + icon, col, 0);
            screen.Render(x * 16 + 0, y * 16 + 8, 4 + 3 * 32 + icon, col, 1);
            screen.Render(x * 16 + 8, y * 16 + 8, 4 + 3 * 32 + icon, col, 1);
        }

        public override void Tick(Level level, int xt, int yt)
        {
            if (Random.NextInt(2) == 0) return;

            int age = level.GetData(xt, yt);
            if (age < 50) level.SetData(xt, yt, age + 1);
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            ToolItem toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.Type != ToolType.Shovel || !player.PayStamina(4 - tool.Level)) return false;
            level.SetTile(xt, yt, Dirt, 0);
            return true;
        }

        public override void SteppedOn(Level level, int xt, int yt, Entity entity)
        {
            if (Random.NextInt(60) != 0) return;
            if (level.GetData(xt, yt) < 2) return;
            Harvest(level, xt, yt);
        }

        public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir) => Harvest(level, x, y);

        private void Harvest(Level level, int x, int y)
        {
            int age = level.GetData(x, y);

            int count = Random.NextInt(2);
            for (int i = 0; i < count; i++)
            {
                level.Add(new ItemEntity(new ResourceItem(Resource.Seeds), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            }

            count = 0;
            if (age == 50)
            {
                count = Random.NextInt(3) + 2;
            }
            else if (age >= 40)
            {
                count = Random.NextInt(2) + 1;
            }
            for (int i = 0; i < count; i++)
            {
                level.Add(new ItemEntity(new ResourceItem(Resource.Wheat), x * 16 + Random.NextInt(10) + 3, y * 16 + Random.NextInt(10) + 3));
            }

            level.SetTile(x, y, Dirt, 0);
        }
    }

}
