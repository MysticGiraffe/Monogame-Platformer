using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.StageStuff
{
    public class Collectable : Tiles
    {
        //Fields
        //Score value of the collectable
        protected int _value;

        //Properties
        //Public value method
        public int Value
        {
            get { return _value; }
        }

        //bool to check to see if collectable has been collected by the player
        public bool isCollected {  get; set; }

        //Check to see if this coin has been counted towards player score. Insures no double counting
        public bool isCounted { get; set; }

        //Constructor, defalut is not collected
        public Collectable() 
        { 
            isCollected = false;
        }
    }
}
