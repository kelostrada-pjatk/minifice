﻿#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minifice.ScreenManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minifice.GameManagement.Movement;
using Minifice.Enums;
using Minifice.GameManagement.Shooting;
#endregion

namespace Minifice.GameManagement
{
    public abstract class Unit
    {
        #region Pola
        public Boundaries boundaries;
        public Animation animation;
        public Animation animationDeath;
        public MoveStrategy moveStrategy;
        public float speed;
        protected TimeSpan timeLastShot = new TimeSpan(0);
        public Vector2 position;
        protected GameTime currentTime;
        protected TimeSpan timeLastMoved = new TimeSpan(0);
        public static float shotFrequency = 1.5f;
        #endregion

        #region Właściwości

        public Vector2 GetMapPosition(GameMap gameMap)
        {
            int j = (int)Math.Floor((2 * (int)position.Y) / GameMap.TileShift.Y);
            int i = (int)Math.Floor((int)position.X / GameMap.TileShift.X - ((j % 2 == 1) ? 1 / 2 : 0));
            if (i < 0) i = 0;
            if (i > gameMap.width) i = gameMap.width - 1;
            if (j < 0) j = 0;
            if (j > gameMap.height - 1) j = gameMap.height - 1;
            return new Vector2(i, j);
        }

        #endregion

        #region Inicjalizacja

        public Unit()
        {

        }

        #endregion

        #region Metody Publiczne

        public abstract Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies);
        
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public void Update(GameTime gameTime)
        {
            currentTime = gameTime;
        }

        public void Load(ContentManager content)
        {
            animation.Load(content);
            animationDeath.Load(content);
        }

        #endregion

    }
}
