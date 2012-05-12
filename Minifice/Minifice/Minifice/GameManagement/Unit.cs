using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minifice.ScreenManagement;
using Microsoft.Xna.Framework.Content;

namespace Minifice.GameManagement
{
    public abstract class Unit
    {
        public Boundaries boundaries;
        public Animation animation;
        public Animation animationDeath;
        public MoveStrategy moveStrategy;
        public float speed;
        public GameTime timeLastShot;
        public Vector2 position;

        public Unit()
        {

        }

        public abstract void Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, InputState input, GameTime gameTime);

        public abstract void Shoot(InputState input, Weapon weapon, GameTime gameTime, List<Weapon> weapons);

        public void Load(ContentManager content)
        {
            animation.Load(content);
            animationDeath.Load(content);
        }

    }
}
