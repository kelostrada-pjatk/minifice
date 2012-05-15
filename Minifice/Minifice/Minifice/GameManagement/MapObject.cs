﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

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


        public override void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {

            layerDepth -= boundaries.Min.Y * 0.000001f;
            //location += boundaries.Min;

            base.Draw(spriteBatch, location, layerDepth);
        }
    }
}