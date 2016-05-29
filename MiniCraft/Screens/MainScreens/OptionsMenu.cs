using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI.Objects;
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
        private ActionOption _randomMucisCycle;

        private Label _titleLabel;
        private List<Option> _soundOptions;

        public OptionsMenu(Menu parent, string backTitle = "Main Menu") : base(parent)
        {
            _backTitle = backTitle;
            MaxToShow = 10;
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            ConfigMenuOptions();

            _titleLabel = new Label(Game.UiManager, "Options", (GameConts.ScreenMiddleWidth - ("Options".Length * 8 / 2)), 15, Color.White);
            Game.UiManager.Add(_titleLabel);

            RenderScrollingListTable(_options);
        }

        private void ConfigMenuOptions()
        {
            _fullScreenOption = new ActionOption($"Full Screen: {(Game.Gdm.IsFullScreen ? "Yes" : "No")}", FullScreenActionToggle);
            _boardLessOption = new ActionOption($"Borderless: {(Game.Window.IsBorderless ? "Yes" : "No")}", SetWindowBorderlessToggle);
            _randomMucisCycle = new ActionOption($"Random Music Order: {(GameConts.Instance.RandomMusicCycle ? "Yes" : "No")}", SetRandomMusicCycle);

            _soundOptions = new List<Option>
            {
                new VolumeContol(VolumeContol.SoundType.Effects, "Effects Volume: "),
                new VolumeContol(VolumeContol.SoundType.Music, "Music Volume: "),
                _randomMucisCycle,
                new ActionOption("Back to Options", () =>  RenderScrollingListTable(_options)),
            };

            _options = new List<Option>
            {
                new ActionOption("Audio Options", () =>  RenderScrollingListTable(_soundOptions)),
                _fullScreenOption,
                _boardLessOption,
                new ChangeMenuOption(_backTitle, Parent, Game)
            };
        }

        private void SetRandomMusicCycle()
        {
            GameConts.Instance.RandomMusicCycle = !GameConts.Instance.RandomMusicCycle;
            _randomMucisCycle.Text = $"Random Music Order: {(GameConts.Instance.RandomMusicCycle ? "Yes" : "No")}";
            GameConts.Instance.Save();
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
    }
}
