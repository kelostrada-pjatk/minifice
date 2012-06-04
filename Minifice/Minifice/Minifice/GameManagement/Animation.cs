using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Minifice.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minifice.GameManagement
{
    public class Animation
    {
        public int frames;
        public int fps;
        public int progress;
        public Direction direction;
        public List<AnimationFrame> framesUp;
        public List<AnimationFrame> framesDown;
        public List<AnimationFrame> framesLeft;
        public List<AnimationFrame> framesRight;
        TimeSpan lastTimeAnimate = new TimeSpan(0);

        public Animation()
        {
            framesUp = new List<AnimationFrame>();
            framesDown = new List<AnimationFrame>();
            framesLeft = new List<AnimationFrame>();
            framesRight = new List<AnimationFrame>();
            progress = 0;
        }

        public Animation(int frames, int fps, Direction direction) : this()
        {
            this.frames = frames;
            this.fps = fps;
            this.direction = direction;
            progress = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            switch (direction)
            {
                case Direction.Up:
                    framesUp[progress].Draw(spriteBatch, location, layerDepth);
                    break;
                case Direction.Down:
                    framesDown[progress].Draw(spriteBatch, location, layerDepth);
                    break;
                case Direction.Left:
                    framesLeft[progress].Draw(spriteBatch, location, layerDepth);
                    break;
                case Direction.Right:
                    framesRight[progress].Draw(spriteBatch, location, layerDepth);
                    break;
            }
        }

        public void Update(Direction direction, GameTime gameTime)
        {
            this.direction = direction;
            float ms = 10000000f / (float)fps;
            if (gameTime.TotalGameTime.Ticks - lastTimeAnimate.Ticks > ms)
            {
                if (++progress >= frames) progress = 0;
                lastTimeAnimate = gameTime.TotalGameTime;
            }
        }

        internal void Load(ContentManager content)
        {
            foreach (var f in framesUp)
                f.Load(content);
            foreach (var f in framesDown)
                f.Load(content);
            foreach (var f in framesLeft)
                f.Load(content);
            foreach (var f in framesRight)
                f.Load(content);
        }

        public bool IsOver()
        {
            return progress == frames - 1;
        }
    }
}