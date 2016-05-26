using System.Collections.Generic;
using System.Threading.Tasks;
using MiniRealms.Engine.UI.Objects;
using MiniRealms.Screens.GameScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.OptionItems;
using MiniRealms.Screens.UIMenus;
using Color = MiniRealms.Engine.Gfx.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace MiniRealms.Screens.MainScreens
{
    public class NewGameMenu : ScrollingMenu
    {
        private List<Option> _options;
        private WorldSizeOption _worldSizeOption;
        private DifficultyOption _difficultyOption;
        private Label _titleLabel;

        public NewGameMenu(Menu parent) : base(parent)
        {
            MaxToShow = 10;
        }

        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _worldSizeOption = new WorldSizeOption();
            _difficultyOption = new DifficultyOption();

            _options = new List<Option>
            {
                _worldSizeOption,
                _difficultyOption,
                new ActionOption("Create and Start", CreateAndStartWorld),
                new ChangeMenuOption("Cancel", Parent, game)
            };

            _titleLabel = new Label(Game.UiManager, "New Game", (GameConts.ScreenMiddleWidth - ("New Game".Length * 8 / 2)), 15, Color.White);
            Game.UiManager.Add(_titleLabel);

            RenderScrollingListTable(_options);
        }

        private void CreateAndStartWorld()
        {
            Game.LoadingText = "World Creation";
            Game.IsLoadingWorld = true;

            Point s = _worldSizeOption.Sizes[_worldSizeOption.Selected];
            GameConts.Instance.MaxHeight = s.Y;
            GameConts.Instance.MaxWidth = s.X;

            Task.Run(() =>
            {
                Game.SetupLevel(s.X, s.Y, _difficultyOption.GetDifficulty());
            }).ContinueWith((e) =>
            {
                Game.IsLoadingWorld = false;
                Game.LoadingText = string.Empty;
                Game.ResetGame();
                Game.SetMenu(new LevelTransitionMenu(3, true));
            });
        }
    }
}
