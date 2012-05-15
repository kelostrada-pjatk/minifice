using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minifice.GameManagement
{
    public class PanelSprite : Sprite
    {
        public PanelSprite()
        {

        }

        public PanelSprite(string textureName, Rectangle source)
            : base(textureName, source)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            spriteBatch.Draw(texture, new Rectangle((int)location.X, (int)location.Y, GameInterface.Width,
            spriteBatch.GraphicsDevice.Viewport.Height), source, Color.White, 0, new Vector2(), SpriteEffects.None, layerDepth);
        }
    }
}
