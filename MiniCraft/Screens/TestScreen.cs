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

        private readonly List<RenderColorTest> _rTest = new List<RenderColorTest>();
        public TestScreen(Interfaces.Menu parent)
        {
            _parent = parent;

            var rand = new Random();
            for (var i = 0; i < 10; i++)
            {
                var a = -1;
                var b = rand.Next(1, 555) + 1;
                var c = rand.Next(1, 555) + 1;
                var d = rand.Next(1, 555) + 1;

                _rTest.Add(new RenderColorTest
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
        }

        public override void Render(Screen screen)
        {
            screen.Clear(0);
            for (var i = 0; i < _rTest.Count; i++)
            {
                var s = _rTest[i].Hex + " - " + _rTest[i].Color;
                Font.Draw(s, screen, (GameConts.ScreenMiddleWidth - (s.Length * 8 / 2)), GameConts.ScreenMiddleHeight - (i * 10) + 25, _rTest[i].Color);
            }
        }
    }
}
