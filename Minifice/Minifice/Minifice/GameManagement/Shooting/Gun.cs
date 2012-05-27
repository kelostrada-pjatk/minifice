using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.Enums;

namespace Minifice.GameManagement.Shooting
{
    class Gun : Missile
    {
        public Gun(Vector2 start, Vector2 target, TimeSpan timeStart, Faction faction)
        {
            power = 1;
            range = 250f;
            speed = 2.5f;
            position = new Vector2(start.X,start.Y);
            this.start = new Vector2(start.X, start.Y);
            this.target = new Vector2(target.X, target.Y);
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(0, 0));
            boundaries = Boundaries.CreateFromPoints(points);
            this.faction = faction;
            timeStartMove = timeStart;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime, GameMap gameMap, List<Fighter> fighter, List<Enemy> enemy)
        {

        }
    }
}
