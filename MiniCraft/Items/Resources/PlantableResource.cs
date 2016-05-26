using System.Collections.Generic;
using MiniRealms.Entities;
using MiniRealms.Levels;
using MiniRealms.Levels.Tiles;

namespace MiniRealms.Items.Resources
{
    public class PlantableResource : Resource
    {
        private readonly List<TileId> _sourceTiles;
        private readonly TileId _targetTile;
        public int MinDepthPlacementLevel = -1;

        public PlantableResource(string name, int sprite, int color, TileId targetTile, params TileId[] sourceTiles1) :
            this(name, sprite, color, targetTile, new List<TileId>(sourceTiles1))
        { }

        public PlantableResource(string name, int sprite, int color, TileId targetTile, List<TileId> sourceTiles)
            : base(name, sprite, color)
        {
            _sourceTiles = sourceTiles;
            _targetTile = targetTile;
        }

        public override bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir)
        {
            if (MinDepthPlacementLevel > -1)
            {
                if (level.Depth >= MinDepthPlacementLevel) return false;
            }

            if (!_sourceTiles.Contains((TileId) tile.Id)) return false;
            var tt = Tile.Tiles[(byte)_targetTile];
            level.SetTile(xt, yt, tt, 0);
            return true;
        }
    }
}
