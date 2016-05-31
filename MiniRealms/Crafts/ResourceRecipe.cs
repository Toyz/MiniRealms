using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Crafts
{
    public class ResourceRecipe : Recipe
    {
        private readonly Resource _resource;
        private readonly int _amount;

        public ResourceRecipe(Resource resource, int amount = 1)
            : base(new ResourceItem(resource, amount))
        {
            _resource = resource;
            _amount = amount;
        }

        public override void Craft(Player player)
        {
            player.Inventory.Add(0, new ResourceItem(_resource, _amount));
        }
    }
}
