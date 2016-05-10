using System;
using GameConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiniRealms.Engine;
using MiniRealms.Engine.Audio.Sounds;
using MiniRealms.Engine.Compents;
using MiniRealms.Engine.Gfx;
using MiniRealms.Entities;
using MiniRealms.Levels;
using MiniRealms.Levels.Tiles;
using MiniRealms.Screens.GameScreens;
using MiniRealms.Screens.Interfaces;
using MiniRealms.Screens.MainScreens;
using Color = MiniRealms.Engine.Gfx.Color;

namespace MiniRealms
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class McGame : Game
    {
        private Texture2D _image;
        private Microsoft.Xna.Framework.Color[] _pixels;
        private bool _running;

        private Microsoft.Xna.Framework.Color[] _colors;
        public int CurrentLevel = 3;
        public int GameTime;

        public bool IsGamePaused { get; set; }
        public bool IsLoadingWorld { get; set; }
        public string LoadingText { get; set; }

        public bool HasWon;
        private InputHandler _input;

        public Level Level;
        public Level[] Levels = new Level[5];
        private Screen _lightScreen;

        public Menu Menu;
        private int _pendingLevelChange;
        public Player Player;
        private int _playerDeadTime;
        public Screen Screen;
        private SpriteBatch _spriteBatch;
        public int TickCount;
        private int _wonTimer;

        public ConsoleComponent Console;
        public ConsoleCommands Cc;
        public FpsCounterComponent FpsCounterComponent;
        public readonly GraphicsDeviceManager Gdm;

        public McGame()
        {
            Gdm = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = GameConts.Height* GameConts.Instance.Scale,
                PreferredBackBufferWidth = GameConts.Width * GameConts.Instance.Scale
            };

            Cc = new ConsoleCommands(this);

            Console = new ConsoleComponent(this) {Interpreter = Cc.ManualInterpreter};
            FpsCounterComponent = new FpsCounterComponent(this);

            Components.Add(Console);
            Components.Add(FpsCounterComponent);

            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            Window.Title = $"{GameConts.Name} -- {GameConts.Version}";

            GameConts.Instance.Load();

            Window.IsBorderless = GameConts.Instance.Borderless;
            Gdm.IsFullScreen = GameConts.Instance.FullScreen;
        }

        public bool HasFocus() => IsActive;

        public void SetMenu(Menu menu)
        {
            Menu = menu;
            menu?.Init(this, _input);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _running = false;
            base.OnExiting(sender, args);
        }

        public void ResetGame()
        {
            _playerDeadTime = 0;
            _wonTimer = 0;
            GameTime = 0;
            HasWon = false;

            CurrentLevel = 3;
        }

        protected override void LoadContent()
        {
            _running = true;

            var pp = 0;
            _colors = new Microsoft.Xna.Framework.Color[256];

            for (var r = 0; r < 6; r++)
            {
                for (var g = 0; g < 6; g++)
                {
                    for (var b = 0; b < 6; b++)
                    {
                        var rr = r*255/5;
                        var gg = g*255/5;
                        var bb = b*255/5;
                        var mid = (rr*30 + gg*59 + bb*11)/100;

                        var r1 = (rr + mid*1)/2*230/255 + 10;
                        var g1 = (gg + mid*1)/2*230/255 + 10;
                        var b1 = (bb + mid*1)/2*230/255 + 10;
                        _colors[pp++] = new Microsoft.Xna.Framework.Color(r1, g1, b1);
                    }
                }
            }

            //while (pp < 256)
            //{
            //    colors[pp++] = Color.Black;
            //}

            var spriteSheet = Content.Load<Texture2D>("Textures/icons");
            Screen = new Screen(GameConts.Width, GameConts.Height, new SpriteSheet(spriteSheet));
            _lightScreen = new Screen(GameConts.Width, GameConts.Height, new SpriteSheet(spriteSheet));

            SoundEffectManager.Initialize(Content);
            SoundEffectManager.SetMasterVolume(GameConts.Instance.Volume);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixels = new Microsoft.Xna.Framework.Color[GameConts.Width  * GameConts.Height];
            _image = new Texture2D(GraphicsDevice, GameConts.Width, GameConts.Height);
            _input = new InputHandler();

            ResetGame();
            SetMenu(new TitleMenu());
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!_running) return;
            //assuming 60 updates from monogame
            Tick();

            base.Update(gameTime);
        }

        protected new virtual void Tick()
        {
            TickCount++;
            if ((!HasFocus()) && !IsLoadingWorld)
            {
                _input.ReleaseAll();
            }
            else
            {
                if (_input.FullScreen.Clicked)
                {
                    Gdm.IsFullScreen = !Gdm.IsFullScreen;

                    GameConts.Instance.BaseScaling = Gdm.IsFullScreen ? 3 : 5;

                    Gdm.ApplyChanges();
                }

                if (_input.ConsoleKey.Clicked)
                {
                    Console.ToggleOpenClose();
                }

                if (Player != null && !Player.Removed && !HasWon)
                {
                    if (!IsGamePaused)
                    {
                        GameTime++;
                    }
                }

                _input.Tick();

                if (Player != null && !Player.Removed && !HasWon)
                {
                    if (_input.CloseKey.Clicked)
                    {
                        IsGamePaused = !IsGamePaused;
                    }
                }

                if (IsGamePaused || Console.IsVisible) return;

                if (Menu != null)
                {
                    Menu.Tick();
                }
                else
                {
                    if (Player != null && Player.Removed)
                    {
                        _playerDeadTime++;
                        if (_playerDeadTime > 60)
                        {
                            SetMenu(new GameOverMenu("Game Over :: Stats", "Goto Main Menu 'C'"));
                        }
                    }
                    else
                    {
                        if (_pendingLevelChange != 0)
                        {
                            SetMenu(new LevelTransitionMenu(_pendingLevelChange));
                            _pendingLevelChange = 0;
                        }
                    }
                    if (_wonTimer > 0)
                    {
                        if (--_wonTimer == 0)
                        {
                            SetMenu(new GameOverMenu("You Won :: Stats", "Goto Main Menu 'C'"));
                        }
                    }
                    Level.Tick();
                    Tile.TickCount++;
                }
            }
        }

        public void RenderAlertWindow(string msg)
        {
            var xx = (GameConts.Width - msg.Length*8)/2;
            var yy = (GameConts.Height - 8)/2;
            var w = msg.Length;
            var h = 1;

            Screen.Render(xx - 8, yy - 8, 0 + 13*32, Color.Get(0, 1, 5, 445), 0);
            Screen.Render(xx + w*8, yy - 8, 0 + 13*32, Color.Get(0, 1, 5, 445), 1);
            Screen.Render(xx - 8, yy + 8, 0 + 13*32, Color.Get(0, 1, 5, 445), 2);
            Screen.Render(xx + w*8, yy + 8, 0 + 13*32, Color.Get(0, 1, 5, 445), 3);
            for (var x = 0; x < w; x++)
            {
                Screen.Render(xx + x*8, yy - 8, 1 + 13*32, Color.Get(0, 1, 5, 445), 0);
                Screen.Render(xx + x*8, yy + 8, 1 + 13*32, Color.Get(0, 1, 5, 445), 2);
            }
            for (var y = 0; y < h; y++)
            {
                Screen.Render(xx - 8, yy + y*8, 2 + 13*32, Color.Get(0, 1, 5, 445), 0);
                Screen.Render(xx + w*8, yy + y*8, 2 + 13*32, Color.Get(0, 1, 5, 445), 1);
            }

            Font.Draw(msg, Screen, xx, yy,
                TickCount/20%2 == 0 ? Color.Get(5, 333, 333, 333) : Color.Get(5, 555, 555, 555));
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (Player != null)
            {
                var xScroll = Player.X - Screen.W/2;
                var yScroll = Player.Y - (Screen.H - 8)/2;
                if (xScroll < 16) xScroll = 16;
                if (yScroll < 16) yScroll = 16;
                if (xScroll > Level.W*16 - Screen.W - 16) xScroll = Level.W*16 - Screen.W - 16;
                if (yScroll > Level.H*16 - Screen.H - 16) yScroll = Level.H*16 - Screen.H - 16;
                if (CurrentLevel > 3)
                {
                    var col = Color.Get(20, 20, 121, 121);
                    for (var y = 0; y < 14; y++)
                        for (var x = 0; x < 24; x++)
                        {
                            Screen.Render(x*8 - ((xScroll/4) & 7), y*8 - ((yScroll/4) & 7), 0, col, 0);
                        }
                }


                Level.RenderBackground(Screen, xScroll, yScroll);
                Level.RenderSprites(Screen, xScroll, yScroll);

                if (CurrentLevel < 3)
                {
                    _lightScreen.Clear(0);
                    Level.RenderLight(_lightScreen, xScroll, yScroll);
                    Screen.Overlay(_lightScreen, xScroll, yScroll);
                }
            }

            RenderGui();

            if (!HasFocus() && !IsLoadingWorld) RenderAlertWindow("Click to Focus");
            if (IsGamePaused) RenderAlertWindow("Game is Paused");
            if (IsLoadingWorld) RenderAlertWindow(LoadingText);

            for (var y = 0; y < Screen.H; y++)
            {
                for (var x = 0; x < Screen.W; x++)
                {
                    var cc = Screen.Pixels[x + y*Screen.W];
                    if (cc < 255) _pixels[x + y* GameConts.Width] = _colors[cc];
                }
            }
            _image.SetData(_pixels);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_image, new Rectangle(0, 0, GameConts.Width * GameConts.Instance.Scale, GameConts.Height * GameConts.Instance.Scale), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }



        private void RenderGui()
        {
            if (Player != null)
            {
                if (Player.ActiveItem != null)
                {
                    for (var y = 0; y < 1; y++)
                    {
                        for (var x = 0; x < 10; x++)
                        {
                            Screen.Render(x*8 + GameConts.ScreenMiddleWidth - 40, Screen.H - 18 + y*8, 0 + 12*32,
                                Color.Get(000, 000, 000, 000), 0);
                        }
                    }
                }

                for (var i = 0; i < 10; i++)
                {
                    Screen.Render(GameConts.ScreenMiddleWidth + i * 8 - 80, Screen.H - 9, 0 + 12*32,
                        i < Player.Health ? Color.Get(-1, 200, 500, 533) : Color.Get(-1, 100, 000, 000), 0);

                    if (Player.StaminaRechargeDelay > 0)
                    {
                        Screen.Render((i*8 + GameConts.ScreenMiddleWidth) + 5, Screen.H - 9, 1 + 12*32,
                            Player.StaminaRechargeDelay/4%2 == 0
                                ? Color.Get(-1, 555, 000, 100)
                                : Color.Get(-1, 110, 000, 100), 0);
                    }
                    else
                    {
                        Screen.Render((i * 8 + GameConts.ScreenMiddleWidth) + 5, Screen.H - 9, 1 + 12*32,
                            i < Player.Stamina
                                ? Color.Get(-1, 220, 550, 553)
                                : Color.Get(-1, 110, 000, 000),
                            0);
                    }
                }
                Player.ActiveItem?.RenderInventory(Screen, GameConts.ScreenMiddleWidth - 40, Screen.H - 18);

                if (_playerDeadTime < 60)
                {
                    int seconds = GameTime/60;
                    int minutes = seconds/60;
                    int hours = minutes/60;
                    minutes %= 60;
                    seconds %= 60;

                    var timeString = hours > 0
                        ? hours + "h" + (minutes < 10 ? "0" : "") + minutes + "m"
                        : minutes + "m " + (seconds < 10 ? "0" : "") + seconds + "s";

                    var xx = (GameConts.Width - timeString.Length*8) + 1;

                    Font.Draw(timeString, Screen, xx, 1, Color.White);

                }

                Font.Draw(GameConts.Version, Screen, (GameConts.Width - GameConts.Version.Length * 8) / 2, 1, Color.White);
            }
            Menu?.Render(Screen);
        }

        public void ChangeLevel(int dir, bool isAbsLevel = false)
        {
            Level.Remove(Player);
            if (isAbsLevel)
            {
                CurrentLevel = dir;
            }
            else
            {
                CurrentLevel += dir;
            }
            Level = Levels[CurrentLevel];
            Player.X = (Player.X >> 4)*16 + 8;
            Player.Y = (Player.Y >> 4)*16 + 8;
            Level.Add(Player);
        }

        public void ScheduleLevelChange(int dir)
        {
            _pendingLevelChange = dir;
        }

        public void Won()
        {
            _wonTimer = 60*3;
            HasWon = true;
        }

        public void SetupLevel(int lw = 128, int lh = 128)
        {
            Levels = new Level[5];

            LoadingText = "Creating Level 1";
            Levels[4] = new Level(lw, lh, 1, null);

            LoadingText = "Creating Level 2";
            Levels[3] = new Level(lw, lh, 0, Levels[4]);

            LoadingText = "Creating Level 3";
            Levels[2] = new Level(lw, lh, -1, Levels[3]);

            LoadingText = "Creating Level 4";
            Levels[1] = new Level(lw, lh, -2, Levels[2]);

            LoadingText = "Creating Level 5";
            Levels[0] = new Level(lw, lh, -3, Levels[1]);

            Level = Levels[CurrentLevel];

            LoadingText = "Spawning You!";
            Player = new Player(this, _input);
            Player.FindStartPos(Level);

            Level.Add(Player);

            foreach (Level t in Levels)
            {
                t.TrySpawn(5000);
            }

            LoadingText = "Finished";
        }
    }
}