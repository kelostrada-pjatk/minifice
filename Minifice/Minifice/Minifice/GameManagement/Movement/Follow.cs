using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ASD.Graph;

namespace Minifice.GameManagement.Movement
{
    public class Follow : MoveStrategy
    {
        public Unit destination;

        public Follow()
            : base()
        {

        }

        public Follow(GameMap gameMap, List<Fighter> fighters, List<Enemy> enemies, Unit unit, Unit destination)
            : base(gameMap, fighters, enemies, unit)
        {
            this.destination = destination;
        }

        public override void Move(GameTime gameTime)
        {
            base.Move(gameTime);

            // Wpierw przerobić planszę na graf.
            IGraph g = gameMap.CreateGraph(fighters, enemies, unit.position, destination.position);

        }
    }
}
