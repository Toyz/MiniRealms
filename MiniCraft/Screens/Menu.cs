using System.Collections.Generic;
using System.Linq;
using MiniCraft.Gfx;

namespace MiniCraft.Screens
{
    public class Menu
    {
        protected McGame Game;
        protected InputHandler Input;

        public void Init(McGame game, InputHandler input)
        {
            Input = input;
            Game = game;
        }

        public virtual void Tick()
        {
        }

        public virtual void Render(Screen screen)
        {
        }

        public void RenderItemList(Screen screen, int xo, int yo, int x1, int y1, IEnumerable<IListItem> listItemsIter, int selected)
        {
            var listItems = listItemsIter.ToArray();

            bool renderCursor = true;
            if (selected < 0)
            {
                selected = -selected - 1;
                renderCursor = false;
            }
            int w = x1 - xo;
            int h = y1 - yo - 1;
            int i0 = 0;
            int i1 = listItems.Length;
            if (i1 > h) i1 = h;
            int io = selected - h / 2;
            if (io > listItems.Length - h) io = listItems.Length - h;
            if (io < 0) io = 0;

            for (int i = i0; i < i1; i++)
            {
                listItems[i + io].RenderInventory(screen, (1 + xo) * 8, (i + 1 + yo) * 8);
            }

            if (!renderCursor) return;
            int yy = selected + 1 - io + yo;
            Font.Draw(">", screen, (xo + 0) * 8, yy * 8, Color.Get(5, 555, 555, 555));
            Font.Draw("<", screen, (xo + w) * 8, yy * 8, Color.Get(5, 555, 555, 555));
        }
    }
}
