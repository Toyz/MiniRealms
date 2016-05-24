using MiniRealms.Engine.Gfx;
using MiniRealms.Engine.UI.Interface;

namespace MiniRealms.Engine.UI.Objects
{
    public class Label : UiObject
    {
        public string Text { get; set; }
        public int Color { get; set; } = Gfx.Color.White;

        public Label(UiManager manager) : base(manager)
        {
        }

        public Label(UiManager manager, string text, int x, int y, int color) : base(manager)
        {
            Text = text;
            X = x;
            Y = y;
            Color = color;
        }

        public override void Tick()
        {
        }

        public override void Render(Screen screen)
        {
            Font.Draw(Text, screen, X, Y, Color);
        }
    }
}
