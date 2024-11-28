using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Sprites
{
    public class Sprite : ICloneable
    {
        //sprite texture
        protected Texture2D _texture;

        //users current pressed key, for movement
        protected KeyboardState _currentKey;

        //Users previous pressed key, for movement
        protected KeyboardState _previousKey;

        //sprites cordinate position
		public Vector2 Position;

        //sprites movement velocity
        public Vector2 Velocity;

        //The direction thet sprite is facing
        public string Direction;

        //For collision
        public Rectangle _rectangle;

        //Constructor
		public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        /// <summary>
        /// Update method for sprites, will be override in child classes
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sprites"></param>
        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }

        /// <summary>
        /// Draw method for sprites, will be override in child classes
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Clone method. Mostly used for creating bombs the player throws
        /// </summary>
        /// <returns></returns>
		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
