using MiniRealms.Crafts;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.GameScreens;

namespace MiniRealms.Entities
{
    public class Furnace : Furniture
    {
        public Furnace()
            : base("Furnace")
        {
            Col = Color.Get(-1, 000, 222, 333);
            Sprite = 3;
            Xr = 3;
            Yr = 2;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new CraftingMenu(Crafting.FurnaceRecipes, player));
            return true;
        }
    }
}
