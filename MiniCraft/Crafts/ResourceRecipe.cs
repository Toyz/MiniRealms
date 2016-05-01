using MiniCraft.Entities;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Crafts
{
    public class ResourceRecipe : Recipe
    {
        private readonly Resource _resource;

        public ResourceRecipe(Resource resource)
            : base(new ResourceItem(resource, 1))
        {
            _resource = resource;
        }

        public override void Craft(Player player)
        {
            player.Inventory.Add(0, new ResourceItem(_resource, 1));
        }
    }
}
