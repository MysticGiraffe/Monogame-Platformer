using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.States
{
    public abstract class State
    {
        //Fields
        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected Game1 _game;

        //Constructor
        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        //Methods for child classes to inherent and expand upon
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        

        public abstract void Update(GameTime gameTime);
    }
}
