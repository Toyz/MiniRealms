namespace MiniRealms.Screens.Interfaces
{
    public abstract class Option
    {
        public virtual string Text { get; set; }
        public virtual string SelectedText => $"> {Text} <";

        protected internal abstract void HandleInput(InputHandler input);
        protected internal abstract void HandleRender();
    }
}
