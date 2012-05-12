#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Minifice.GameManagement
{
    public class GameMap
    {
        #region Pola

        public int width;
        public int height;
        public static Vector2 TileShift = new Vector2(48, 23);
        public List<List<MapTile>> mapTiles;

        #endregion

        #region Inicjalizacja

        public GameMap()
        {
            mapTiles = new List<List<MapTile>>();
        }

        public GameMap(int w, int h)
        {
            width = w;
            height = h;
            mapTiles = new List<List<MapTile>>(w);
            for (int i = 0; i < w; i++)
            {
                mapTiles.Add(new List<MapTile>(h));
                for (int j = 0; j < h; j++)
                {
                    mapTiles[i].Add(new MapTile());
                }
            }
        }


        #endregion

        #region Metody Publiczne

        internal void Load(ContentManager content)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    mapTiles[i][j].Load(content);
                }
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float x = i * TileShift.X + ((j % 2 == 1) ? TileShift.X / 2 : 0);
                    float y = j * ((float)Math.Ceiling(TileShift.Y/2));
                    mapTiles[i][j].Draw(spriteBatch, new Vector2(x,y));
                }
            }
        }

        #endregion
    }
}
