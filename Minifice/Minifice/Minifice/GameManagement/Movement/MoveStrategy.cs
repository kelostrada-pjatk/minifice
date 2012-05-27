#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Minifice.GameManagement.Movement
{
    public abstract class MoveStrategy
    {
        public GameMap gameMap;
        public List<Fighter> fighters;
        public List<Enemy> enemies;
        public Unit unit;

        public MoveStrategy()
        {

        }

        public MoveStrategy(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Unit unit)
        {
            this.gameMap = gameMap;
            this.fighters = fighters;
            this.enemies = enemies;
            this.unit = unit;
        }

        public abstract Vector2? Move(GameTime gameTime);
    }
}
