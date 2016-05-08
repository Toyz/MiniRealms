using System;
using System.Collections.Generic;
using MiniRealms.Entities;
using MiniRealms.Levels;
using MiniRealms.Levels.Tiles;

namespace MiniRealms.Items.Resources
{

    public class Resource
    {
        public static List<Resource> AllResources = new List<Resource>();

        public static Resource Wood = new Resource("Wood", 1 + 4 * 32, Engine.Gfx.Color.Get(-1, 200, 531, 430));
        public static Resource Stone = new Resource("Stone", 2 + 4 * 32, Engine.Gfx.Color.Get(-1, 111, 333, 555));
        //Flowers!
        public static Resource Flower = new PlantableResource("Flower", 0 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 444, 330), TileId.Flower, TileId.Grass);
        public static Resource RedFlower = new PlantableResource("Rose", 0 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 300, 330), TileId.RedFlower, TileId.Grass);
        public static Resource YellowFlower = new PlantableResource("Daffodil", 0 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 550, 330), TileId.YellowFlower, TileId.Grass);
        //Other shit
        public static Resource Acorn = new PlantableResource("Acorn", 3 + 4 * 32, Engine.Gfx.Color.Get(-1, 100, 531, 320), TileId.TreeSapling, TileId.Grass);
        public static Resource Dirt = new PlantableResource("Dirt", 2 + 4 * 32, Engine.Gfx.Color.Get(-1, 100, 322, 432), TileId.Dirt, TileId.Hole, TileId.Water, TileId.Lava);
        public static Resource Sand = new PlantableResource("Sand", 2 + 4 * 32, Engine.Gfx.Color.Get(-1, 110, 440, 550), TileId.Sand, TileId.Grass, TileId.Dirt);
        public static Resource CactusFlower = new PlantableResource("Cactus", 4 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 40, 50), TileId.CactusSapling, TileId.Sand);
        public static Resource Seeds = new PlantableResource("Seeds", 5 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 40, 50), TileId.Wheat, TileId.Farmland);
        public static Resource Cloud = new PlantableResource("cloud", 2 + 4 * 32, Engine.Gfx.Color.Get(-1, 222, 555, 444), TileId.Cloud, TileId.InfiniteFall);

        public static Resource Wheat = new Resource("Wheat", 6 + 4 * 32, Engine.Gfx.Color.Get(-1, 110, 330, 550));
        public static Resource Bread = new FoodResource("Bread", 8 + 4 * 32, Engine.Gfx.Color.Get(-1, 110, 330, 550), 2, 5);
        public static Resource Apple = new FoodResource("Apple", 9 + 4 * 32, Engine.Gfx.Color.Get(-1, 100, 300, 500), 1, 5);

        public static Resource Coal = new Resource("COAL", 10 + 4 * 32, Engine.Gfx.Color.Get(-1, 000, 111, 111));
        public static Resource IronOre = new Resource("I.ORE", 10 + 4 * 32, Engine.Gfx.Color.Get(-1, 100, 322, 544));
        public static Resource GoldOre = new Resource("G.ORE", 10 + 4 * 32, Engine.Gfx.Color.Get(-1, 110, 440, 553));
        public static Resource IronIngot = new Resource("IRON", 11 + 4 * 32, Engine.Gfx.Color.Get(-1, 100, 322, 544));
        public static Resource GoldIngot = new Resource("GOLD", 11 + 4 * 32, Engine.Gfx.Color.Get(-1, 110, 330, 553));

        public static Resource Slime = new Resource("SLIME", 10 + 4 * 32, Engine.Gfx.Color.Get(-1, 10, 30, 50));
        public static Resource Glass = new Resource("glass", 12 + 4 * 32, Engine.Gfx.Color.Get(-1, 555, 555, 555));
        public static Resource Cloth = new Resource("cloth", 1 + 4 * 32, Engine.Gfx.Color.Get(-1, 25, 252, 141));
        public static Resource Gem = new Resource("gem", 13 + 4 * 32, Engine.Gfx.Color.Get(-1, 101, 404, 545));

        public string Name { get;  }
        public int Sprite { get; }
        public int Color { get; }

        public Resource(string name, int sprite, int color)
        {
            if (name.Length > 8) throw new Exception($"{nameof(name)} cannot be longer than 8 characters!");
            Name = name;
            Sprite = sprite;
            Color = color;

            Extensions.Add(AllResources, this);
        }

        public virtual bool InteractOn(Tile tile, Level level, int xt, int yt, Player player, int attackDir) => false;
    }
}
