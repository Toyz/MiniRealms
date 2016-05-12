using System.Collections.Generic;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;

namespace MiniRealms.Screens.MainScreens
{
    public class OptionsMenu : Menu
    {
        private readonly McGame _game;

        private int _selected;

        private static List<Option> _options;
        private readonly ActionOption _fullScreenOption;
        private readonly ActionOption _boardLessOption;

        public OptionsMenu(Menu parent, McGame game)
        {
            _game = game;
            _fullScreenOption = new ActionOption($"Full Screen: {(game.Gdm.IsFullScreen ? "Yes" : "No")}", FullScreenActionToggle);
            _boardLessOption = new ActionOption($"Borderless: {(game.Window.IsBorderless ? "Yes" : "No")}", SetWindowBorderlessToggle);

            _options = new List<Option>
            {
                new VolumeContol(),
                _fullScreenOption,
                _boardLessOption,
                new ActionOption("Main Menu", () => Game.SetMenu(new AnimatedTransitionMenu(parent,  color: Color.DarkGrey)))
            };
        }

        private void SetWindowBorderlessToggle()
        {
            Game.Window.IsBorderless = !Game.Window.IsBorderless;
            _boardLessOption.Text = $"Borderless: {(Game.Window.IsBorderless ? "Yes" : "No")}";
            GameConts.Instance.Borderless = Game.Window.IsBorderless;
            GameConts.Instance.Save();
        }

        private void FullScreenActionToggle()
        {
            McGame mcGame = _game;

            mcGame.Gdm.IsFullScreen = !mcGame.Gdm.IsFullScreen;
            mcGame.Gdm.ApplyChanges();
            _fullScreenOption.Text = $"Full Screen: {(mcGame.Gdm.IsFullScreen ? "Yes" : "No")}";
            GameConts.Instance.FullScreen = mcGame.Gdm.IsFullScreen;
            GameConts.Instance.Save();
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

            string title = "Options";
            Font.Draw(title, screen,  GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);

            for (int i = 0; i < _options.Count; i++)
            {
                Option option = _options[i];
                string msg = option.Text;
                int col = Color.DarkGrey;
                if (i == _selected)
                {
                    msg = option.SelectedText;
                    col = Color.White;
                    option.HandleRender();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, GameConts.ScreenMiddleHeight + (i * 8) - 20, col);
            }
        }
    }
}
