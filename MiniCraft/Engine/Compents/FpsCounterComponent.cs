using System;
using Microsoft.Xna.Framework;

namespace MiniRealms.Engine.Compents
{
    public class FpsCounterComponent : DrawableGameComponent
    {
        public int FrameRate { get; protected set; }
        private int _frameCounter;
        private TimeSpan _elapsedTime = TimeSpan.Zero;


        public FpsCounterComponent(Game game)
            : base(game)
        {
        }


        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;
            _elapsedTime -= TimeSpan.FromSeconds(1);
            FrameRate = _frameCounter;
            _frameCounter = 0;
        }


        public override void Draw(GameTime gameTime)
        {
            _frameCounter++;
        }
    }
}
