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
    class Gun : Missile
    {
        Texture2D bullet;

        public Gun(Vector2 start, Vector2 target, TimeSpan timeStart, Faction faction, GameTime gameTime)
        {
            power = 1;
            range = 250f;
            speed = 2.5f;
            position = new Vector2(start.X,start.Y);
            this.start = new Vector2(start.X, start.Y);

            float s = speed * (gameTime.ElapsedGameTime.Ticks) * 0.00001f;


            this.target = new Vector2(target.X, target.Y);
            List<Vector2> points = new List<Vector2>();
            points.Add(Vector2.Zero);
            Vector2 v = new Vector2(target.X - start.X, target.Y - start.Y);
            if (v != Vector2.Zero)
            {
                v.Normalize();
                points.Add(v);
            }
            boundaries = Boundaries.CreateFromPoints(points);
            this.faction = faction;
            timeStartMove = timeStart;
        }

        public override void Load(ContentManager content)
        {
            bullet = content.Load<Texture2D>(@"Game\bullet");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bullet, position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1);
        }

        public override void Update(GameTime gameTime, GameMap gameMap, List<Fighter> fighter, List<Enemy> enemy)
        {

        }
    }
}
