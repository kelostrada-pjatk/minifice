using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minifice.GameManagement
{
    public class Boundaries
    {
        List<Vector2> points = new List<Vector2>();
        List<Function> functions = new List<Function>();
        
        Vector2 min;
        Vector2 max;

        public Vector2 Min
        {
            get { return min; }
            set { min = value; }
        }

        public Vector2 Max
        {
            get { return max; }
            set { max = value; }
        }

        private Boundaries()
        {
            min = new Vector2(float.MaxValue);
            max = new Vector2(float.MinValue);
        }

        internal static Boundaries CreateFromPoints(List<Vector2> points)
        {
            Boundaries b = new Boundaries();

            foreach (Vector2 v in points)
            {
                b.points.Add(new Vector2(v.X, v.Y));
                if (v.X < b.min.X)
                    b.min.X = v.X;
                if (v.Y < b.min.Y)
                    b.min.Y = v.Y;
                if (v.X > b.max.X)
                    b.max.X = v.X;
                if (v.Y > b.max.Y)
                    b.max.Y = v.Y;
            }

            for (int i = 0; i < points.Count; i++)
            {
                float A = float.NaN, B, p1, p2;
                int j = (i+1==points.Count)?0:i+1;
                if (points[j].X == points[i].X)
                {
                    // to znaczy że to nie jest funkcja
                    p1 = (points[i].Y < points[j].Y) ? points[i].Y : points[j].Y;
                    p2 = (points[i].Y >= points[j].Y) ? points[i].Y : points[j].Y;
                    B = points[i].X;
                }
                else
                {
                    // obliczanie wspolrzednych funkcji
                    A = (points[j].Y - points[i].Y) / (points[j].X - points[i].X);
                    B = (points[i].Y * points[j].X - points[j].Y * points[i].X) / (points[j].X - points[i].X);
                    p1 = (points[i].X < points[j].X) ? points[i].X : points[j].X;
                    p2 = (points[i].X >= points[j].X) ? points[i].X : points[j].X;
                }

                b.functions.Add(new Function(A,B,p1,p2));
            }


            return b;
        }

        internal bool Intersects(Boundaries b)
        {
            foreach (var f1 in b.functions)
                foreach (var f2 in functions)
                {
                    if (f1.a == float.NaN || f2.a == float.NaN)
                    {
                        // Ktoras z funkcji nie jest tak na prawde funkcja. Teraz przypadki
                        if (f1.a == float.NaN && f2.a == float.NaN)
                        {
                            if (f1.b == f2.b)
                                if (f1.p2 >= f2.p1 && f2.p2 >= f1.p1) return true;
                        }
                        if (f1.a == float.NaN && f2.a != float.NaN)
                        {
                            if (f1.b >= f2.p1 && f1.b <= f2.p2)
                                if (f2.func(f1.b) >= f1.p1 && f2.func(f1.b) <= f1.p2) return true;
                        }
                        if (f1.a != float.NaN && f2.a == float.NaN)
                        {
                            if (f2.b >= f1.p1 && f2.b <= f1.p2)
                                if (f1.func(f2.b) >= f2.p1 && f1.func(f2.b) <= f2.p2) return true;
                        }
                    }
                    else if (f1.a == f2.a)
                    {
                        if (f1.b == f2.b)
                            if (f1.p2 >= f2.p1 && f2.p2 >= f1.p1) return true;
                    }
                    else
                    {
                        float x = (f2.b - f1.b) / (f1.a - f2.a);
                        if (x >= f1.p1 && x <= f1.p2 && x >= f2.p1 && x <= f2.p2) return true;
                    }
                }

            return false;
        }
    }

    public class Function
    {
        public float a, b;
        public float p1, p2;

        public Function()
        {

        }

        public Function(float a, float b, float p1, float p2)
        {
            this.a = a;
            this.b = b;
            this.p1 = p1;
            this.p2 = p2;
        }

        public float func(float x)
        {
            return a * x + b;
        }
    }
}
