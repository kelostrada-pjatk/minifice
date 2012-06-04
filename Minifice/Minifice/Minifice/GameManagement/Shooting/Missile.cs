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

        public abstract bool Update(GameTime gameTime, GameMap gameMap, List<Fighter> fighter, List<Enemy> enemy);

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
            return new Gun(start, target, timeStart, faction, gameTime);
        }

        protected Vector2 GetMapPosition(GameMap gameMap)
        {
            int j = (int)Math.Floor((2 * (int)position.Y) / GameMap.TileShift.Y);
            int i = (int)Math.Floor((int)position.X / GameMap.TileShift.X - ((j % 2 == 1) ? 1 / 2 : 0));
            if (i < 0) i = 0;
            if (i > gameMap.width) i = gameMap.width - 1;
            if (j < 0) j = 0;
            if (j > gameMap.height - 1) j = gameMap.height - 1;
            return new Vector2(i, j);
        }

        protected Object Collision(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            int i = (int)GetMapPosition(gameMap).X;
            int j = (int)GetMapPosition(gameMap).Y;

            for (int k = -2; k < 3; k++)
            {
                for (int l = -2; l < 3; l++)
                {
                    if (i + k >= 0 && j + l >= 0 && i + k < gameMap.width && j + l < gameMap.height)
                    {
                        foreach (var mo in gameMap.mapTiles[i + k][j + l].mapObjects)
                        {
                            if ((j + l) % 2 == 0)
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X, (j + l) * GameMap.TileShift.Y / 2)).Intersects(boundaries + position)) return mo;
                            }
                            else
                            {
                                if ((mo.boundaries + new Vector2((i + k) * GameMap.TileShift.X + GameMap.TileShift.X / 2, (j + l) * GameMap.TileShift.Y / 2)).Intersects(boundaries + position)) return mo;
                            }
                        }
                    }
                }
            }

            Boundaries b;
            List<Vector2> points = new List<Vector2>();

            points.Add(new Vector2(-5, -45));
            points.Add(new Vector2(5, -45));
            points.Add(new Vector2(5, 0));
            points.Add(new Vector2(-5, 0));

            b = Boundaries.CreateFromPoints(points);

            if (faction == Faction.Enemies)
                foreach (Fighter f in fighters)
                {
                    //if ((boundaries + position).Intersects(f.boundaries + f.position + new Vector2(0, -15)))
                    if ((boundaries + position).Intersects(b + f.position))
                        return f;
                }

            if (faction == Faction.Fighters)
                foreach (Enemy e in enemies)
                {
                    //if ((boundaries + position).Intersects(e.boundaries + e.position + new Vector2(0,-15)))
                    if ((boundaries + position).Intersects(b + e.position))
                        return e;
                }

            return null;
        }


    }
}
