using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTwoPlatformer.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Sprites
{
    public class Enemy : Sprite
    {
        //Fields
        //Movement speed
        protected int _speed;

        //Check which direction the enemy is facing (Enemies move left and right)
        protected bool _switchDirection;

        //Timer to track when enemy switches direction 
        protected double _switchCounter;

        //start position coordinates
        protected Vector2 _startPostion;

        //spritesheet locations for each enemy textue
        private int[] _spritesheetLocation = new int[4];

        //Properties
        //How many times a bomb must hit an enemy to diapear
        public int Health { get; set; }

        //Is the enemy dead
        public bool Dead { get; protected set; }

        //Tracks the points the player gets from killing an enemny
        public int KillValue { get; protected set; }

        //Checks if the score value is added to the player score. Prevents double counting in loops
        public bool KillValueCounted { get; set; }

        //Constructor
        public Enemy(Texture2D texture) : base(texture)
        {
            _texture = texture;
        }

       
        /// <summary>
        /// Update method for enemies, child classes (Fly and Snail) inherit this method since there is not difference between them other then type
        /// </summary>
        /// <param name="gameTime">Game1</param>
        /// <param name="sprites">Game1</param>
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //Check if the enemy has no health, if yes they are dead add to kill counter in GameState
            if (Health <= 0 && Dead == false)
            {
                Dead = true;
                if (this is EnemyFly)
                    GameState.KilledFlies++;
                if (this is EnemySnail)
                    GameState.KilledSnails++;

                GameState.EnemyScore += KillValue;

                return;
            }

            //If enemy is dead then return, no need to update
            if (Dead)
            {
                return;
            }

            //Method for enemy movement and switching direction 
            if (_switchDirection)
            {
                if (Position.X < _startPostion.X - 70)
                {
                    _switchDirection = false;
                    return;
                }

                Position.X -= _speed;

                if (_switchCounter / 60 >= 0.01)
                {
                    _switchCounter = 0;
                    _switchDirection = false;
                }
            }

            if (!_switchDirection)
            {
                if (Position.X > _startPostion.X + 70)
                {
                    _switchDirection = true;
                    return;
                }

                Position.X += _speed;

                if (_switchCounter / 60 >= 0.01)
                {
                    _switchCounter = 0;
                    _switchDirection = true;
                }
            }

            //Assign rectangle position
            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, 70, 70);
        }

        /// <summary>
        /// Draw method specifically for enemies. Fly and snail inherite since methods are basically the same
        /// </summary>
        /// <param name="_spritebatch"></param>
        public override void Draw(SpriteBatch _spritebatch)
        {
            //Don't draw if the enemy is dead, should disapear from the screen
            if (Dead)
                return;

            //Get the sprite location on the spritesheet depending on type of object (fly or snail)
            FindEnemySprite();

            if (_switchDirection)
            {

                _spritebatch.Draw(_texture, Position, new Rectangle(_spritesheetLocation[0], _spritesheetLocation[1], _spritesheetLocation[2], _spritesheetLocation[3]), Color.White);
            }
            else
            {
                _spritebatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, _spritesheetLocation[2], _spritesheetLocation[3]), new Rectangle(_spritesheetLocation[0], _spritesheetLocation[1], _spritesheetLocation[2], _spritesheetLocation[3]), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
            }
        }


        /// <summary>
        /// Method to find the enemy sprite location on the enemy spritesheet
        /// Different locations depending on type of enemy
        /// </summary>
        public void FindEnemySprite()
        {
            if (this is EnemySnail)
            {
                _spritesheetLocation[0] = 143;
                _spritesheetLocation[1] = 34;
                _spritesheetLocation[2] = 54;
                _spritesheetLocation[3] = 31;
            }

            if (this is EnemyFly)
            {
                _spritesheetLocation[0] = 0;
                _spritesheetLocation[1] = 32;
                _spritesheetLocation[2] = 72;
                _spritesheetLocation[3] = 36;
            }
        }

    }
}
