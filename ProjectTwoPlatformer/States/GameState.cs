using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectTwoPlatformer.CameraClasses;
using ProjectTwoPlatformer.Controls;
using ProjectTwoPlatformer.Sprites;
using ProjectTwoPlatformer.StageStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectTwoPlatformer.States
{
    public class GameState : State
    {
        //Fields
        //List of all sprites created once state is loaded. Includes player, collectables, and enemies
        private List<Sprite> _sprites;

        //Camera object. So player is always in the middle of the screen
        private Camera _camera;

        //Map object
        private Map _map;

        //Timer that starts after player dies. Provides delay before GameOverState is called
        private double _gameOverTime;

        //Timer that tracks the amount of time the player has been in this state. How long it takes them to complete level
        private double _timeToClear;

        //Properties
        //Sprite font for text
        public static SpriteFont _font;

        //Number of coins in the level
        public static int TotalCoins;

        //Number of fly enemies in the level
        public static int TotalFlies;

        //Number of snail enemies in level
        public static int TotalSnails;

        //Number of flies player has killed
        public static int KilledFlies;

        //Number of snails player has killed
        public static int KilledSnails;

        //Number of coins player has collected
        public static int CollectedCoins;

        //Number of green gems player has collected
        public static int CollectedGreenGem;

        //Number of Blue gems player has collected
        public static int CollectedBlueGem;

        //Number of Yellow gems player has collected
        public static int CollectedYellowGem;

        //Number of Red gems player has collected
        public static int CollectedRedGem;

        //Tracks the score gained from killing enemies. Added to player score tracked in animatedplayersprite
        public static int EnemyScore;

        //Constructor
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            //Set graphicsDevice from Game1 to private variable
            _graphicsDevice = graphicsDevice;

            //Set content for tiles
            Tiles.Content = content;

            //Create new map object
            _map = new Map();

            //Reset collectable counts (needed when player restarts the game)
            _timeToClear = 0;
            CollectedCoins = 0;
            CollectedGreenGem = 0;
            CollectedBlueGem = 0;
            CollectedYellowGem = 0;
            CollectedRedGem = 0;
            TotalFlies = 0;
            TotalSnails = 0;
            KilledFlies = 0;
            KilledSnails = 0;
            EnemyScore = 0;
            _gameOverTime = 0f;

            //Player textures
            Texture2D texture = content.Load<Texture2D>("p1_walk");
            Texture2D jumpTexture = content.Load<Texture2D>("p1_jump");
            Texture2D hurtTexture = content.Load<Texture2D>("p1_hurt");

            //Enemy spritesheet
            Texture2D enemies = content.Load<Texture2D>("enemies_spritesheet");

            //Font for score
            _font = content.Load<SpriteFont>("Fonts/Font");

            //Collectable Textures
            Texture2D collectableTexture = content.Load<Texture2D>("items_spritesheet");

            //Map textures
            Texture2D mapTile = content.Load<Texture2D>("tiles_spritesheet");

            //Create new camera
            _camera = new Camera(graphicsDevice.Viewport);

            //Get the int array that holds the map build
            int[,] mapTiles = GenerateTileMap.MapLayout();

            //Generate the level tile map
            _map.Generate(mapTiles, 70, mapTile, collectableTexture);

            //Get the total number of coins generated on the map
            TotalCoins = _map.Coins;

            //Create a new list of sprites
            _sprites = new List<Sprite>()
            {
                //Create a new player
                new AnimatedPlayerSprite(texture, jumpTexture, hurtTexture, new Vector2(150, 70*16), 4, 3, 4f)
                {
                    //Bombs that player can throw
                    Bomb = new Bomb(content.Load<Texture2D>("bomb"))
                },

                //Create enemy snails
                new EnemySnail(enemies, new Vector2(9*70, (70*17) + 40)),
                new EnemySnail(enemies, new Vector2(900, (70*8) + 40)),
                new EnemySnail(enemies, new Vector2(23*70, (70*17) + 40)),
                new EnemySnail(enemies, new Vector2(45*70, (70*11) + 40)),
                new EnemySnail(enemies, new Vector2(58*70, (70*11) + 40)),

                //Create enemy flies
                new EnemyFly(enemies, new Vector2(16*70, 70*15)),
                new EnemyFly(enemies, new Vector2(8*70, 70*6)),
                new EnemyFly(enemies, new Vector2(24*70, 70*16)),
                new EnemyFly(enemies, new Vector2(30*70, 70*14)),
                new EnemyFly(enemies, new Vector2(52*70, 70*10)),
            };

            //Count the number of each enemy created (for clear level stats)
            foreach (Sprite sprite in _sprites)
            {
                if (sprite is EnemySnail)
                    TotalSnails++;
                if (sprite is EnemyFly)
                    TotalFlies++;
            }
        }

        /// <summary>
        /// Draws content specific to the GameState
        /// </summary>
        /// <param name="gameTime">From Game1</param>
        /// <param name="spriteBatch">From Game1</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Make sure to reference Camera method, so that player is always on the screen, usually the middle
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, null, null, null, _camera.Transform);

            //Draw map
            _map.Draw(spriteBatch);

            //Draw all sprites in list
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Update method specific to GameState
        /// </summary>
        /// <param name="gameTime">From Game1</param>
        public override void Update(GameTime gameTime)
        {
            //Track time it takes player to clear or fail level
            _timeToClear += gameTime.ElapsedGameTime.TotalSeconds;

            //Update each sprite in sprite array
            foreach (var sprite in _sprites.ToArray())
            {
                sprite.Update(gameTime, _sprites);

                //Extra if sprite is the player
                if (sprite is AnimatedPlayerSprite)
                {
                    AnimatedPlayerSprite player = (AnimatedPlayerSprite)sprite;

                    //check for collision
                    PlayerCollision(player);

                    //Check if game over (player died), if so get player score/time and send them to the GameOverState
                    if (player.GameOver == true)
                    {
                        //Create a short dely before being brought to the game over screen
                        _gameOverTime += gameTime.ElapsedGameTime.TotalSeconds;

                        if (_gameOverTime > 0.5)
                        {
                            CollectedCollectables();
                            player.PlayerScore += EnemyScore;
                            _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _timeToClear, player.PlayerScore));
                        }
                    }
                    //Check if player successfully cleared the level, if so get player score/time and send to the ClearLevelState
                    else if (player.LevelCleared == true)
                    {
                        //Create a short dely before being brought to the game over screen
                        _gameOverTime += gameTime.ElapsedGameTime.TotalSeconds;

                        if (_gameOverTime > 0.5)
                        {
                            CollectedCollectables();
                            player.PlayerScore += EnemyScore;
                            _game.ChangeState(new ClearLevelState(_game, _graphicsDevice, _content, _timeToClear, player.PlayerScore));
                        }
                    }
                }
                //Check if the sprite is a bomb
                if (sprite is Bomb)
                {
                    Bomb bomb = (Bomb)sprite;
                    //Check for collision
                    BombCollision(bomb);
                }
            }
        }

        /// <summary>
        /// Method to check for different types of bomb collison. Could be with the map tiles, or an enemy
        /// </summary>
        /// <param name="bomb">The bomb object we are checking for collison </param>
        private void BombCollision(Bomb bomb)
        {
            //Check each map tile
            foreach (CollisionTiles tile in _map.CollisionTiles)
            {
                bomb.MapCollision(tile.Rectangle);
            }

            //Check each enemy
            foreach (Sprite sprite in _sprites)
            {
                if (sprite is Enemy)
                {
                    Enemy enemy = (Enemy)sprite;

                    if (enemy.Dead)
                        continue;

                    bomb.EnemyCollision(sprite._rectangle, enemy);
                }
            }
        }

        /// <summary>
        /// Method to check player collison. Could be with map tiles, enemies, and collectables
        /// </summary>
        /// <param name="player">The player object to check collison for</param>
        private void PlayerCollision(AnimatedPlayerSprite player)
        {
            //Check collision with map tiles
            foreach (CollisionTiles tile in _map.CollisionTiles)
            {
                player.MapCollision(tile.Rectangle, _map.Width, _map.Height);
                _camera.Update(player.Position, _map.Width, _map.Height);
            }

            //Collision with collectables
            foreach (Collectable collectable in _map.CollectableTiles)
            {
                player.Collision(collectable.Rectangle, collectable);
            }

            //Check collision with end of level flag
            foreach (Flag flag in _map.FlagTiles)
            {
                player.Collision(flag.Rectangle, flag);
            }

            //Check collision with enemies
            foreach (Sprite sprite in _sprites)
            {
                if (sprite is Enemy)
                {
                    Enemy enemy = (Enemy)sprite;
                    player.Collision(sprite._rectangle, enemy);
                }
            }
        }

        /// <summary>
        /// Method to assign the collectable counter to a numbr for Clear Level stats. Counts the number of each type of collectable player has collected (collided with)
        /// </summary>
        private void CollectedCollectables()
        {
            foreach (Collectable c in _map.CollectableTiles)
            {
                if (c.GetType() == typeof(Coin))
                {
                    Coin coin = (Coin)c;
                    if (coin.isCollected == true)
                        CollectedCoins++;
                }

                if (c.GetType() == typeof(Gem))
                {
                    Gem gem = (Gem)c;
                    if (gem.isCollected == true)
                    {
                        if (gem.Colour == "Green")
                            CollectedGreenGem++;
                        if (gem.Colour == "Blue")
                            CollectedBlueGem++;
                        if (gem.Colour == "Yellow")
                            CollectedYellowGem++;
                        if (gem.Colour == "Red")
                            CollectedRedGem++;
                    }

                }
            }
        }
    }
}
