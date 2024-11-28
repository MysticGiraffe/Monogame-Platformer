using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Data.Common;
using Microsoft.Xna.Framework.Input;
using ProjectTwoPlatformer.Collision;
using System.Threading;
using System.Runtime.CompilerServices;
using ProjectTwoPlatformer.CameraClasses;
using ProjectTwoPlatformer.States;
using ProjectTwoPlatformer.StageStuff;

namespace ProjectTwoPlatformer.Sprites
{
    public class AnimatedPlayerSprite : Sprite
    {
        //Fields
        //Counter to make walking slower
        private int WalkingCounter = 0;

        //Speed of player movement
        private float Speed { get; set; }

        //Current frame of player walking annimation
        private int currentFrame;

        //Number of frames in animation
        private int totalFrames;

        //Create an array of 4 int to hold the location of the sprite on the atlas sheet and the size of each sprite
        //Need to do this because the sprite size and location is not uniform
        private int[] locations = new int[4];

        //Jump texture
        private Texture2D Jump;

        //Hurt texture
        private Texture2D Hurt;

        //Bool to deal with jumping off map death
        private bool _isHurt;

        //Properties

        //Number of rows in texture atlas
        public int Rows { get; set; }

        //Number of columns in texture atlas
        public int Columns { get; set; }

        //Bomb
        public Bomb Bomb;

        //Gravity
        public bool _hasjumped = true;

        //Player score
        public int PlayerScore { get; set; }

        //Check if game is over because player died
        //If yes then take player to the GameOverState
        public bool GameOver { get; protected set; }

        //Check to see if player has successfully completed the level
        //If yes then take them to the LevelClearedsState
        public bool LevelCleared { get; protected set; }


        //Constructor, assign defaluts
        public AnimatedPlayerSprite(Texture2D texture, Texture2D jumpTexture, Texture2D hurtTexture, Vector2 position, int rows, int columns, float speed) : base(texture)
        {
            _texture = texture;
            Position = position;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = 10;
            Speed = speed;
            Jump = jumpTexture;
            Hurt = hurtTexture;
            _isHurt = false;
            LevelCleared = false;
            GameOver = false;
        }

        /// <summary>
        /// Update method specifically for AnimatedPlayerSprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sprites"></param>
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //To check if user has pressed button to fire a bomb
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            //Set velocity of player, and player hitbox based on it location
            Position += Velocity;
            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, locations[0], locations[1]);

            //If the player has not died continue normal game cycle
            //else they cannot move anymore while the pause after death but before the GameOverState occurs
            if (!_isHurt)
                Input(gameTime, sprites);
            else
            {
                Velocity.X = 0;
                Velocity.Y = 0;
            }

