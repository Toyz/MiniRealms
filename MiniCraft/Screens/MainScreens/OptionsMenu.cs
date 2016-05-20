using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;

namespace MiniRealms.Screens.MainScreens
{
    public class OptionsMenu : ScrollingMenu
    {
        private readonly string _backTitle;
        private static List<Option> _options;
        private ActionOption _fullScreenOption;
        private ActionOption _boardLessOption;

        public OptionsMenu(Menu parent, string backTitle = "Main Menu") : base(parent)
        {
            _backTitle = backTitle;
            MaxToShow = 10;
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _fullScreenOption = new ActionOption($"Full Screen: {(game.Gdm.IsFullScreen ? "Yes" : "No")}", FullScreenActionToggle);
            _boardLessOption = new ActionOption($"Borderless: {(game.Window.IsBorderless ? "Yes" : "No")}", SetWindowBorderlessToggle);

            _options = new List<Option>
            {
                new VolumeContol(),
                _fullScreenOption,
                _boardLessOption,
                new ChangeMenuOption(_backTitle, Parent, game)
            };

            RenderScrollingListTable(_options);
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
            Game.Gdm.IsFullScreen = !Game.Gdm.IsFullScreen;
            Game.Gdm.ApplyChanges();
            _fullScreenOption.Text = $"Full Screen: {(Game.Gdm.IsFullScreen ? "Yes" : "No")}";
            GameConts.Instance.FullScreen = Game.Gdm.IsFullScreen;
            GameConts.Instance.Save();
        }

        public override void Render(Screen screen)
        {
            base.Render(screen);

            string title = "Options";
            Font.Draw(title, screen,  GameConts.ScreenMiddleWidth - (title.Length * 8 / 2), 1 * 8, Color.White);
        }
    }
}
