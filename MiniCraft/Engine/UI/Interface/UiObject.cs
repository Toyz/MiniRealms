using MiniRealms.Engine.Gfx;

namespace MiniRealms.Engine.UI.Interface
{
    public abstract class UiObject
    {
        protected readonly UiManager Manager;
        public int X { get; set; }
        public int Y { get; set; }

        protected UiObject(UiManager manager)
        {
            Manager = manager;
        }

        public abstract void Tick();
        public abstract void Render(Screen screen);
    }
}
