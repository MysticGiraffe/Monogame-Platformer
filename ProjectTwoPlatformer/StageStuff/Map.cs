using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectTwoPlatformer.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTwoPlatformer.StageStuff
{
    public class Map
    {
        //List of all tiles in map
        private List<Tiles> tiles = new List<Tiles>();

        public List<Tiles> Tiles { get { return tiles; } }

        //Get number of coins on the map
        public int Coins { get; set; }

        //Get a list of tiles that are of type CollisionTiles
        public List<CollisionTiles> CollisionTiles
        {
            get
            {
                List<CollisionTiles> collisionTiles = new List<CollisionTiles>();

                foreach (var tile in tiles)
                {
                    if (tile is CollisionTiles)
                    {
                        CollisionTiles toAdd = tile as CollisionTiles;
                        collisionTiles.Add(toAdd);
                    }

                }
                return collisionTiles;
            }
        }

        //Get a list of tiles that are specifically collectable (Coins and gems)
        public List<Collectable> CollectableTiles
        {
            get
            {
                List<Collectable> collectables = new List<Collectable>();

                foreach (var tile in tiles)
                {
                    if (tile.GetType() == typeof(Coin) || tile.GetType() == typeof(Gem))
                    {
                        Collectable toAdd = tile as Collectable;
                        collectables.Add(toAdd);
                    }

                }
                return collectables;
            }
        }

        //Get a list of all coins
        public List<Coin> CoinsList
        {
            get
            {
                List<Coin> collectables = new List<Coin>();

                foreach (var tile in tiles)
                {
                    if (tile.GetType() == typeof(Coin))
                    {
                        Coin toAdd = tile as Coin;
                        collectables.Add(toAdd);
                    }

                }
                return collectables;
            }
        }

        //Currently only one of each colour per level, track that specific gem with single object
        public Gem GreenGem { get; set; }
        public Gem BlueGem { get; set; }
        public Gem YellowGem { get; set; }
        public Gem RedGem { get; set; } 

        //Get the Flag tiles in the map
        public List<Flag> FlagTiles
        {
            get
            {
                List<Flag> flagTiles = new List<Flag>();

                foreach (var tile in tiles)
                {
                    if (tile.GetType() == typeof(Flag))
                    {
                        Flag toAdd = tile as Flag;
                        flagTiles.Add(toAdd);
                    }

                }
                return flagTiles;
            }
        }

        //Total number of tiles on the x axis
        //Length of tile * total number of tiles
        private int width;

        //Total number of tiles on y axis
        //Height of tile * number of tiles
        private int height;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        //Constructor
        public Map()
        {

        }


        /// <summary>
        /// Generate the tile map using an int array. Add each tile to the tiles list so they are all stored in one object
        /// </summary>
        /// <param name="map">An int array that has numbers 0 - 7 each representing a differnt type of tile</param>
        /// <param name="size">The size of each tile</param>
        /// <param name="textureTiles">The texture spritesheet for the map "Blocks"</param>
        /// <param name="textureItems">The texture spritesheet for the map "Collectables" coins, gems</param>
        public void Generate(int[,] map, int size, Texture2D textureTiles, Texture2D textureItems)
        {
            //Cycle through numbers in x and y axis
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    //Determine which tile
                    int number = map[y, x];

                    if (number == 1)
                    {
                        tiles.Add(new CollisionTiles(textureTiles, new Rectangle(x * size, y * size, size, size), new Rectangle(504, 576, 70, 70)));
                    }
                    if (number == 2)
                    {
                        tiles.Add(new CollisionTiles(textureTiles, new Rectangle(x * size, y * size, size, size), new Rectangle(576, 864, 70, 70)));
                    }
                    if (number == 3)
                    {
                        tiles.Add(new Coin(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(288, 360, 70, 70)));
                        Coins++;
                    }
                    if (number == 4)
                    {
                        GreenGem = new Gem(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(144, 290, 70, 70), "Green");
                        tiles.Add(GreenGem);
                    }
                    if (number == 5)
                    {
                        BlueGem = new Gem(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(144, 362, 70, 70), "Blue");
                        tiles.Add(BlueGem);
                    }
                    if (number == 6)
                    {
                        RedGem = new Gem(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(131, 218, 70, 70), "Red");
                        tiles.Add(RedGem);
                    }
                    if (number == 7)
                    {
                        YellowGem = new Gem(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(131, 72, 70, 70), "Yellow");
                        tiles.Add(YellowGem);
                    }
                    if (number == 8)
                    {
                        tiles.Add(new Flag(textureItems, new Rectangle(x * size, y * size, size, size), new Rectangle(216, 432, 70, 70)));
                    }

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }
        }

        /// <summary>
        /// Draw specific to tile map, collectables and map block tiles
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tiles tile in tiles)
            {
                if (tile.GetType() == typeof(Coin) || tile.GetType() == typeof(Gem))
                {
                    Collectable collectable = (Collectable)tile;

                    if (!collectable.isCollected)
                        collectable.Draw(spriteBatch);
                    else
                        continue;
                }
                else
                {
                    tile.Draw(spriteBatch);
                }
            }
        }

    }
}
