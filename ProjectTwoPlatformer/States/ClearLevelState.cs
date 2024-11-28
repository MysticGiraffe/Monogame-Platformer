using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectTwoPlatformer.Controls;
using ProjectTwoPlatformer.Managers;
using ProjectTwoPlatformer.Models;
using ProjectTwoPlatformer.StageStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.States
{
    public class ClearLevelState : State
    {
        //Fields

        //ScoreManager object for saving player scores
        private ScoreManager _scoreManager;

        //Time the player spent in GameState
        private double _totalGameTime;

        //Where the players score falls when ordered from highest to lowest
        private int scoreRank;

        //Final score of player
        private int _playerFinalScore;

        //The entered player name to be saved with score
        private string _playerName = "";

        //Has player entered a name
        private bool _enterName;

        //Font for text
        private SpriteFont _font;

        //Track what the user is typing 
        private KeyboardState _currentKey;
        private KeyboardState _previousKey;

        //List to hold components (buttons)
        private List<Component> _components;

        //Buttons for user to enter typed name
        private Button submitPlayerName;

        //From Game1
        private GraphicsDevice _device;

        //width of button texture
        private float _buttonWidth;

        //The score added to final score based on how fast player completed level
        private int _timeScore = 0;

        //All Letter keys a person can use to enter thier name
        private Keys[] letterArray = new Keys[26]
            {
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
            };

        //Constructor
        public ClearLevelState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, double time, int finalScore) : base(game, graphicsDevice, content)
        {
            //Defalt to false, player needs to enter a name
            _enterName = false;

            

            //Load texture and font
            Texture2D buttonTexture = _content.Load<Texture2D>("Controls/Button");
            SpriteFont buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            //Assign to private variable
            _totalGameTime = time;

            _device = graphicsDevice;

            _playerFinalScore = finalScore;

            _font = buttonFont;

            //Get button texture width for draw layout
            _buttonWidth = buttonTexture.Width;

            //Load the ScoreManager to show/add scores
            _scoreManager = ScoreManager.Load();

            //Call method to time score to final game score
            TimeBonus();

            //Submit player name and click method
            submitPlayerName = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((_device.Viewport.Width / 2) - (_buttonWidth / 2) - 195, 50),
                Text = "Submit Name"
            };

            submitPlayerName.Click += SubmitPlayerName_Click;

            //Two buttons, retry and exit
            Button restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) - 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2) + 100),
                Text = "Play Again?"
            };

            restartButton.Click += RestartButton_Click;

            Button quitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((graphicsDevice.Viewport.Width / 2) - (buttonTexture.Width / 2) + 100, (graphicsDevice.Viewport.Height / 2) - (buttonTexture.Height / 2) + 100),
                Text = "Quit"
            };

            quitButton.Click += QuitButton_Click;

            //add retry and quit to component list. Name is not added since it will be hidden when these two buttons are on the screen
            _components = new List<Component>()
            {
                restartButton,
                quitButton
            };

        }

        /// <summary>
        /// Clicking button will indicate that the user has finished entering thier name for the score board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SubmitPlayerName_Click(object sender, EventArgs e)
        {
            _enterName = true;

            //Default the name if they have entered nothing
            if (_playerName == "")
                _playerName = "PLAYER";

            //Add the score to the score file. Save so it will be tracked even when game project is not running
            Score toAdd = new Score()
            {
                ScoreID = _scoreManager.Scores.Count + 1,
                PlayerName = _playerName,
                Value = _playerFinalScore,
            };

            _scoreManager.Add(toAdd);

            ScoreManager.Save(_scoreManager);

            _scoreManager.UpdateHighScores();

            //Get the rank of the players score
            scoreRank = _scoreManager.Scores.IndexOf(toAdd) + 1;
        }

        /// <summary>
        /// Draw method specificly for ClearLevel state
        /// </summary>
        /// <param name="gameTime">from Game1</param>
        /// <param name="spriteBatch">from Game1</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Shows player score an prompt to enter name
            spriteBatch.DrawString(_font, $"Score: {_playerFinalScore}\nPlayer Name: {_playerName}", new Vector2((_device.Viewport.Width / 2) - (_buttonWidth / 2) - 195, 10), Color.Black);

            if (!_enterName)
                //Draw submit button while player has not clicked it and submitted name
                submitPlayerName.Draw(gameTime, spriteBatch);
            else
            {
                //Message to display highscore rank
                spriteBatch.DrawString(_font, $"Your score is {scoreRank} out of {_scoreManager.Scores.Count}", new Vector2((_device.Viewport.Width / 2) - (_buttonWidth / 2) + 125, 10), Color.Black);
                
                //Score breakdown of collectables and enemies killed
                spriteBatch.DrawString(_font, $"Score Breakdown:\n" +
                    $"{_timeScore}Time:                {Math.Round(_totalGameTime, 2)}s\n" +
                    $"  200 x Coin:             {GameState.CollectedCoins}/{GameState.TotalCoins}\n" +
                    $"1500 x Green Gem:     {GameState.CollectedGreenGem}/1\n" +
                    $"1000 x Red Gem:        {GameState.CollectedRedGem}/1\n" +
                    $"  750 x Blue Gem:        {GameState.CollectedBlueGem}/1\n" +
                    $"  500 x Yellow Gem:    {GameState.CollectedYellowGem}/1\n" +
                    $"  250 x Flies:                {GameState.KilledFlies}/{GameState.TotalFlies}\n" +
                    $"  450 x Snails:              {GameState.KilledSnails}/{GameState.TotalSnails}\n", new Vector2((_device.Viewport.Width / 2) - (_buttonWidth / 2) - 195, 90), Color.Black);
                
                //top ten highscore entries
                spriteBatch.DrawString(_font, $"High Scores:\n{string.Join("\n", _scoreManager.HighScores.Select(s => s.PlayerName + ": " + s.Value).ToArray())}", new Vector2((_device.Viewport.Width / 2) - (_buttonWidth / 2) + 125, 40), Color.Black);

                //Buttons
                foreach (Component component in _components)
                    component.Draw(gameTime, spriteBatch);
            }


            spriteBatch.End();
        }

        /// <summary>
        /// Update method specifically for ClearLevelState
        /// </summary>
        /// <param name="gameTime">from Game1</param>
        public override void Update(GameTime gameTime)
        {
            //Get the current and previous keys the user has pressed
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            //If name has not been entered, call method to create player name based on keyboard input, and update the playername
            if (!_enterName)
            {
                GetPlayerName();
                submitPlayerName.Update(gameTime);
            }
            else
            {
                //Name has been entered cannot update name anymore
                foreach (Component component in _components)
                    component.Update(gameTime);
            }

        }

        /// <summary>
        /// Creates player name to use in score tracking based on the keys pressed by the user. Adds most recent letter to player name Can use backspace to delete the most recent character
        /// Can add a space character. Uses an array of all letter keys. This insures that you cannot get multiple of the same letter if you press the key down for too long
        /// </summary>
        public void GetPlayerName()
        {
            if (_currentKey.IsKeyDown(Keys.Back) && _previousKey.IsKeyUp(Keys.Back))
            {
                if (_playerName.Length == 0)
                    return;
                _playerName = _playerName.Remove(_playerName.Length - 1, 1);
            }

            if (_playerName.Length > 12)
                return;

            if (_currentKey.IsKeyDown(Keys.Space) && _previousKey.IsKeyUp(Keys.Space) && _playerName.Length >= 1 && _playerName.Length <= 11)
                _playerName = _playerName + " ";

            foreach (Keys key in letterArray)
            {
                if (_currentKey.IsKeyDown(key) && _previousKey.IsKeyUp(key))
                {
                    _playerName = _playerName + key.ToString();
                }
            }

        }

        /// <summary>
        /// Quit button click method to exit game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }


        /// <summary>
        /// Restart button click method to play another game. Score and time do not transfer it is a new fresh game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartButton_Click(object sender, EventArgs e)
        {
            //Load the GameState State again
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        /// <summary>
        /// Method to calculate the time bonus for completing the level
        /// </summary>
        private void TimeBonus()
        {
            if (_totalGameTime <= 30)
            {
                _timeScore = 5000;
                _playerFinalScore += 5000;
            }
            else if (_totalGameTime <= 60)
            {
                _timeScore = 4000;
                _playerFinalScore += 4000;
            }
            else if (_totalGameTime <= 75)
            {
                _timeScore = 3000;
                _playerFinalScore += 3000;
            }
            else if (_totalGameTime <= 90)
            {
                _timeScore = 2000;
                _playerFinalScore += 2000;
            }
            else if (_totalGameTime <= 105)
            {
                _timeScore = 1000;
                _playerFinalScore += 1000;
            }
            else if (_totalGameTime <= 120)
            {
                _timeScore = 500;
                _playerFinalScore += 500;
            }
        }
    }
}
