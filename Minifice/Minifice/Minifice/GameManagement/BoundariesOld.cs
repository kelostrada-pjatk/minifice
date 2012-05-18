/*
#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace Minifice.GameManagement
{
    public class Boundaries
    {
        BoundingBox boundingBox;

        public Vector2 Min
        {
            get
            {
                return new Vector2(boundingBox.Min.X, boundingBox.Min.Y);
            }
            set
            {
                boundingBox.Min = new Vector3(value.X, value.Y, 0);
            }
        }

        public Vector2 Max
        {
            get
            {
                return new Vector2(boundingBox.Max.X, boundingBox.Max.Y);
            }
            set
            {
                boundingBox.Max = new Vector3(value.X, value.Y, 0);
            }
        }

        public Boundaries()
        {
            boundingBox = new BoundingBox();
        }

        public Boundaries(BoundingBox box) : base()
        {
            boundingBox = box;
        }

        public ContainmentType Contains(Boundaries boundaries)
        {
            return this.boundingBox.Contains(boundaries.boundingBox);
        }
        
        public static Boundaries CreateFromPoints(IEnumerable<Vector2> points)
        {
            List<Vector3> newPoints = new List<Vector3>();
            foreach (var p in points)
            {
                newPoints.Add(new Vector3(p.X, p.Y, 0));
            }
            return new Boundaries(BoundingBox.CreateFromPoints(newPoints));
        }

        public bool Equals(Boundaries boundaries)
        {
            return boundingBox.Equals(boundaries.boundingBox);
        }

        public Vector2[] GetCorners()
        {
            List<Vector2> corners = new List<Vector2>();
            foreach (var p in boundingBox.GetCorners())
            {
                corners.Add(new Vector2(p.X, p.Y));
            }
            return corners.ToArray();
        }

        public override int GetHashCode()
        {
            return boundingBox.GetHashCode();
        }

        public bool Intersects(Boundaries boundaries)
        {
            return boundingBox.Intersects(boundaries.boundingBox);
        }

        public override string ToString()
        {
            return boundingBox.ToString();
        }


    }


}
*/