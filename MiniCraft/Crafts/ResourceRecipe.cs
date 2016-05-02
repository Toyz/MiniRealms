using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Crafts
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
