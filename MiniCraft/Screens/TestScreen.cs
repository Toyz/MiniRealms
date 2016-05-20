using System;
using System.Collections.Generic;
using MiniRealms.Engine.Gfx;
using MiniRealms.Screens.MainScreens;

namespace MiniRealms.Screens
{
    public class TestScreen : Interfaces.Menu
    {
        private readonly Interfaces.Menu _parent;

        private class RenderColorTest
        {
            public int Color;
            public string Hex;
        }

        private readonly List<RenderColorTest> _options = new List<RenderColorTest>();
        private readonly Random _rand = new Random();

        public TestScreen(Interfaces.Menu parent)
        {
            _parent = parent;
        }


        public override void Init(McGame game, InputHandler input)
        {
            base.Init(game, input);

            _options.Clear();
            for (var i = 0; i < GameConts.Height / 8 + 10; i++)
            {
                var a = -1;
                var b = _rand.Next(1, 555) + 1;
                var c = _rand.Next(1, 555) + 1;
                var d = _rand.Next(1, 555) + 1;

                _options.Add(new RenderColorTest
                {
                    Color = Color.Get(a, b, c, d),
                    Hex = Color.GetHex(a, b, c, d)
                });
            }
        }

        public override void Tick()
        {
            if (Input.Menu.Clicked)
            {
                Game.SetMenu(new AnimatedTransitionMenu(_parent));
            }

            if (Game.TickCount%60 == 0)
            {
                foreach (RenderColorTest t in _options)
                {
                    var a = -1;
                    var b = _rand.Next(1, 555) + 1;
                    var c = _rand.Next(1, 555) + 1;
                    var d = _rand.Next(1, 555) + 1;
                    t.Color = Color.Get(a, b, c, d);
                    t.Hex = Color.GetHex(a, b, c, d);
                    /*_options[i] = new RenderColorTest
                    {
                        Color = Color.Get(a, b, c, d),
                        Hex = Color.GetHex(a, b, c, d)
                    };*/
                }
            }
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);
            for (var i = 0; i < _options.Count; i++)
            {
                var s = _options[i].Hex + " - " + _options[i].Color;
                Font.Draw(s, screen, (screen.W - s.Length * 8) / 2, (GameConts.ScreenMiddleHeight + (i * 10) - ((_options.Count * 8) / 2)), _options[i].Color);
            }
        }
    }
}
