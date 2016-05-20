using System;
using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Levels.Tiles
{
    public class Tile
    {
        public static int TickCount = 0;
        protected Random Random = new Random();

        public static Tile[] Tiles = new Tile[256];
        public static Tile Grass;
        public static Tile Rock;
        public static Tile Water;

        public static Tile Flower;
        public static Tile RedFlower;
        public static Tile YellowFlower;

        public static Tile Tree;
        public static Tile Dirt;
        public static Tile Sand;
        public static Tile Cactus;
        public static Tile Hole;
        public static Tile TreeSapling;
        public static Tile CactusSapling;
        public static Tile Farmland;
        public static Tile Wheat;
        public static Tile Lava;
        public static Tile StairsDown;
        public static Tile StairsUp;
        public static Tile InfiniteFall;
        public static Tile Cloud;
        public static Tile HardRock;
        public static Tile IronOre;
        public static Tile GoldOre;
        public static Tile GemOre;
        public static Tile CloudCactus;

        static Tile()
        {
            Grass = new GrassTile(TileId.Grass);
            Rock = new RockTile(TileId.Rock);
            Water = new WaterTile(TileId.Water);
            Flower = new FlowerTile(TileId.Flower, Resource.Flower);
            RedFlower = new FlowerTile(TileId.RedFlower, Resource.RedFlower);
            YellowFlower = new FlowerTile(TileId.YellowFlower, Resource.YellowFlower);
            Tree = new TreeTile(TileId.Tree);
            Dirt = new DirtTile(TileId.Dirt);
            Sand = new SandTile(TileId.Sand);
            Cactus = new CactusTile(TileId.Cactus);
            Hole = new HoleTile(TileId.Hole);
            TreeSapling = new SaplingTile(TileId.TreeSapling, Grass, Tree);
            CactusSapling = new SaplingTile(TileId.CactusSapling, Sand, Cactus);
            Farmland = new FarmTile(TileId.Farmland);
            Wheat = new WheatTile(TileId.Wheat);
            Lava = new LavaTile(TileId.Lava);
            StairsDown = new StairsTile(TileId.StairsDown, false);
            StairsUp = new StairsTile(TileId.StairsUp, true);
            InfiniteFall = new InfiniteFallTile(TileId.InfiniteFall);
            Cloud = new CloudTile(TileId.Cloud);
            HardRock = new HardRockTile(TileId.Hardrock);
            IronOre = new OreTile(TileId.IronOre, Resource.IronOre);
            GoldOre = new OreTile(TileId.GoldOre, Resource.GoldOre);
            GemOre = new OreTile(TileId.GemOre, Resource.Gem);
            CloudCactus = new CloudCactusTile(TileId.CloudCactus);
        }


        public readonly byte Id;

        public bool ConnectsToGrass = false;
        public bool ConnectsToSand = false;
        public bool ConnectsToLava = false;
        public bool ConnectsToWater = false;
        public readonly Sprite[] Sprites;

        public Tile(TileId tileId)
        {
            Id = (byte)tileId;
            if (Tiles[Id] != null) throw new InvalidOperationException("Duplicate tile ids!");
            Tiles[Id] = this;

            Sprites = SpriteSheet.GetSprites(tileId).ToArray();
//            Debug.WriteLine($"{tileId}, Spirts Count {Sprites.Count}");
        }

        public virtual void Render(Screen screen, Level level, int x, int y)
        {
        }

        public virtual bool MayPass(Level level, int x, int y, Entity e) => true;

        public virtual int GetLightRadius(Level level, int x, int y) => 0;

        public virtual void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
        {
        }

        public virtual void BumpedInto(Level level, int xt, int yt, Entity entity)
        {
        }

        public virtual void Tick(Level level, int xt, int yt)
        {
        }

        public virtual void SteppedOn(Level level, int xt, int yt, Entity entity)
        {
        }

        public virtual bool Interact(Level level, int xt, int yt, Player player, Item item, int attackDir) => false;

        public virtual bool Use(Level level, int xt, int yt, Player player, int attackDir) => false;

        public virtual bool ConnectsToLiquid() => ConnectsToWater || ConnectsToLava;
    }
}
