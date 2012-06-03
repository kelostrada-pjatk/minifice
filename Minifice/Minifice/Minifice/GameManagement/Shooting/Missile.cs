using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.Enums;
using Microsoft.Xna.Framework.Content;

namespace Minifice.GameManagement.Shooting
{
    public abstract class Missile
    {
        protected int power;
        protected float range;
        protected float speed;
        protected Animation animationExplosion;
        protected Animation animation;
        protected Vector2 position;
        protected Vector2 start;
        protected Vector2 target;
        protected Boundaries boundaries;
        protected Faction faction;
        protected TimeSpan timeStartMove;

        public abstract void Load(ContentManager content);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime, GameMap gameMap, List<Fighter> fighter, List<Enemy> enemy);

        public static Missile FromWeapon(Weapon weapon, Vector2 start, Vector2 target, TimeSpan timeStart, Faction faction, GameTime gameTime)
        {
            switch (weapon)
            {
                case Weapon.Gun:
                    return new Gun(start, target, timeStart, faction, gameTime);
                case Weapon.Grenade:
                    break;
                case Weapon.Mine:
                    break;
            }
            return new Gun(start, target, timeStart, faction);
        }


    }
}
