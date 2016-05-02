using MiniCraft.Gfx;

namespace MiniCraft.Screens
{
    public class InstructionsMenu : Menu
    {
        private readonly Menu _parent;

        public InstructionsMenu(Menu parent)
        {
            _parent = parent;
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

            Font.Draw("HOW TO PLAY", screen, 4 * 8 + 4, 1 * 8, Color.Get(0, 555, 555, 555));
            Font.Draw("Move your character", screen, 0 * 8 + 4, 3 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("with the arrow keys", screen, 0 * 8 + 4, 4 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("press C to attack", screen, 0 * 8 + 4, 5 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("and X to open the", screen, 0 * 8 + 4, 6 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("inventory and to", screen, 0 * 8 + 4, 7 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("use items.", screen, 0 * 8 + 4, 8 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("Select an item in", screen, 0 * 8 + 4, 9 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("the inventory to", screen, 0 * 8 + 4, 10 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("equip it.", screen, 0 * 8 + 4, 11 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("Kill the air wizard", screen, 0 * 8 + 4, 12 * 8, Color.Get(0, 333, 333, 333));
            Font.Draw("to win the game!", screen, 0 * 8 + 4, 13 * 8, Color.Get(0, 333, 333, 333));
        }
    }

}
