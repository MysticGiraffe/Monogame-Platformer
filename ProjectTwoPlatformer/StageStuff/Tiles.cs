using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.StageStuff
{
	public class Tiles
	{
		//Fields
		//Texture of the tile
		protected Texture2D _texture;

		//The size of the tile (the size of the texture)
		private Rectangle rectangle;

		//The location on the spritesheet to get a specific texture
		protected Rectangle _sourceRectangle;

		public Rectangle Rectangle
		{
			get { return rectangle; }
			protected set { rectangle = value; }
		}

		private static ContentManager content;
		public static ContentManager Content
		{
			protected get { return content; }
			set { content = value; }
		}

        /// <summary>
        /// Draw method for all tile types, since method is the same for them all 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, _sourceRectangle, Color.White);
        }
    }
}
