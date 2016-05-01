using MiniCraft.Entities;
using MiniCraft.Gfx;

namespace MiniCraft.Items
{
    public class PowerGloveItem : Item
    {
        public override int GetColor()
        {
            return ColorHelper.Get(-1, 100, 320, 430);
        }

        public override int GetSprite()
        {
            return 7 + 4 * 32;
        }

        public override void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
            screen.Render(x, y, GetSprite(), GetColor(), bits);
        }

        public override void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, GetSprite(), GetColor(), 0);
            Font.Draw(GetName(), screen, x + 8, y, ColorHelper.Get(-1, 555, 555, 555));
        }

        public override string GetName()
        {
            return "Pow glove";
        }

        public override bool Interact(Player player, Entity entity, int attackDir)
        {
            if (entity is Furniture)
            {
                Furniture f = (Furniture)entity;
                f.Take(player);
                return true;
            }
            return false;
        }
    }
}
