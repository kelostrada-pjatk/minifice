#region Using
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
        public static float shotFrequency = 0.7f;
        public static float BoundariesSize = 7;
        protected readonly Camera2d camera;
        protected int health;
        public bool isDying = false;

        #endregion

        #region Właściwości

        public bool IsAlive
        {
            get { return health > 0; }
        }

        #endregion

        #region Inicjalizacja

        public Unit()
        {

        }

        public Unit(Camera2d camera)
        {
            this.camera = camera;
        }

        #endregion

        #region Metody Publiczne

        public abstract Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies);
        
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Die(List<Fighter> fighters)
        {
            if (health > 0)
            {
                health--;
                timeLastShot += TimeSpan.FromSeconds(0.5);
            }
            if (!isDying && health == 0)
            {
                animation = animationDeath;
                isDying = true;
            }
        }

        public bool Update(GameTime gameTime)
        {
            currentTime = gameTime;

            if (isDying)
                animationDeath.Update(Direction.Left, gameTime);

            return (isDying && animation.IsOver());
        }

        public void Load(ContentManager content)
        {
            animation.Load(content);
            animationDeath.Load(content);
        }

        #endregion

    }
}
