using System.Collections.Generic;
using System.Linq;
using MiniCraft.Items;
using MiniCraft.Items.Resources;

namespace MiniCraft.Entities
{
    public class Inventory
    {
        public List<Item> Items = new List<Item>();

        public void Add(Item item)
        {
            Add(Items.Count, item);
        }

        public void Add(int slot, Item item)
        {
            var take = item as ResourceItem;
            if (take != null)
            {
                ResourceItem toTake = take;
                ResourceItem has = FindResource(toTake.Resource);
                if (has == null)
                {
                    Items.Insert(slot, toTake);
                }
                else
                {
                    has.Count += toTake.Count;
                }
            }
            else
            {
                Items.Insert(slot, item);
            }
        }

        private ResourceItem FindResource(Resource resource)
        {
            return Items.OfType<ResourceItem>().FirstOrDefault(has => has.Resource == resource);
        }

        public bool HasResources(Resource r, int count)
        {
            ResourceItem ri = FindResource(r);
            return ri != null && ri.Count >= count;
        }

        public bool RemoveResource(Resource r, int count)
        {
            ResourceItem ri = FindResource(r);
            if (ri == null) return false;
            if (ri.Count < count) return false;
            ri.Count -= count;
            if (ri.Count <= 0) Items.Remove(ri);
            return true;
        }

        public int Count(Item item)
        {
            var resourceItem = item as ResourceItem;
            if (resourceItem != null)
            {
                ResourceItem ri = FindResource(resourceItem.Resource);
                if (ri != null) return ri.Count;
            }
            else
            {
                return Items.Count(t => t.Matches(item));
            }
            return 0;
        }
    }

}
