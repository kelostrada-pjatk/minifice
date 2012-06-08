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
        [XmlElement]
        public Rectangle source;
        [XmlAttribute]
        public string textureName;
        [XmlElement]
        public Vector2 origin = new Vector2();
        [XmlElement]
        public Rectangle destination;

        #endregion

        #region Właściwości

        public Source Source
        {
            get
            {
                return new Source(source.X, source.Y, source.Width, source.Height);
            }
            set
            {
                source = new Rectangle(value.X, value.Y, value.Width, value.Height);
            }
        }

        #endregion

        #region Inicjalizacja

        public Sprite()
        {

        }

        public Sprite(string textureName, Rectangle source)
        {
            this.textureName = textureName;
            this.source = source;
            this.destination = source;
        }

        public Sprite(string textureName, Rectangle source, Rectangle destination)
            : this(textureName, source)
        {
            this.destination = destination;
        }

        #endregion

        #region Metody Publiczne

        public virtual void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureName);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            if (texture != null)
                if (destination.Width != 0 || destination.Height != 0)
                    spriteBatch.Draw(texture, new Rectangle((int)location.X, (int)location.Y, destination.Width + 1, destination.Height + 1), source, Color.White, 0, origin, SpriteEffects.None, layerDepth);
                else
                    spriteBatch.Draw(texture, new Rectangle((int)location.X, (int)location.Y, source.Width + 1, source.Height + 1), source, Color.White, 0, origin, SpriteEffects.None, layerDepth);
        }

        #endregion
    }

#region Klasa Pomocnicza
    public class Source
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Source()
        {

        }

        public Source(int _X, int _Y, int _Width, int _Height)
        {
            X = _X;
            Y = _Y;
            Width = _Width;
            Height = _Height;
        }
    }
    #endregion
}
