using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectTwoPlatformer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.States
{
    public class GameOverState : State
    {
        //Fields 
        //Components  (buttons)
        private List<Component> _components;

        //Final score of user
        private int _playerFinalScore;

        //font for displaying text
        private SpriteFont _font;

        //Total time player spent in GameState
        private double _totalGameTime;

        //The graphics device from Game1 and GameState
        private GraphicsDevice _device;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, double time, int finalScore) : base(game, graphicsDevice, content)
        {
            //Assign parameters to private variables
            _playerFinalScore = finalScore;

            _device = graphicsDevice;

            _totalGameTime = time;

            //Load texture and font
            Texture2D buttonTexture = _content.Load<Texture2D>("Controls/Button");
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            _font = buttonFont;

            //Retry button and click event
            Button restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) - 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2)),
                Text = "Play Again?"
            };

            restartButton.Click += RestartButton_Click;

            //Quit button and click event
            Button quitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) + 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2)),
                Text = "Quit"
            };

            quitButton.Click += QuitButton_Click;

            //Load buttons into component list
            _components = new List<Component>()
            {
                restartButton,
                quitButton
            };
        }

        /// <summary>
        /// Draw Method specific to GameOver state
        /// </summary>
        /// <param name="gameTime">From Game1</param>
        /// <param name="spriteBatch">from Game1</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //You died message. Includes score and time
            spriteBatch.DrawString(_font, $"You Died!\n\nScore: {_playerFinalScore}\nTime: {Math.Round(_totalGameTime, 2)}s.", new Vector2((_device.Viewport.Width / 2) - 45, (_device.Viewport.Height / 2 ) - 150), Color.Black);

            //Buttons
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Update specific to GameOverState
        /// </summary>
        /// <param name="gameTime">From Game1</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        /// <summary>
        /// Quit button click Method to let user exit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        /// <summary>
        /// Restart button click method that sends user back to game state to play game again from begining (nothing saves between each round)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartButton_Click(object sender, EventArgs e)
        {
            //Load the GameState State again
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
    }
}
