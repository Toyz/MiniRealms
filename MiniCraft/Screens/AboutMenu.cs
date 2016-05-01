using MiniCraft.Gfx;

namespace MiniCraft.Screens
{
    public class AboutMenu : Menu
    {
        private readonly Menu _parent;

        public AboutMenu(Menu parent)
        {
            this._parent = parent;
        }

        public override void Tick()
        {
            if (Input.Attack.Clicked || Input.Menu.Clicked)
            {
                Game.SetMenu(_parent);
            }
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            Font.Draw("About Minicraft", screen, 2 * 8 + 4, 1 * 8, ColorHelper.Get(0, 555, 555, 555));
            Font.Draw("Minicraft was made", screen, 0 * 8 + 4, 3 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("by Markus Persson", screen, 0 * 8 + 4, 4 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("For the 22'nd ludum", screen, 0 * 8 + 4, 5 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("dare competition in", screen, 0 * 8 + 4, 6 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("december 2011.", screen, 0 * 8 + 4, 7 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("it is dedicated to", screen, 0 * 8 + 4, 9 * 8, ColorHelper.Get(0, 333, 333, 333));
            Font.Draw("my father. <3", screen, 0 * 8 + 4, 10 * 8, ColorHelper.Get(0, 333, 333, 333));
        }
    }
}
