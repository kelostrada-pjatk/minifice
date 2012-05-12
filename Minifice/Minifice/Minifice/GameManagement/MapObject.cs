using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement
{
    public class MapObject : Sprite
    {
        public bool viewBlocking;
        public bool throwableOver;
        public bool collectible;
        public Bonus type;
        public Boundaries boundaries;

        public override void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            layerDepth -= boundaries.Min.Y * 0.000001f;
            location += boundaries.Min;
            
            base.Draw(spriteBatch, location, layerDepth);
        }
    }
}
