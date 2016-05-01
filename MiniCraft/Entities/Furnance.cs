﻿using MiniCraft.Crafts;
using MiniCraft.Gfx;
using MiniCraft.Screens;

namespace MiniCraft.Entities
{
    public class Furnace : Furniture
    {
        public Furnace()
            : base("Furnace")
        {
            Col = ColorHelper.Get(-1, 000, 222, 333);
            Sprite = 3;
            Xr = 3;
            Yr = 2;
        }

        public override bool Use(Player player, int attackDir)
        {
            player.Game.SetMenu(new CraftingMenu(Crafting.FurnaceRecipes, player));
            return true;
        }
    }
}