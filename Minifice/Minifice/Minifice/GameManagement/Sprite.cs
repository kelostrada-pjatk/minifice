#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Minifice.GameManagement
{
    public abstract class Sprite
    {
        #region Pola

        [XmlIgnore]
        protected Texture2D texture;
        public Rectangle source;
        public string textureName;

        #endregion

        #region Inicjalizacja

        public Sprite()
        {

        }

        public Sprite(string textureName, Rectangle source)
        {
            this.textureName = textureName;
            this.source = source;
        }

        #endregion

        #region Metody Publiczne

        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            if (texture != null)
                spriteBatch.Draw(texture, new Rectangle((int)location.X, (int)location.Y, (int)GameMap.TileShift.X+1, (int)GameMap.TileShift.Y+1), source, Color.White, 0, new Vector2(), SpriteEffects.None, layerDepth);
        }

        #endregion
    }
}
