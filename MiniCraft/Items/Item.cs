using System;
using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;
using MiniCraft.Screens;

namespace MiniCraft.Items
{
    public class Item : IListItem
    {
        public virtual int GetColor() => 0;

        public virtual int GetSprite() => 0;

        public virtual void OnTake(ItemEntity itemEntity)
        {
        }

        public virtual void RenderInventory(Screen screen, int x, int y)
        {
        }

        public virtual bool Interact(Player player, Entity entity, int attackDir) => false;

        public virtual void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
        }

        public virtual bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir) => false;

        public virtual bool IsDepleted() => false;

        public virtual bool CanAttack() => false;

        public virtual int GetAttackDamageBonus(Entity e) => 0;

        public virtual string GetName() => "";

        public virtual bool Matches(Item item) => item.GetType() == GetType();
    }
}
