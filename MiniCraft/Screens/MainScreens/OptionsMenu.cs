using System.Collections.Generic;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.Options;

namespace MiniRealms.Screens.MainScreens
{
    public class OptionsMenu : Menu
    {
        private readonly McGame _game;

        private int _selected;

        private static List<IOption> _options;
        private readonly ActionOption _fullScreenOption;
        private readonly ActionOption _boardLessOption;

        public OptionsMenu(Menu parent, McGame game)
        {
            _game = game;
            _fullScreenOption = new ActionOption($"Full Screen: {(game.Gdm.IsFullScreen ? "Yes" : "No")}", FullScreenActionToggle);
            _boardLessOption = new ActionOption($"Borderless: {(game.Window.IsBorderless ? "Yes" : "No")}", SetWindowBorderlessToggle);

            _options = new List<IOption>
            {
                new VolumeContol(),
                _fullScreenOption,
                _boardLessOption,
                new ActionOption("Main Menu", () => Game.SetMenu(parent))
            };
        }

        private void SetWindowBorderlessToggle()
        {
            Game.Window.IsBorderless = !Game.Window.IsBorderless;
            _boardLessOption.Text = $"Borderless: {(Game.Window.IsBorderless ? "Yes" : "No")}";
        }

        public void FullScreenActionToggle()
        {
            McGame mcGame = _game;

            mcGame.Gdm.IsFullScreen = !mcGame.Gdm.IsFullScreen;
            mcGame.Gdm.ApplyChanges();
            _fullScreenOption.Text = $"Full Screen: {(mcGame.Gdm.IsFullScreen ? "Yes" : "No")}";
            _boardLessOption.Enabled = !Game.Gdm.IsFullScreen;
        }

        public override void Tick()
        {
            if (Input.Up.Clicked)
            {
                _selected--;
                SoundEffectManager.Play("menu_move");
            }

            if (Input.Down.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected++;
            }

            int len = _options.Count;
            if (_selected < 0) _selected += len;
            if (_selected >= len) _selected -= len;


            _options[_selected].HandleInput(Input);
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);

            Font.Draw(GameConts.Name, screen, (screen.W - GameConts.Name.Length * 8) / 2, 20, Game.TickCount / 20 % 2 == 0 ? Color.White : Color.Yellow);

            for (int i = 0; i < _options.Count; i++)
            {
                string msg = _options[i].Text;
                int col = Color.DarkGrey;
                if (i == _selected)
                {
                    msg = "> " + msg + " <";
                    col = Color.White;
                    _options[i].HandleRender();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, GameConts.ScreenMiddleHeight + (i * 8) - 20, col);
            }
        }
    }
}
