#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minifice.ScreenManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        public TimeSpan timeLastShot;
        public Vector2 position;
        protected GameTime currentTime;
        #endregion

        #region Inicjalizacja

        public Unit()
        {

        }

        #endregion

        #region Metody Publiczne

        public abstract void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies);

        public abstract void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons);

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
