using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
using Minifice.Enums;
using Minifice.GameManagement.Shooting;


namespace Minifice.GameManagement
{
    public class Enemy : Unit
    {
        public override Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            return null;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
