using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Items
{
    public class FurnitureItem : Item
    {
        public Furniture Furniture;
        public bool Placed;

        public FurnitureItem(Furniture furniture)
        {
            Furniture = furniture;
        }

        public override int GetColor() => Furniture.Col;

        public override int GetSprite() => Furniture.Sprite + 10 * 32;

        public override void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
            screen.Render(x, y, GetSprite(), GetColor(), bits);
        }

        public override void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, GetSprite(), GetColor(), 0);
            Font.Draw(Furniture.Name, screen, x + 8, y, ColorHelper.Get(-1, 555, 555, 555));
        }

        public override void OnTake(ItemEntity itemEntity)
        {
        }

        public override bool CanAttack() => false;

        public override bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir)
        {
            if (!tile.MayPass(level, xt, yt, Furniture)) return false;
            Furniture.X = xt * 16 + 8;
            Furniture.Y = yt * 16 + 8;
            level.Add(Furniture);
            Placed = true;
            return true;
        }

        public override bool IsDepleted() => Placed;

        public override string GetName() => Furniture.Name;
    }
}
