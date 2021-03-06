﻿using System;
using System.Collections.Generic;
using System.Linq;
using MiniRealms.Crafts;
using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Items;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.GameScreens
{
    public class CraftingMenu : Menu
    {
        private class RecipeSorter : IComparer<Recipe>
        {
            public int Compare(Recipe r1, Recipe r2)
            {
                if (r1.CanCraft && !r2.CanCraft) return -1;
                if (!r1.CanCraft && r2.CanCraft) return 1;
                return string.Compare(r1.ResultTemplate.Id().ToString(), r2.ResultTemplate.Id().ToString(), StringComparison.Ordinal);
            }
        }

        private readonly Player _player;
        private int _selected;

        private readonly List<Recipe> _recipes;

        public CraftingMenu(List<Recipe> recipes, Player player)
        {
            _player = player;

            for (int i = 0; i < recipes.Size(); i++)
            {
                recipes.Get(i).CheckCanCraft(player);
            }

            _recipes = new List<Recipe>(recipes.OrderByDescending(x=>x.CanCraft));
            _recipes.Sort(new RecipeSorter());
        }

        public override void Tick()
        {
            if (Input.Menu.Clicked) Game.SetMenu(null);

            if (Input.Up.Clicked) _selected--;
            if (Input.Down.Clicked) _selected++;

            int len = _recipes.Size();

            if (len == 0) _selected = 0;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;

            if (Input.Attack.Clicked && len > 0)
            {
                Recipe r = _recipes.Get(_selected);
                r.CheckCanCraft(_player);
                if (r.CanCraft)
                {
                    r.DeductCost(_player);
                    r.Craft(_player);
                    GameEffectManager.Play("craft");
                    _recipes.Sort(new RecipeSorter());
                }
                for (int i = 0; i < _recipes.Size(); i++)
                {
                    _recipes.Get(i).CheckCanCraft(_player);
                }
            }
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, "Have", 14, 2, 21, 4);
            Font.RenderFrame(screen, "Cost", 14, 5, 21, 12);
            Font.RenderFrame(screen, "Crafting", 2, 2, 13, 12);
            RenderItemList(screen, 2, 2, 13, 12, _recipes, _selected);

            if (_recipes.Size() > 0)
            {
                Recipe recipe = _recipes.Get(_selected);
                int hasResultItems = _player.Inventory.Count(recipe.ResultTemplate);
                int xo = 15 * 8;
                screen.Render(xo - 3, 3 * 8 + 2, recipe.ResultTemplate.GetSprite(), recipe.ResultTemplate.GetColor(), 0);
                Font.Draw($"{hasResultItems}", screen, xo + 8, 3 * 8 + 2, Color.Get(-1, 555, 555, 555));

                List<Item> costs = recipe.Costs;
                for (int i = 0; i < costs.Size(); i++)
                {
                    Item item = costs.Get(i);
                    int yo = (6 + i) * 8;
                    screen.Render(xo - 3, yo, item.GetSprite(), item.GetColor(), 0);
                    int requiredAmt = 1;
                    var resourceItem = item as ResourceItem;
                    if (resourceItem != null)
                    {
                        requiredAmt = resourceItem.Count;
                    }
                    int has = _player.Inventory.Count(item);
                    int color = Color.Get(-1, 555, 555, 555);
                    if (has < requiredAmt)
                    {
                        color = Color.Get(-1, 222, 222, 222);
                    }
                    if (has > 99) has = 99;
                    Font.Draw($"{has}/{requiredAmt}", screen, xo + 8 + 1, yo, color);
                }
            }
        }
    }
}
