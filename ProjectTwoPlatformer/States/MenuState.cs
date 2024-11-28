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
    public class MenuState : State
    {
        //Fields
        //Holds list of interactable components (buttons)
        private List<Component> _components;

        //texture to hold Menu Screen title texture
        private Texture2D _title;

        //Constructor
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            //Load textures
            _title= _content.Load<Texture2D>("Title_Screen");
            Texture2D buttonTexture = _content.Load<Texture2D>("Controls/Button");

            //Load the sprite font for printing messages to the screen
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            //Start game button and assign click method
            Button startGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) - 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2) + 150),
                Text = "Start Game"
            };

            startGameButton.Click += startGameButton_Click;

            //Quit game button and assign click method
            Button quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) + 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2) + 150),
                Text = "Quit"
            };

            quitGameButton.Click += quitGameButton_Click;

            //Add buttons to list of components
            _components = new List<Component>()
            {
                startGameButton,
                quitGameButton
            };
        }

        /// <summary>
        /// Click method for the quit game button. Allows the user to exit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        /// <summary>
        /// Click method for the start game button. Moves user to the GameState which is the state that handles running the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startGameButton_Click(object sender, EventArgs e)
        {
           //load a new Game state so user can play 
           _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        /// <summary>
        /// Draw objects specific to the MenuState
        /// </summary>
        /// <param name="gameTime">GameTime from Game1</param>
        /// <param name="spriteBatch">Spritebatch from Game1</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Title texture
            spriteBatch.Draw(_title, new Vector2(0,0), Color.White);

            //Buttons
            foreach (Component component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Update objects specific to the MenuState
        /// </summary>
        /// <param name="gameTime">GameTime from Game1</param>
        public override void Update(GameTime gameTime)
        {
            //Buttons
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}
