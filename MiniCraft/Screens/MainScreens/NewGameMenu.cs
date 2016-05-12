using System.Collections.Generic;
using System.Threading.Tasks;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using Color = MiniRealms.Engine.Gfx.Color;
using Font = MiniRealms.Engine.Gfx.Font;
using Point = Microsoft.Xna.Framework.Point;

namespace MiniRealms.Screens.MainScreens
{
    public class NewGameMenu : Menu
    {
        private int _selected;
        private readonly List<Option> _options;
        private readonly WorldSizeOption _worldSizeOption;

        public NewGameMenu(Menu parent)
        {
            _worldSizeOption = new WorldSizeOption();
            _options = new List<Option>
            {
                _worldSizeOption,
                new ActionOption("Create and Start", CreateAndStartWorld),
                new ActionOption("Cancel", () => Game.SetMenu(new AnimatedTransitionMenu(parent,  color: Color.DarkGrey)))
            };
        }

        private void CreateAndStartWorld()
        {
            Game.LoadingText = "World Creation";
            Game.IsLoadingWorld = true;
            Game.CurrentLevel = 3;

            Point s = _worldSizeOption.Sizes[_worldSizeOption.Selected];
            GameConts.Instance.MaxHeight = s.Y;
            GameConts.Instance.MaxWidth = s.X;

            Task.Run(() =>
            {
                Game.SetupLevel(s.X, s.Y);
            }).ContinueWith((e) =>
            {
                Game.IsLoadingWorld = false;
                Game.LoadingText = string.Empty;
                Game.ResetGame();
                Game.SetMenu(null);
            });
        }

        public override void Tick()
        {
            if (Game.IsLoadingWorld) return;
            
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

            string title = "New Game";
            Font.Draw(title, screen, GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);

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
