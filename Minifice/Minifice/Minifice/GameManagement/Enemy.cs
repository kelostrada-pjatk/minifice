﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;


namespace Minifice.GameManagement
{
    public class Enemy : Unit
    {
        public override void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input, GameTime gameTime)
        {

        }

        public override void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
