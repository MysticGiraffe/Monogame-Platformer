using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Sprites
{
    public class EnemyFly : Enemy
    {
        //Constructor with fly specific field values
        public EnemyFly(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;

            _startPostion = position;

            _speed = 3;

            _switchCounter = 0;

            Health = 2;

            KillValue = 250;
        }
    }
}
