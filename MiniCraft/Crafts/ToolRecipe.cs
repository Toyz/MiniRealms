using MiniCraft.Entities;
using MiniCraft.Items;

namespace MiniCraft.Crafts
{
    public class ToolRecipe : Recipe
    {
        private readonly ToolType _type;
        private readonly int _level;

        public ToolRecipe(ToolType type, int level)
            :base (new ToolItem(type, level))
        {
            _type = type;
            _level = level;
        }

        public override void Craft(Player player)
        {
            player.Inventory.Add(0, new ToolItem(_type, _level));
        }
    }
}
