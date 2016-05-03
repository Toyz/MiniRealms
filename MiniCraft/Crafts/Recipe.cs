using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;
using MiniRealms.Screens;

namespace MiniRealms.Crafts
{
    public abstract class Recipe : IListItem
    {
        public List<Item> Costs = new List<Item>();
        public bool CanCraft;
        public Item ResultTemplate;

        protected Recipe(Item resultTemplate)
        {
            ResultTemplate = resultTemplate;
        }

        public Recipe AddCost(Resource resource, int count)
        {
            Extensions.Add(Costs, new ResourceItem(resource, count));
            return this;
        }

        public void CheckCanCraft(Player player)
        {
            for (int i = 0; i < Costs.Size(); i++)
            {
                Item item = Costs.Get(i);
                var resourceItem = item as ResourceItem;
                if (resourceItem == null) continue;
                ResourceItem ri = resourceItem;
                if (player.Inventory.HasResources(ri.Resource, ri.Count)) continue;
                CanCraft = false;
                return;
            }
            CanCraft = true;
        }

        public virtual void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, ResultTemplate.GetSprite(), ResultTemplate.GetColor(), 0);
            int textColor = CanCraft ? Color.Get(-1, 555, 555, 555) : Color.Get(-1, 222, 222, 222);
            Font.Draw(ResultTemplate.GetName(), screen, x + 8, y, textColor);
        }

        public abstract void Craft(Player player);

        public void DeductCost(Player player)
        {
            for (int i = 0; i < Costs.Size(); i++)
            {
                Item item = Costs.Get(i);
                var resourceItem = item as ResourceItem;
                if (resourceItem == null) continue;
                ResourceItem ri = resourceItem;
                player.Inventory.RemoveResource(ri.Resource, ri.Count);
            }
        }
    }
}
