using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement
{
    public class AnimationFrame : Sprite
    {
        public AnimationFrame()
        {

        }

        public AnimationFrame(string textureName, Rectangle source)
            : base(textureName, source)
        {

        }

        public AnimationFrame(string textureName, Rectangle source, Rectangle destination)
            : base(textureName, source, destination)
        {

        }
    }
}
