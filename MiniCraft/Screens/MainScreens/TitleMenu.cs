using System.Collections.Generic;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.DebugScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;

namespace MiniRealms.Screens.MainScreens
{
    public class TitleMenu : Menu
    {
        public bool ShowErrorAlert { get; set; }
        public string ErrorAlertBody { get; set; }

        private int _selected;

        private static List<Option> _options = new List<Option>();


        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _options = new List<Option>
            {
                new ChangeMenuOption("New Game", new NewGameMenu(this), Game),
                new ChangeMenuOption("How to play", new InstructionsMenu(this), Game),
#if DEBUG
                new LabelOption("Mods and Addons") {Enabled = false},
                new ChangeMenuOption("Debug And Testing", new DebugMenu(this), Game),
#endif
                new ChangeMenuOption("Options", new OptionsMenu(this), Game),
                new ActionOption("Exit", () => Game.Exit()) { ClickSound = false }
            };
        }

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;

            if (Input.CloseKey.Clicked) ShowErrorAlert = false;

            if (Input.Up.Clicked)
            {
                _selected--;
                ShowErrorAlert = false;
                SoundEffectManager.Play("menu_move");
            }
            if (Input.Down.Clicked)
            {
                SoundEffectManager.Play("menu_move");
                _selected++;
                ShowErrorAlert = false;
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
                Option option = _options[i];
                string msg = option.Text;
                int col = Color.DarkGrey;
                if (i == _selected )
                {

                    msg = option.SelectedText;
                    col = option.SelectedColor;
                    if (!option.Enabled)
                    {
                        col = Color.DarkGrey;
                    }
                    option.HandleRender();
                }
                Font.Draw(msg, screen, (screen.W - msg.Length * 8) / 2, (GameConts.ScreenThirdHeight + (i * 10) - ((_options.Count * 8) / 2)), col);
            }

            var xx = (GameConts.Width - "(Arrow keys,X and C)".Length * 8) / 2;

            Font.Draw("(Arrow keys,X and C)", screen, xx, screen.H - 8, Color.DarkGrey);

            if (ShowErrorAlert && !Game.IsLoadingWorld)
            {
                Game.RenderAlertWindow(ErrorAlertBody);
            }
        }
    }
}
