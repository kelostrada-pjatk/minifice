using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minifice.ScreenManagement;
using Minifice.Enums;
using Minifice.GameManagement.Shooting;


namespace Minifice.GameManagement
{
    public class Enemy : Unit
    {
        #region Inicjalizacja

        public Enemy(Vector2 position, Camera2d camera) : base(camera)
        {
            this.position = position;

            // Definicja animacji postaci
            this.animation = new Animation(2, 5, Direction.Left);
            AnimationFrame mini1 = new AnimationFrame(@"Game\mini1", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini1.origin = new Vector2(100, 180);
            AnimationFrame mini2 = new AnimationFrame(@"Game\mini2", new Rectangle(0, 0, 200, 200), new Rectangle(0, 0, 40, 40));
            mini2.origin = new Vector2(100, 180);
            this.animation.framesLeft.Add(mini1);
            this.animation.framesLeft.Add(mini2);
            this.animation.framesRight.Add(mini1);
            this.animation.framesRight.Add(mini2);
            this.animation.framesUp.Add(mini1);
            this.animation.framesUp.Add(mini2);
            this.animation.framesDown.Add(mini1);
            this.animation.framesDown.Add(mini2);

            this.animationDeath = new Animation();

            List<Vector2> points = new List<Vector2>();

            points.Add(new Vector2(-7, 0));
            points.Add(new Vector2(0, -7));
            points.Add(new Vector2(7, 0));
            points.Add(new Vector2(0, 7));

            this.boundaries = Boundaries.CreateFromPoints(points);
            
            this.health = 2;
            this.speed = 0.75f;
            this.timeLastShot = new TimeSpan();

        }

        #endregion

        public override Vector2? Move(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies)
        {
            return null;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, position.Y / (GameMap.TileShift.Y / 2) * 0.001f + 0.001f);
        }
    }
}
