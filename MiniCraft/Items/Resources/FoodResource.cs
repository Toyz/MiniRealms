using MiniCraft.Entities;
using MiniCraft.Levels;
using MiniCraft.Levels.Tiles;

namespace MiniCraft.Items.Resources
{
    public class FoodResource : Resource
    {
        private readonly int _heal;
        private readonly int _staminaCost;

        public FoodResource(string name, int sprite, int color, int heal, int staminaCost)
            : base(name, sprite, color)
        {
            _heal = heal;
            _staminaCost = staminaCost;
        }

        public override bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir)
        {
            if (player.Health >= player.MaxHealth || !player.PayStamina(_staminaCost)) return false;
            player.Heal(_heal);
            return true;
        }
    }
}
