using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MiniCraft
{
    public class InputHandler : KeyListener
    {
        public class Key
        {            
            public int Presses, Absorbs;
            public bool Down, Clicked;

            public Key(InputHandler handler)
            {
                handler._keys.Add(this);
            }

            public void Toggle(bool pressed)
            {
                if (!Equals(pressed, Down))
                {
                    Down = pressed;
                }
                if (pressed)
                {
                    Presses++;
                }
            }

            public virtual void Tick()
            {
                if (Absorbs < Presses)
                {
                    Absorbs++;
                    Clicked = true;                  
                }
                else
                {
                    Clicked = false;
                }
            }
        }

        private readonly List<Key> _keys = new List<Key>();

        public Key CloseKey;
        public Key Up;
        public Key Down;
        public Key Left;
        public Key Right;
        public Key Attack;
        public Key Menu;

        public InputHandler()
        {
            Up = new Key(this);
            Down = new Key(this);
            Left = new Key(this);
            Right = new Key(this);
            Attack = new Key(this);
            Menu = new Key(this);
            CloseKey = new Key(this);
        }


        public void ReleaseAll()
        {
            foreach (Key t in _keys)
            {
                t.Down = false;
            }
        }

        public override void Tick()
        {
            base.Tick();

            foreach (Key t in _keys)
            {
                t.Tick();
            }
        }

        public override void KeyPressed(Keys ke)
        {
            Toggle(ke, true);
        }

        public override void KeyReleased(Keys ke)
        {
            Toggle(ke, false);
        }

        private void Toggle(Keys ke, bool pressed)
        {
            //Movement
            if (ke == Keys.NumPad8) Up.Toggle(pressed);
            if (ke == Keys.NumPad2) Down.Toggle(pressed);
            if (ke == Keys.NumPad4) Left.Toggle(pressed);
            if (ke == Keys.NumPad6) Right.Toggle(pressed);
            if (ke == Keys.W) Up.Toggle(pressed);
            if (ke == Keys.S) Down.Toggle(pressed);
            if (ke == Keys.A) Left.Toggle(pressed);
            if (ke == Keys.D) Right.Toggle(pressed);
            if (ke == Keys.Up) Up.Toggle(pressed);
            if (ke == Keys.Down) Down.Toggle(pressed);
            if (ke == Keys.Left) Left.Toggle(pressed);
            if (ke == Keys.Right) Right.Toggle(pressed);

            //Toggles
            if (ke == Keys.Tab) Menu.Toggle(pressed);
            if (ke == Keys.LeftAlt) Menu.Toggle(pressed);
            if (ke == Keys.RightAlt) Menu.Toggle(pressed);
            if (ke == Keys.Space) Attack.Toggle(pressed);
            if (ke == Keys.LeftControl) Attack.Toggle(pressed);
            if (ke == Keys.NumPad0) Attack.Toggle(pressed);
            if (ke == Keys.Insert) Attack.Toggle(pressed);
            if (ke == Keys.Enter) Menu.Toggle(pressed);

            //Actions
            if (ke == Keys.X) Menu.Toggle(pressed);
            if (ke == Keys.C) Attack.Toggle(pressed);
            if (ke == Keys.Escape) CloseKey.Toggle(pressed);
        }
    }
}
