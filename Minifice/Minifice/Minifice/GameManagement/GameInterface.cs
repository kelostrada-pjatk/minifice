﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Minifice.ScreenManagement;

namespace Minifice.GameManagement
{
    public class GameInterface
    {
        public static int Width = 200;

        private PanelSprite panel;

        public GameInterface()
        {
            panel = new PanelSprite(@"Game\interface", new Rectangle(0, 0, 3, 3));
        }

        public void Load(ContentManager content)
        {
            panel.Load(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            panel.Draw(spriteBatch, new Vector2(), 1f);
        }

        public void HandleInput(InputState input)
        {
            
        }
    }
}
