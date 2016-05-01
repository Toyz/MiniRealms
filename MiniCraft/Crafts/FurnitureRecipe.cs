using MiniCraft.Entities;
using MiniCraft.Items;

namespace MiniCraft.Crafts
{
    public class FurnitureRecipe<T> : Recipe
        where T : Furniture, new()
    {

        public FurnitureRecipe()
            : base(new FurnitureItem(new T()))
        {
        }

        public override void Craft(Player player)
        {
            player.Inventory.Add(0, new FurnitureItem(new T()));
        }
    }
}
