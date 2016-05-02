using MiniCraft.Crafts;
using MiniCraft.Gfx;
using MiniCraft.Screens;

namespace MiniCraft.Entities
{
    public class Anvil : Furniture
    {
        public Anvil()
            : base("Anvil")
        {
            Col = Color.Get(-1, 000, 111, 222);
            Sprite = 0;
            Xr = 3;
            Yr = 2;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new CraftingMenu(Crafting.AnvilRecipes, player));
            return true;
        }
    }
}
