﻿using System;
using MiniCraft.Entities;
using MiniCraft.Gfx;

namespace MiniCraft.Items
{

    public class ToolItem : Item
    {
        private readonly Random _random = new Random();

        public const int MaxLevel = 5;
        public static readonly string[] LevelNames = { //
	        "Wood", "Rock", "Iron", "Gold", "Gem"//
	    };

        public static readonly int[] LevelColors = {//
	        ColorHelper.Get(-1, 100, 321, 431),//
			ColorHelper.Get(-1, 100, 321, 111),//
			ColorHelper.Get(-1, 100, 321, 555),//
			ColorHelper.Get(-1, 100, 321, 550),//
			ColorHelper.Get(-1, 100, 321, 055),//
	    };

        public ToolType Type;
        public int Level;

        public ToolItem(ToolType type, int level)
        {
            Type = type;
            Level = level;
        }

        public override int GetColor() => LevelColors[Level];

        public override int GetSprite() => Type.Sprite + 5 * 32;

        public override void RenderIcon(Screen screen, int x, int y, int bits = 0)
        {
            screen.Render(x, y, GetSprite(), GetColor(), bits);
        }

        public override void RenderInventory(Screen screen, int x, int y)
        {
            screen.Render(x, y, GetSprite(), GetColor(), 0);
            Font.Draw(GetName(), screen, x + 8, y, ColorHelper.Get(-1, 555, 555, 555));
        }

        public override string GetName() => LevelNames[Level] + " " + Type.Name;

        public override void OnTake(ItemEntity itemEntity)
        {
        }

        public override bool CanAttack() => true;

        public override int GetAttackDamageBonus(Entity e)
        {
            if (Type == ToolType.Axe)
            {
                return (Level + 1) * 2 + _random.NextInt(4);
            }
            if (Type == ToolType.Sword)
            {
                return (Level + 1) * 3 + _random.NextInt(2 + Level * Level * 2);
            }
            return 1;
        }

        public override bool Matches(Item item)
        {
            var toolItem = item as ToolItem;
            if (toolItem == null) return false;
            ToolItem other = toolItem;
            if (other.Type != Type) return false;
            if (other.Level != Level) return false;
            return true;
        }
    }
}