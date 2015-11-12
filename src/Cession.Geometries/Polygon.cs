using System;
using System.Linq;
using System.Collections.Generic;

namespace Cession.Geometries
{
    public class Polygon
    {
        public static Point[][] Split(Point[] polygon, Point p1, Point p2)
        {
            var linkedList = new LinkedList<Point>(polygon);
            var result = Split(linkedList, p1, p2);
            return result.Select(l => l.ToArray()).ToArray();
        }

        public static List<LinkedList<Point>> Split(LinkedList<Point> polygon,Point p1,Point p2)
        {
            List<LinkedListNode<Point>> intersectNodes = new List<LinkedListNode<Point>>();

            for (var i = polygon.First; i != null; i= i.Next)
            {
                var ni = i.Next == null ? polygon.First : i.Next;
                var cross = Line.IntersectWithSegment(p1,p2,i.Value,ni.Value);
                if (cross.HasValue)
                {
                    var node = polygon.AddAfter(i, cross.Value);
                    intersectNodes.Add(node);
                }
            }

            List<LinkedList<Point>> result = new List<LinkedList<Point>>();
            if (intersectNodes.Count == 0)
                return result;

            LinkedList<Point> sp;
            for (int i = 0; i < intersectNodes.Count; i++)
            {
                var node = intersectNodes[i];
                if(i % 2 == 0)
                {
                    sp = new LinkedList<Point>();
                    result.Add(sp);
                    sp.AddLast(node.Value);
                    while(node != intersectNodes[i+1])
                    {
                        node = node.Next;
                        sp.AddLast(node.Value);
                    }
                    sp.AddLast(node.Value);
                }
                else
                {
                    if(i == intersectNodes.Count - 1)
                    {
                        sp = new LinkedList<Point>();
                        while (node != intersectNodes[i - 1])
                        {
                            result.Add(sp);
                        }
                        result.Add(sp);
                    }
                }
            }
            return result;
        }

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

        public static bool IsClockwise (IReadOnlyList<Point> polygon)
        {
            double signedArea = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                Point p1 = polygon [i];
                Point p2 = polygon [(i + 1) % polygon.Count];
                signedArea += (p1.X * p2.Y - p2.X * p1.Y);
            }
            return signedArea > 0;
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

