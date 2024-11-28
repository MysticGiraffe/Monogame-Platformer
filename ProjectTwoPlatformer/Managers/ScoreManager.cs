using ProjectTwoPlatformer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectTwoPlatformer.Managers
{
    public class ScoreManager
    {

        //Fields
        //Saved in bin folder
        private static string _fileName= "scores.xml";

        //Properties
        //Get a list of top 10 scores
        public List<Score> HighScores { get; private set; }

        //Get list of all scores
        public List<Score> Scores { get; private set; }

        //Constructors 
        public ScoreManager() : this (new List<Score>())
        {

        }

        //to add list of scores 
        public ScoreManager(List<Score> scores)
        {
            Scores = scores;

            UpdateHighScores();
        }


        //Methods
        /// <summary>
        /// Add a score to the list of scores, then update the order of score list (by value highest to lowest)
        /// </summary>
        /// <param name="score"></param>
        public void Add(Score score)
        {
            Scores.Add(score);

            //Order the list of scores from highest to lowest
            Scores = Scores.OrderByDescending(s => s.Value).ToList();
        }

        /// <summary>
        /// This method loads the player's score to an XML file
        /// If the files does not exist, it creates a new empty file
        /// Otherwise, the file exists and it reads all the score entries in the file
        /// and loads all the score data to a scores list to be used by the app
        /// </summary>
        /// <returns></returns>
        public static ScoreManager Load()
        {
            //If there is not a file to load, create a new instance of ScoreManager
            if (!File.Exists(_fileName))
                return new ScoreManager();

            //If file exists
            using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                var scores = (List<Score>)serializer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        /// <summary>
        /// Method go get the top 10 highest scores 
        /// </summary>
        public void UpdateHighScores()
        {
            HighScores = Scores.OrderByDescending(s => s.Value).Take(10).ToList();
        }

        /// <summary>
        /// Method to save a new score to the XML file
        /// Called whenever a player enters thier name at the end of the game
        /// It overrides the file, and addes all scores in the scores list
        /// </summary>
        /// <param name="scoreManager"></param>
        public static void Save(ScoreManager scoreManager)
        {
            //Overrides the file if it already exists
            using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<Score>));

                serializer.Serialize(writer, scoreManager.Scores);
            }
        }
    }
}
