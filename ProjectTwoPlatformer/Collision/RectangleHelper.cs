using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Collision
{
    public static class RectangleHelper
    {
        //Check if player has touched the top of tile
        public static bool TouchTop (this Rectangle r1, Rectangle tile)
        {
            return r1.Bottom >= tile.Top
                && r1.Bottom <= tile.Top + (tile.Height / 2)
                && r1.Right >= tile.Left + (tile.Width / 10)
                && r1.Left <= tile.Right - (tile.Width / 10);
        }

        //Check if player has touched the bottom of tile
        public static bool TouchBottom (this Rectangle r1, Rectangle tile)
        {
            return r1.Top <= tile.Bottom + (tile.Height / 2)
                && r1.Top >= tile.Bottom - 1
                && r1.Right >= tile.Left + (tile.Width / 5)
                && r1.Left <= tile.Right - (tile.Width / 5);
        }

        //Check if player is touching the left of the tile
        public static bool TouchLeft (this Rectangle r1, Rectangle tile)
        {
            return r1.Right <= tile.Right
                && r1.Right >= tile.Left - 5
                && r1.Top <= tile.Bottom - (tile.Width / 4)
                && r1.Bottom >= tile.Top + (tile.Width / 4);
        }

        //Check if player is touching the right of the tile
        public static bool TouchRight (this Rectangle r1, Rectangle tile)
        {
            return r1.Left >= tile.Left 
                && r1.Left <= tile.Right + 5
                && r1.Top <= tile.Bottom - (tile.Width / 4)
                && r1.Bottom >= tile.Top + (tile.Width / 4);
        }

    }
}
