using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.Models
{
    public class Score
    {
        //Fields
        //If player has same name and score, need to something to make them different
        public int ScoreID { get; set; }

        //Player name
        public string PlayerName { get; set; }

        //Player game score
        public int Value { get; set; }
        
    }
}