            if (Velocity.Y < 10)
            {
                Velocity.Y += 0.35f;
            }
        }

        /// <summary>
        /// Method to deal with keyboard input and player movement/actions
        /// Player can move left using a
        /// Move right using d
        /// Jump using space bar
        /// throw a bomb with e
        /// </summary>
        /// <param name="gameTime">game time object </param>
        /// <param name="sprites">List of sprites (the bombs that exist)</param>
        private void Input(GameTime gameTime, List<Sprite> sprites)
        {
            //Counter exists so player moves at a consistant speed. This method fires multiple times a second
            //Player would move way too fast without the counter. Also helps with walk animation
            WalkingCounter++;

            //Move right
            if (_currentKey.IsKeyDown(Keys.D))
            {
                //Move sprite position forward by speed
                Velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / Speed;

                if (WalkingCounter == 2)
                {
                    WalkingCounter = 0;
                    NextFrameWalking();
                }
            }
            //Move left
            else if (_currentKey.IsKeyDown(Keys.A))
            {
                //Move sprite position forward by speed
                Velocity.X = -((float)gameTime.ElapsedGameTime.TotalMilliseconds / Speed);

                if (WalkingCounter == 2)
                {
                    WalkingCounter = 0;
                    NextFrameWalking();
                }
            }
            //Not moving
            else
            {
                Velocity.X = 0f;
                currentFrame = 0;
                WalkingCounter = 0;
                StillFrame();
            }

            //Player has jumped. Cannot jump more then once, must be on the ground to reset the jump
            if (_currentKey.IsKeyDown(Keys.Space) && _hasjumped == false)
            {
                Position.Y -= 12f;
                Velocity.Y -= 10f;
                _hasjumped = true;
            }

            //Make player throw bomb
            if (_currentKey.IsKeyDown(Keys.E) && _previousKey.IsKeyUp(Keys.E))
            {
                CreateBomb(sprites);
            }
        }

        /// <summary>
        /// Draw method specifically for AnimatedPlayerSprite
        /// </summary>
        /// <param name="_spritebatch"></param>
        public override void Draw(SpriteBatch _spritebatch)
        {
            //Rectangle that gets the loaction of the sprite from the walking player spritesheet. Locations are different based on where in the animation the player is at
            Rectangle sourceRectangle = new Rectangle(locations[2], locations[3], locations[0], locations[1]);

            //The position of the player (where to draw the sprite)
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, locations[0], locations[1]);

            //Check if the player has died
            //If yes then show hurt model, the go to game over screeen
            //If no continue with normal movement
            if (_isHurt == true)
            {
                HurtFrame();
                _spritebatch.Draw(Hurt, destinationRectangle, Color.White);
            }
            else
            {
                if (_hasjumped == true && Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    JumpFrame();
                    _spritebatch.Draw(Jump, destinationRectangle, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                }
                else if (_hasjumped == true)
                {
                    JumpFrame();
                    _spritebatch.Draw(Jump, destinationRectangle, Color.White);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _spritebatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _spritebatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    _spritebatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        /// <summary>
        /// Method to create a new bomb each time the e key is pressed
        /// </summary>
        /// <param name="sprites">The list of current bombs the player has created (can be multiple bombs on the screen)</param>
        private void CreateBomb(List<Sprite> sprites)
        {
            var bomb = Bomb.Clone() as Bomb;
            bomb.Position = this.Position;
            bomb.projectileVelocity = Speed * 2;
            bomb.lifeSpan = 0.5f;
            bomb.player = this;

            if (_currentKey.IsKeyDown(Keys.A))
                bomb.Direction = "Left";
            else
                bomb.Direction = "Right";

            sprites.Add(bomb);
        }
        
        /// <summary>
        /// Get the array locations for the still texture of player
        /// </summary>
        private void StillFrame()
        {
            locations[0] = 67;
            locations[1] = 92;

            locations[2] = 0;
            locations[3] = 0;
        }

        /// <summary>
        /// Get the array locations for the jumping texture of the player
        /// </summary>
        private void JumpFrame()
        {
            locations[0] = 67;
            locations[1] = 92;

            locations[2] = 0;
            locations[3] = 0;
        }

        /// <summary>
        /// Get the array locations for the hurt texture of the player
        /// </summary>
        private void HurtFrame()
        {
            locations[0] = 67;
            locations[1] = 92;

            locations[2] = 0;
            locations[3] = 0;
        }


        /// <summary>
        /// Method is used to assign the correct spritesheet location for each part of the walking animation
        /// The frame size varries, so everything has to be hardcodded
        /// </summary>
        private void NextFrameWalking()
        {
            currentFrame++;

            if (currentFrame > totalFrames)
            {
                currentFrame = 0;
            }

            //Create array to send back
            //sprite frame width, sprite frame height, atlas width location, atlas lenght location
            if (currentFrame == 1)
            {
                locations[0] = 67;
                locations[1] = 92;

                locations[2] = 0;
                locations[3] = 0;
            }
            if (currentFrame == 2)
            {
                locations[0] = 66;
                locations[1] = 93;

                locations[2] = 67;
                locations[3] = 0;
            }
            if (currentFrame == 3)
            {
                locations[0] = 67;
                locations[1] = 92;

                locations[2] = 133;
                locations[3] = 0;
            }
            if (currentFrame == 4)
            {
                locations[0] = 67;
                locations[1] = 93;

                locations[2] = 0;
                locations[3] = 93;
            }
            if (currentFrame == 5)
            {
                locations[0] = 66;
                locations[1] = 93;

                locations[2] = 67;
                locations[3] = 93;
            }
            if (currentFrame == 6)
            {
                locations[0] = 71;
                locations[1] = 92;

                locations[2] = 133;
                locations[3] = 93;
            }
            if (currentFrame == 7)
            {
                locations[0] = 71;
                locations[1] = 93;

                locations[2] = 0;
                locations[3] = 186;
            }
            if (currentFrame == 8)
            {
                locations[0] = 71;
                locations[1] = 93;

                locations[2] = 71;
                locations[3] = 186;
            }
            if (currentFrame == 9)
            {
                locations[0] = 66;
                locations[1] = 93;

                locations[2] = 67;
                locations[3] = 0;
            }
            if (currentFrame == 10)
            {
                locations[0] = 71;
                locations[1] = 93;

                locations[2] = 0;
                locations[3] = 279;
            }
            if (currentFrame == 11)
            {
                locations[0] = 67;
                locations[1] = 92;

                locations[2] = 71;
                locations[3] = 279;
            }
            if (currentFrame == 12)
            {
                locations[0] = 67;
                locations[1] = 92;

                locations[2] = 0;
                locations[3] = 0;
            }
        }

        /// <summary>
        /// Method to detect player collision with each individual map tile
        /// </summary>
        /// <param name="tileRectangle">The tile to check for collision </param>
        /// <param name="xOffset">The width of the map, to check that the player cannot go out of bounds on the x-axis</param>
        /// <param name="yOffset">The height of the map, to check that the player cannot go out of bounds on the y-axis</param>
        public void MapCollision(Rectangle tileRectangle, int xOffset, int yOffset)
        {
            if (_rectangle.TouchTop(tileRectangle))
            {
                _rectangle.Y = tileRectangle.Y - _rectangle.Height;
                Velocity.Y = 0f;
                _hasjumped = false;
            }

            if (_rectangle.TouchLeft(tileRectangle))
            {
                Position.X = tileRectangle.X - _rectangle.Width - 2;
            }

            if (_rectangle.TouchRight(tileRectangle))
            {
                Position.X = tileRectangle.X + tileRectangle.Width + 2;
            }

            if (_rectangle.TouchBottom(tileRectangle))
            {
                Velocity.Y += 0.5f;
            }

            //Touching the edge of the screen x axis
            if (Position.X < 0)
                Position.X = 0;
            if (Position.X > xOffset - _rectangle.Width)
                Position.X = xOffset - _rectangle.Width;

            //Touching the edge of the screen y axis
            //If the player is touching the bottom of the screen, they have died "game over"
            //This is because there are some platforming sections of the game
            if (Position.Y < 0)
                Velocity.Y = 0f;
            if (Position.Y > yOffset - _rectangle.Height)
            {
                //Set hurt bool to true
                _isHurt = true;
                GameOver = true;
                Position.Y = yOffset - _rectangle.Height;
            }

        }

        /// <summary>
        /// Method to see if the player has collided with any collectables or enemies or flag
        /// Emeny -> player dies, needs to restart game
        /// Collectable -> adds to the player score
        /// Flag -> level is cleared
        /// </summary>
        /// <param name="collectableRectangle">The collision rectangle of the other object (object or enemy or flag)</param>
        /// <param name="collisionObject">The object beging checked for collision. Specific actions depending on the type</param>
        public void Collision(Rectangle collectableRectangle, Object collisionObject)
        {
            if (_rectangle.TouchTop(collectableRectangle) || _rectangle.TouchLeft(collectableRectangle) || _rectangle.TouchRight(collectableRectangle) || _rectangle.TouchBottom(collectableRectangle))
            {
                //check if passed object is an enemy. Touch an enemy and the player looses
                if (collisionObject is Enemy)
                {
                    Enemy enemy = (Enemy)collisionObject;

                    if (enemy.Dead)
                    {
                        return;
                    }

                    _isHurt = true;
                    GameOver = true;
                }

                //check if collectable is passed. Add value to the player score
                if (collisionObject is Collectable)
                {
                    Collectable collectable = (Collectable)collisionObject;
                    collectable.isCollected = true;

                    if (!collectable.isCounted)
                    {
                        this.PlayerScore += collectable.Value;
                        collectable.isCounted = true;
                    }
                }

                //Check if the object is a flag. Player has completed the level
                if (collisionObject is Flag)
                {
                    LevelCleared = true;
                }
            }
        }
    }
}
