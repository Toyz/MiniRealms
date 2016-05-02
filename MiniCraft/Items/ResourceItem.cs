using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items.Resources;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Items
{
    public class ResourceItem : Item
    {
        public Resource Resource;
        public int Count = 1;

        public ResourceItem(Resource resource)
        {
            Resource = resource;
        }

        public ResourceItem(Resource resource, int count)
        {
            Resource = resource;
            Count = count;
        }

        public override int GetColor() => Resource.Color;

        public override int GetSprite() => Resource.Sprite;

        public override void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
            screen.Render(x, y, Resource.Sprite, Resource.Color, bits);
        }

        public override void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, Resource.Sprite, Resource.Color, 0);
            Font.Draw(Resource.Name, screen, x + 32, y, ColorHelper.Get(-1, 555, 555, 555));
            int cc = Count;
            if (cc > 99) cc = 99;
            Font.Draw("" + cc, screen, x + 9, y, ColorHelper.Get(-1, 444, 444, 444));
        }

        public override string GetName() => Resource.Name;

        public override void OnTake(ItemEntity itemEntity)
        {
        }

        public override bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir)
        {
            if (!Resource.InteractOn(tile, level, xt, yt, player, attackDir)) return false;
            Count--;
            return true;
        }

        public override bool IsDepleted() => Count <= 0;
    }
}
