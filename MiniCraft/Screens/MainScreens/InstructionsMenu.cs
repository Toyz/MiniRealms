using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;

namespace MiniRealms.Screens.MainScreens
{
    public class InstructionsMenu : Menu
    {
        private readonly Menu _parent;

        private readonly string[] _howToPlay =
        {
            "Arrow Keys to Move",
            " ",
            "C to attack",
            " ",
            "X to interact and use items",
            " ",
            " ",
            "Pick a item in your",
            " ",
            "inventory to use it.",
            " ",
            " ",
            "Kill the boss to win!"
        };

        public InstructionsMenu(Menu parent)
        {
            _parent = parent;
        }

        public override void Tick()
        {
            if (Input.Attack.Clicked || Input.Menu.Clicked)
            {
                Game.SetMenu(new AnimatedTransitionMenu(_parent));
            }
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            Font.Draw("HOW TO PLAY", screen, GameConts.ScreenMiddleWidth - ("HOW TO PLAY".Length * 8 / 2) , 1 * 8, Color.Get(0, 555, 555, 555));

            for (var line = 0; line < _howToPlay.Length; line++)
            {
                Font.Draw(_howToPlay[line], screen, GameConts.ScreenMiddleWidth - (_howToPlay[line].Length * 8 / 2 ), ((GameConts.ScreenMiddleWidth / 8) / 4 + line) * 8, Color.Get(0, 333, 333, 333));
            }

            string msg = "Press C to go back";

            Font.Draw(msg, screen, GameConts.ScreenMiddleWidth - (msg.Length * 8 / 2), screen.H - 8, Color.White);
        }
    }

}
