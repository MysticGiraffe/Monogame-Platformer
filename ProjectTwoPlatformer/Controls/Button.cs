using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Controls
{
    internal class Button : Component
    {
        //Fields
        //Gets where on the screen the mouse is currently (for clicking buttons)
        private MouseState _currentMouse;

        //font
        private SpriteFont _font;

        //Check if the mouse is hovering over a button
        private bool _isHovering;

        //check where the mouse was previously 
        private MouseState _previousMouse;

        //Button texture
        private Texture2D _texture;

        //Properties
        //Button click
        public event EventHandler Click;

        //Check if the button has been clicked
        public bool Clicked { get; private set; }

        //Colour of the text in button
        public Color PenColour { get; set; }

        //Positon of button on screen
        public Vector2 Position { get; set; }

        //Mouse collision positon 
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        //The text to be put on button
        public string Text { get; set; }

        //Constructor
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }


        /// <summary>
        /// Draw method for buttons
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour);

            //Formate the text drawn on the button texture
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        /// <summary>
        /// Button update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            //Check if the mouse is touching the button
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
