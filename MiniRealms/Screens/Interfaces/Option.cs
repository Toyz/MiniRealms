
namespace MiniRealms.Screens.Interfaces
{
    public abstract class Option
    {
        public virtual string Text { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual bool ClickSound { get; set; } = true;

        private string _selectedText;
        public virtual string SelectedText 
        {
            get { return _selectedText?.Length > 0 ? _selectedText : $"> {Text} <"; }
            set { _selectedText = value; }
        }

        public virtual int SelectedColor { get; set; } = Engine.Gfx.Color.White;

        protected internal abstract void HandleInput(InputHandler input);
        protected internal abstract void HandleRender();
    }
}
