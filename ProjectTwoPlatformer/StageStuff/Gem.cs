using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.StageStuff
{
    public class Gem : Collectable
    {
        //Properties
        //Get colour of the gem, value changes based on colour
        public string Colour { get; set;  }

        //Constructor
        public Gem(Texture2D texture, Rectangle newRectangle, Rectangle spriteLocation, string colour) : base ()
        {
            _texture = texture;
            _sourceRectangle = spriteLocation;
            this.Rectangle = newRectangle;

            Colour = colour;

            if (colour == "Green")
                _value = 1500;

            if (colour == "Blue")
                _value = 750;

            if (colour == "Red")
                _value = 1000;

            if (colour == "Yellow")
                _value = 500;
        }
    }
}
