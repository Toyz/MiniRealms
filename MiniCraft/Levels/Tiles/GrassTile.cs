using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Items.Resources;
using MiniCraft.Sounds;

namespace MiniCraft.Levels.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(TileId id)
            : base(id)
        {
            ConnectsToGrass = true;
        }

        public override void Render(Screen screen, Level level, int x, int y)
        {
            int col = ColorHelper.Get(level.GrassColor, level.GrassColor, level.GrassColor + 111, level.GrassColor + 111);
            int transitionColor = ColorHelper.Get(level.GrassColor - 111, level.GrassColor, level.GrassColor + 111, level.DirtColor);

            bool u = !level.GetTile(x, y - 1).ConnectsToGrass;
            bool d = !level.GetTile(x, y + 1).ConnectsToGrass;
            bool l = !level.GetTile(x - 1, y).ConnectsToGrass;
            bool r = !level.GetTile(x + 1, y).ConnectsToGrass;

            if (!u && !l)
            {
                screen.Render(x * 16 + 0, y * 16 + 0, 0, col, 0);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 0, (l ? 11 : 12) + (u ? 0 : 1) * 32, transitionColor, 0);

            if (!u && !r)
            {
                screen.Render(x * 16 + 8, y * 16 + 0, 1, col, 0);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 0, (r ? 13 : 12) + (u ? 0 : 1) * 32, transitionColor, 0);

            if (!d && !l)
            {
                screen.Render(x * 16 + 0, y * 16 + 8, 2, col, 0);
            }
            else
                screen.Render(x * 16 + 0, y * 16 + 8, (l ? 11 : 12) + (d ? 2 : 1) * 32, transitionColor, 0);
            if (!d && !r)
            {
                screen.Render(x * 16 + 8, y * 16 + 8, 3, col, 0);
            }
            else
                screen.Render(x * 16 + 8, y * 16 + 8, (r ? 13 : 12) + (d ? 2 : 1) * 32, transitionColor, 0);
        }

        public override void Tick(Level level, int xt, int yt)
        {
            if (Random.NextInt(40) != 0) return;

            int xn = xt;
            int yn = yt;

            if (Random.Nextbool())
                xn += Random.NextInt(2) * 2 - 1;
            else
                yn += Random.NextInt(2) * 2 - 1;

            if (level.GetTile(xn, yn) == Dirt)
            {
                level.SetTile(xn, yn, this, 0);
            }
        }

        public override bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem tool = toolItem;
            if (tool.Type == ToolType.Shovel && player.PayStamina(4 - tool.Level))
            {
                level.SetTile(xt, yt, Dirt, 0);
                Sound.MonsterHurt.Play();
                if (Random.NextInt(5) == 0)
                {
                    level.Add(new ItemEntity(new ResourceItem(Resource.Seeds), xt*16 + Random.NextInt(10) + 3,
                        yt*16 + Random.NextInt(10) + 3));
                    return true;
                }
            }
            if (tool.Type != ToolType.Hoe) return false;
            if (!player.PayStamina(4 - tool.Level)) return false;
            Sound.MonsterHurt.Play();
            if (Random.NextInt(5) == 0)
            {
                level.Add(new ItemEntity(new ResourceItem(Resource.Seeds), xt * 16 + Random.NextInt(10) + 3, yt * 16 + Random.NextInt(10) + 3));
                return true;
            }
            level.SetTile(xt, yt, Farmland, 0);
            return true;
        }
    }

}
