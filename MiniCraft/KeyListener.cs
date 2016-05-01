using Microsoft.Xna.Framework.Input;

namespace MiniCraft
{
    public class KeyListener
    {
        private readonly bool[] _keys = new bool[256];
        private KeyboardState _state;

        public KeyListener()
        {
            _state = Keyboard.GetState();
        }

        public virtual void Tick()
        {
            _state = Keyboard.GetState();

            for (var i = 0; i < 256; i++)
            {
                var key = (Keys)i;
                if(_keys[i] && _state.IsKeyUp(key))
                {
                    _keys[i] = false;
                    KeyReleased(key);
                }
                else if (!_keys[i] && _state.IsKeyDown(key))
                {
                    _keys[i] = true;
                    KeyPressed(key);
                }
            }
        }

        public virtual void KeyPressed(Keys ke)
        {
        }

        public virtual void KeyReleased(Keys ke)
        {
        }
    }
}
