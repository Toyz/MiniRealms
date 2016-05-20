namespace MiniRealms.Screens.Interfaces
{
    public abstract class Option
    {
        public virtual string Text { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual string SelectedText => $"> {Text} <";
        public virtual int SelectedColor { get; set; } = Engine.Gfx.Color.White;

        protected internal abstract void HandleInput(InputHandler input);
        protected internal abstract void HandleRender();
    }
}
