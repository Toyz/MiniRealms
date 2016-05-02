using MiniCraft.Crafts;
using MiniCraft.Gfx;
using MiniCraft.Screens;

namespace MiniCraft.Entities
{
    public class Workbench : Furniture
    {
        public Workbench()
            : base("Workbench")
        {
            Col = Color.Get(-1, 100, 321, 431);
            Sprite = 4;
            Xr = 3;
            Yr = 2;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new CraftingMenu(Crafting.WorkbenchRecipes, player));
            return true;
        }
    }
}
