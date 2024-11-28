using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.CameraClasses
{
    public class Camera
    {
        //Fields
        //view that is moved to follow the player
        private Matrix _transform;

        //Center of the screen
        private Vector2 _center;

        //Viewport
        private Viewport _viewport;

        //Properties

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix Transform
        {
            get { return _transform; }
        }

        /// <summary>
        /// Update for camera. Makes sure that player is always on the screen. In center except when nearing the
        /// edges of the map where the camera stops following the player 
        /// Offsets are to ensure the camera does not move beyond the limits of the map
        /// </summary>
        /// <param name="postion">Position of the players sprite character</param>
        /// <param name="xOffset">Width of the screen</param>
        /// <param name="yOffset">Height of the screen</param>
        public void Update(Vector2 postion, int xOffset, int yOffset)
        {
            if (postion.X < _viewport.Width / 2)
            {
                _center.X = _viewport.Width / 2;
            }
            else if (postion.X > xOffset - (_viewport.Width / 2))
            {
                _center.X = xOffset - (_viewport.Width / 2);
            }
            else
                _center.X = postion.X;

            if (postion.Y < _viewport.Height / 2)
            {
                _center.Y = _viewport.Height / 2;
            }
            else if (postion.Y > yOffset - (_viewport.Height / 2))
            {
                _center.Y = yOffset - (_viewport.Height / 2);
            }
            else
                _center.Y = postion.Y;

            //Create a matrix to follow the player. Camera following the player
            _transform = Matrix.CreateTranslation(new Vector3(-_center.X + (_viewport.Width / 2),
                                                              -_center.Y + (_viewport.Height / 2), 0));
        }

    }
}
