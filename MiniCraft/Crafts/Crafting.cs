using System.Collections.Generic;
using MiniRealms.Engine;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Items.Resources;

namespace MiniRealms.Crafts
{
    public static class Crafting
    {
        public static readonly List<Recipe> AnvilRecipes = new List<Recipe>();
        public static readonly List<Recipe> OvenRecipes = new List<Recipe>();
        public static readonly List<Recipe> FurnaceRecipes = new List<Recipe>();
        public static readonly List<Recipe> WorkbenchRecipes = new List<Recipe>();

        static Crafting()
        {
            Extensions.Add(WorkbenchRecipes, new ResourceRecipe(Resource.Stick, 4).AddCost(Resource.Wood, 1));
            Extensions.Add(WorkbenchRecipes, new ResourceRecipe(Resource.Torch, 4).AddCost(Resource.Wood, 4).AddCost(Resource.Coal, 1));


            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Lantern>().AddCost(Resource.Wood, 5).AddCost(Resource.Slime, 10).AddCost(Resource.Glass, 4));
            //Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Lantern>().AddCost(Resource.Wood, 4).AddCost(Resource.Coal, 1));

            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Oven>().AddCost(Resource.Stone, 15));
            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Furnace>().AddCost(Resource.Stone, 20));
            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Workbench>().AddCost(Resource.Wood, 20));
            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Chest>().AddCost(Resource.Wood, 20));
            Extensions.Add(WorkbenchRecipes, new FurnitureRecipe<Anvil>().AddCost(Resource.IronIngot, 5));

            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Sword, 0).AddCost(Resource.Wood, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Axe, 0).AddCost(Resource.Wood, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Hoe, 0).AddCost(Resource.Wood, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Pickaxe, 0).AddCost(Resource.Wood, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Shovel, 0).AddCost(Resource.Wood, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Sword, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Axe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Hoe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Pickaxe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
            Extensions.Add(WorkbenchRecipes, new ToolRecipe(ToolType.Shovel, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));

            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Sword, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Axe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Hoe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Pickaxe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Shovel, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));

            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Sword, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Axe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Hoe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Pickaxe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Shovel, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));

            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Sword, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Axe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Hoe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Pickaxe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
            Extensions.Add(AnvilRecipes, new ToolRecipe(ToolType.Shovel, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));

            Extensions.Add(FurnaceRecipes, new ResourceRecipe(Resource.IronIngot).AddCost(Resource.IronOre, 4).AddCost(Resource.Coal, 1));
            Extensions.Add(FurnaceRecipes, new ResourceRecipe(Resource.GoldIngot).AddCost(Resource.GoldOre, 4).AddCost(Resource.Coal, 1));
            Extensions.Add(FurnaceRecipes, new ResourceRecipe(Resource.Glass).AddCost(Resource.Sand, 4).AddCost(Resource.Coal, 1));

            Extensions.Add(OvenRecipes, new ResourceRecipe(Resource.Bread).AddCost(Resource.Wheat, 4));
        }
    }
}
