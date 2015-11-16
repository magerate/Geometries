using System;
using System.Linq;
using System.Collections.Generic;

namespace Cession.Geometries
{
    public partial class Polygon
    {
        public static Rect GetBounds(IReadOnlyList<Point> polygon)
        {
            if (null == polygon)
                throw new ArgumentNullException ();
            if (polygon.Count < 3)
                throw new ArgumentException ("polygon");

            double left = double.MaxValue;
            double right = double.MinValue;
            double top = double.MaxValue;
            double bottom = double.MinValue;

            for (int i = 0; i < polygon.Count; i++)
            {
                left = Math.Min (polygon [i].X, left);
                right = Math.Max (polygon [i].X, right);

                top = Math.Min (polygon [i].Y, top);
                bottom = Math.Max (polygon [i].Y, bottom);
            }
            return Rect.FromLTRB (left, top, right, bottom);
        }

        //true clockwise false counterclockwise null means not a valid polygon
        public static bool? IsClockwise (IReadOnlyList<Point> polygon)
        {
            if (null == polygon)
                return null;

            if (polygon.Count < 3)
                return null;

            int pc = 0;
            int nc = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                Point p1 = polygon [i];
                Point p2 = polygon [(i + 1) % polygon.Count];
                double crossProduct = p1.X * p2.Y - p2.X * p1.Y;
                if (crossProduct > 0)
                    pc++;
                else if (crossProduct < 0)
                    nc++;
            }

            if (pc == 0 && nc == 0)
                return null;
            return pc > 0;
        }

        public static bool IsConvex(IReadOnlyList<Point> polygon)
        {
            double pcp = 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                Point p1 = polygon[i];
                Point p2 = polygon[(i + 1) % polygon.Count];
                var cp = p1.X * p2.Y - p2.X * p1.Y;
                if(cp != 0)
                {
                    if (cp * pcp < 0)
                        return false;
                    else
                        pcp = cp;
                }
            }
            return true;
        }

        public static bool Contains (Point point, IReadOnlyList<Point> polygon)
        {
            return ContainsPoint (point, polygon) != 0;
        }

        public static int ContainsPoint (Point point, IReadOnlyList<Point> polygon)
        {
            //returns 0 if false, +1 if true, -1 if pt ON polygon boundary
            //http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.88.5498&rep=rep1&type=pdf
            int result = 0, cnt = polygon.Count;
            if (cnt < 3)
                return 0;
            var ip = polygon [0];
            for (int i = 1; i <= cnt; ++i) {
                var ipNext = (i == cnt ? polygon [0] : polygon [i]);
                if (ipNext.Y == point.Y) {
                    if ((ipNext.X == point.X) || (ip.Y == point.Y &&
                    ((ipNext.X > point.X) == (ip.X < point.X))))
                        return -1;
                }
                if ((ip.Y < point.Y) != (ipNext.Y < point.Y)) {
                    if (ip.X >= point.X) {
                        if (ipNext.X > point.X)
                            result = 1 - result;
                        else {
                            double d = (ip.X - point.X) * (ipNext.Y - point.Y) -
                            (ipNext.X - point.X) * (ip.Y - point.Y);
                            if (d == 0)
                                return -1;
                            else if ((d > 0) == (ipNext.Y > ip.Y))
                                result = 1 - result;
                        }
                    } else {
                        if (ipNext.X > point.X) {
                            double d = (ip.X - point.X) * (ipNext.Y - point.Y) -
                            (ipNext.X - point.X) * (ip.Y - point.Y);
                            if (d == 0)
                                return -1;
                            else if ((d > 0) == (ipNext.Y > ip.Y))
                                result = 1 - result;
                        }
                    }
                }
                ip = ipNext;
            }
            return result;
        }
    }
}

