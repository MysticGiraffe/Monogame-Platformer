using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTwoPlatformer.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Sprites
{
    public class EnemySnail : Enemy
    {   
        //constructor with snail specific field values
        public EnemySnail(Texture2D texture, Vector2 position) : base(texture)
        {

            Position = position;

            _startPostion = position;

            _speed = 2;

            _switchCounter = 0;

            Health = 3;

            KillValue = 450;
        }
    }
}
