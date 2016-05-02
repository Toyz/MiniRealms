using MiniRealms.Crafts;
using MiniRealms.Gfx;
using MiniRealms.Screens;

namespace MiniRealms.Entities
{
    public class Oven : Furniture
    {
        public Oven()
            : base("Oven")
        {
            Col = Color.Get(-1, 000, 332, 442);
            Sprite = 2;
            Xr = 3;
            Yr = 2;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new CraftingMenu(Crafting.OvenRecipes, player));
            return true;
        }
    }
}
