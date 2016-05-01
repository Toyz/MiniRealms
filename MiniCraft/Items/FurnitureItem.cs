using System;
using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Items
{
    public class FurnitureItem : Item
    {
        public Furniture furniture;
        public bool placed = false;

        public FurnitureItem(Furniture furniture)
        {
            this.furniture = furniture;
        }

        public override int GetColor()
        {
            return furniture.Col;
        }

        public override int GetSprite()
        {
            return furniture.Sprite + 10 * 32;
        }

        public override void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
            screen.Render(x, y, GetSprite(), GetColor(), bits);
        }

        public override void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, GetSprite(), GetColor(), 0);
            Font.Draw(furniture.Name, screen, x + 8, y, ColorHelper.Get(-1, 555, 555, 555));
        }

        public override void OnTake(ItemEntity itemEntity)
        {
        }

        public override bool CanAttack()
        {
            return false;
        }

        public override bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir)
        {
            if (tile.MayPass(level, xt, yt, furniture))
            {
                furniture.X = xt * 16 + 8;
                furniture.Y = yt * 16 + 8;
                level.Add(furniture);
                placed = true;
                return true;
            }
            return false;
        }

        public override bool IsDepleted()
        {
            return placed;
        }

        public override string GetName()
        {
            return furniture.Name;
        }
    }
}
