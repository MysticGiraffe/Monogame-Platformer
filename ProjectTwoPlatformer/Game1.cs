using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectTwoPlatformer.Sprites;
using ProjectTwoPlatformer.StageStuff;
using ProjectTwoPlatformer.CameraClasses;
using System;
using System.Collections.Generic;
using System.IO;
using ProjectTwoPlatformer.States;

namespace ProjectTwoPlatformer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        //This is the state that is currently running
        private State _currentState;

        //This is the state that will be called next
        private State _nextState;

        /// <summary>
        /// Method to set the next state of the game. Prepare state to be loaded
        /// </summary>
        /// <param name="state">Next state. For example menu</param>
        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Make mouse visable for button pressing
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Set current state to menu by defalut when game starts
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Change the state of the game if next state is not null
            if (_nextState != null)
            {
                _currentState = _nextState;

                //Clear nextstate since we only change states when it is not empty
                _nextState = null;
            }

            //Update the current game state. Call current state update method
            _currentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //Call the current game state draw method
            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}