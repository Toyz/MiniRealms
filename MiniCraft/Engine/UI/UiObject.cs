using MiniRealms.Engine.Gfx;

namespace MiniRealms.Engine.UI
{
    public abstract class UiObject
    {
        protected readonly UiManager Manager;

        protected UiObject(UiManager manager)
        {
            Manager = manager;
            //manager.Add(this);
        }

        public abstract void Tick();
        public abstract void Render(Screen screen);
    }
}
