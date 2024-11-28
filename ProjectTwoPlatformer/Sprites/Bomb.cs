using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectTwoPlatformer.Collision;
using ProjectTwoPlatformer.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Sprites
{
	public class Bomb : Sprite
	{
        //Fields
        //The bomb can only exist for a certian duration of time, then disapears
        private float _timer;

        //Enemy cannot be hit by a bomb for 5 frames. To prevent the enemy from dying from one bomb collison 
        private static int BombImmune = 5;

        //Timer for BombImmune
        private int _bombTimer;

        //Properties

        //How fast the bomb moves
        public float projectileVelocity;

        //How long can the bomb exist on screen
		public double lifeSpan;

        //Player the bomb is connected to 
		public AnimatedPlayerSprite player;

        //Has it been removed
		public bool isRemoved = false;

		

		//Constructor
		public Bomb(Texture2D texture) : base(texture)
		{
			_previousKey = _currentKey;
			_currentKey = Keyboard.GetState();
        }

        /// <summary>
        /// Update method specifically for bombs
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sprites"></param>
		public override void Update(GameTime gameTime, List<Sprite> sprites)
		{
			_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Make sure the bomb is removed after the lifespane time has occured
			if (_timer > lifeSpan)
			{
				isRemoved = true;
				return;
			}

			//If player is facing left throw bombs to the left
			if (this.Direction == "Left")
				Position.X -= projectileVelocity;
			//Else player is facing the right then throw bombs to the right 
			else
				Position.X += projectileVelocity;

            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, 70, 70);
        }

        /// <summary>
        /// Draw method specifically for bombs
        /// </summary>
        /// <param name="_spritebatch"></param>
		public override void Draw(SpriteBatch _spritebatch)
		{
			if (!this.isRemoved)
				_spritebatch.Draw(_texture, Position, Color.White);
		}

       /// <summary>
       /// Collsions method to check if a bomb has collided with any map tiles
       /// Don't want bombs to go through the tiles of the map that the player walks on
       /// </summary>
       /// <param name="tileRectangle"></param>
       /// <returns></returns>
        public bool MapCollision(Rectangle tileRectangle)
        {
            if ( _rectangle.TouchLeft(tileRectangle) || _rectangle.TouchRight(tileRectangle))
            {
				this.isRemoved = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check to see if a bomb has collided with an enemy, if yes then decrease the enemies health
        /// </summary>
        /// <param name="tileRectangle">The collison rectangle of the enemy</param>
        /// <param name="enemy">The enemy thats being checked for collison</param>
        /// <returns></returns>
        public bool EnemyCollision(Rectangle tileRectangle, Enemy enemy)
        {
            if (_rectangle.TouchLeft(tileRectangle) || _rectangle.TouchRight(tileRectangle) || _rectangle.TouchTop(tileRectangle) || _rectangle.TouchBottom(tileRectangle))
            {
                _bombTimer++;

                if (_bombTimer >= BombImmune && isRemoved == false)
                {
                    _bombTimer = 0;

                    this.isRemoved = true;

                    if (enemy is EnemySnail)
                    {
                        EnemySnail snail = (EnemySnail)enemy;
                        snail.Health--;
                    }

                    if (enemy is EnemyFly)
                    {
                        EnemyFly fly = (EnemyFly)enemy;
                        fly.Health--;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
