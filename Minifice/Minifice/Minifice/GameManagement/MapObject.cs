using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;

namespace Minifice.GameManagement
{
    public class MapObject : Sprite
    {
        [XmlAttribute]
        public bool viewBlocking;
        [XmlAttribute]
        public bool throwableOver;
        [XmlAttribute]
        public bool collectible;
        public Bonus type;
        public Boundaries boundaries;

        public MapObject()
            : base()
        {

        }

        public void Load(ContentManager content, int i, int j)
        {
            base.Load(content);
            boundaries += new Vector2(i * GameMap.TileShift.X + ((j % 2 != 0) ? GameMap.TileShift.X / 2 : 0), j * GameMap.TileShift.Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {

            //layerDepth -= (boundaries.Max.Y + boundaries.Min.Y)/2 * 0.000001f;

            base.Draw(spriteBatch, location, layerDepth);
        }
    }
}