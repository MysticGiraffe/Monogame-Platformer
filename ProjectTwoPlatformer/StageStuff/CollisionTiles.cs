using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.StageStuff
{
	public class CollisionTiles: Tiles
	{
		//Constructor
		public CollisionTiles(Texture2D texture, Rectangle newRectangle, Rectangle spriteLocation)
		{
			_texture = texture;
			_sourceRectangle = spriteLocation;
			this.Rectangle = newRectangle;
		}
	}
}
