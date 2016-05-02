using MiniCraft.Gfx;
using MiniCraft.Screens;

namespace MiniCraft.Entities
{
    public class Chest : Furniture
    {
        public Inventory Inventory = new Inventory();

        public Chest()
            : base("Chest")
        {
            Col = Color.Get(-1, 110, 331, 552);
            Sprite = 1;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new ContainerMenu(player, "Chest", Inventory));
            return true;
        }
    }
}
