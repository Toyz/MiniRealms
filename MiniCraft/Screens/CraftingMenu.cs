using System;
using System.Collections.Generic;
using System.Linq;
using MiniCraft.Crafts;
using MiniCraft.Entities;
using MiniCraft.Gfx;
using MiniCraft.Items;
using MiniCraft.Sounds;

namespace MiniCraft.Screens
{
    public class CraftingMenu : Menu
    {
        private class RecipeSorter : IComparer<Recipe>
        {
            public int Compare(Recipe r1, Recipe r2)
            {
                if (r1.CanCraft && !r2.CanCraft) return -1;
                if (!r1.CanCraft && r2.CanCraft) return 1;
                return string.Compare(r1.ResultTemplate.GetName(), r2.ResultTemplate.GetName(), StringComparison.Ordinal);
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
                    Sound.Craft.Play();
                }
                for (int i = 0; i < _recipes.Size(); i++)
                {
                    _recipes.Get(i).CheckCanCraft(_player);
                }
            }
        }

        public override void Render(Screen screen)
        {
            Font.RenderFrame(screen, "Have", 12, 1, 19, 3);
            Font.RenderFrame(screen, "Cost", 12, 4, 19, 11);
            Font.RenderFrame(screen, "Crafting", 0, 1, 11, 11);
            RenderItemList(screen, 0, 1, 11, 11, _recipes, _selected);

            if (_recipes.Size() > 0)
            {
                Recipe recipe = _recipes.Get(_selected);
                int hasResultItems = _player.Inventory.Count(recipe.ResultTemplate);
                int xo = 13 * 8;
                screen.Render(xo, 2 * 8, recipe.ResultTemplate.GetSprite(), recipe.ResultTemplate.GetColor(), 0);
                Font.Draw("" + hasResultItems, screen, xo + 8, 2 * 8, Color.Get(-1, 555, 555, 555));

                List<Item> costs = recipe.Costs;
                for (int i = 0; i < costs.Size(); i++)
                {
                    Item item = costs.Get(i);
                    int yo = (5 + i) * 8;
                    screen.Render(xo, yo, item.GetSprite(), item.GetColor(), 0);
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
                    Font.Draw("" + has + "/" + requiredAmt, screen, xo + 8, yo, color);
                }
            }
            // renderItemList(screen, 12, 4, 19, 11, recipes.get(selected).costs, -1);
        }
    }
}
