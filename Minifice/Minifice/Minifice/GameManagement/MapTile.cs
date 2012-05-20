#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Minifice.GameManagement
{
    public class MapTile
    {
        #region Pola

        public BackgroundSprite backgroundSprite;
        public List<MapObject> mapObjects = new List<MapObject>();

        #endregion

        #region Inicjalizacja

        public MapTile()
        {

        }

        public MapTile(BackgroundSprite backgroundSprite, List<MapObject> mapObjects)
        {
            this.backgroundSprite = backgroundSprite;
            this.mapObjects = mapObjects;
        }

        #endregion

        #region Metody Publiczne

        internal void Load(ContentManager content)
        {
            backgroundSprite.Load(content);
            foreach (MapObject mo in mapObjects)
                mo.Load(content);
        }

        #endregion

        internal void Draw(SpriteBatch spriteBatch, Vector2 point)
        {
            float layerDepth = point.Y /(GameMap.TileShift.Y / 2) * 0.001f + 0.001f;
            backgroundSprite.Draw(spriteBatch, point, 0);
            foreach (MapObject mo in mapObjects)
                mo.Draw(spriteBatch, point, layerDepth);
        }
    }
}
