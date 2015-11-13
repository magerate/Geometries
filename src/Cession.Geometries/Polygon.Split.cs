using System;
using System.Collections.Generic;
using System.Linq;

namespace Cession.Geometries
{
    public class SplitVertex
    {
        public SplitVertex Previous;
        public SplitVertex Next;
        public double X;
        public double Y;
        public bool IsIntersect;
        public bool IsCorner;

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public override string ToString ()
        {
            return $"({X},{Y})";
        }

        public SplitVertex GetNextIntersection()
        {
            var v = Next;
            while (!v.IsIntersect)
                v = v.Next;
            return v;
        }

        public bool? GetDirection()
        {
            return GetDirection (Previous);
        }

        public bool? GetDirection(SplitVertex end)
        {
            double signedArea = 0;
            SplitVertex v = this;
            do
            {
                signedArea += (v.X * v.Next.Y - v.Next.X * v.Y);
                v = v.Next;
            } while (v != end);
            signedArea += (end.X * Y - X * end.Y);

            if (signedArea == 0)
                return null;
            return signedArea > 0;
        }

        public static double CrossProduct(SplitVertex v1,SplitVertex v2,SplitVertex v3)
        {
//            var vector1 = v2.ToPoint () - v1.ToPoint ();
//            var vector2 = v2.ToPoint () - v3.ToPoint ();
//            return vector1.CrossProduct (vector2);
//            return v1.X * v2.Y - v1.Y * v2.X;
            return (v2.X - v1.X) * (v3.Y - v2.Y) - (v2.Y - v1.Y) * (v3.X - v2.X);
        }
    }

    internal static class SplitHeper
    {
        public static SplitVertex ToSplitVertex(this Point p)
        {
            return new SplitVertex() { X = p.X,Y = p.Y };
        }

        public static SplitVertex ToLinkList(this Point[] polygon)
        {
            SplitVertex result = null;
            SplitVertex current = null;
            for (int i = 0; i < polygon.Length; i++)
            {
                Point p = polygon[i];
                var v = p.ToSplitVertex();
                if (current != null)
                {
                    current.Next = v;
                    v.Previous = current;
                }
                current = v;

                if (i == 0)
                    result = v;

                if (i == polygon.Length - 1)
                {
                    current.Next = result;
                    result.Previous = current;
                }
            }
            return result;
        }
    }

    public partial class Polygon
    {
        public static Point[][] Split(Point[] polygon, Point p1, Point p2)
        {
            var linkedList = polygon.ToLinkList();
            var result = Split(linkedList, p1, p2);
            var ps = result.Select(l => l.Select(v=>v.ToPoint()).ToArray())
                .ToArray();
            return ps;
        }

        private static void ConnectIntersections(SplitVertex start,
            SplitVertex end,
            List<SplitVertex> intersections,
            bool? direction,
            List<SplitVertex> polygon)
        {
            int index1 = intersections.IndexOf(start);
            int index2 = intersections.IndexOf(end);
            if (Math.Abs(index1 - index2) == 1)
                return;

//            int step = index2 > index1 ? 1 : -1;
//            index2 -= step;
//            index1 += step;
//
//            for (; index2 != index1; index2 -= step)
//            {
//                var cornerDirection = intersections[index2].GetDirection (intersections[index2].GetNextIntersection ());
//                if (cornerDirection != direction && cornerDirection != null)
//                {
//                    do
//                    {
//                        polygon.Add (end);
//                        end = end.Next;
//                    } while (!end.IsIntersect);
//                    polygon.Add (end);
//                }
//            }
            SplitVertex stop;
            if (index2 > index1)
            {
                end = intersections [index2 - 1];
                stop = intersections [index1 + 1];
            }
            else
            {
                end = intersections [index2 + 1];
                stop = intersections [index1 - 1];
            }

            while(end != stop)
            {
                //connect vertex when encounter concave intersection
                if (end.IsIntersect)
                {
                    var cornerDirection = end.GetDirection (end.GetNextIntersection ());
                    if (cornerDirection != direction && cornerDirection != null)
                    {
                        double cp1 = SplitVertex.CrossProduct (intersections [0], intersections [intersections.Count - 1], start.Next);
                        double cp2 = SplitVertex.CrossProduct (intersections [0], intersections [intersections.Count - 1], end.Next);
                        if (cp1 * cp2 > 0)
                        {
                            do
                            {
                                polygon.Add (end);
                                end = end.Next;
                            } while (!end.IsIntersect);
                            polygon.Add (end);
                        }
                        else
                            end = end.Next;
                    }
                    else
                        end = end.Next;
                }
                else
                    end = end.Next;
            }
        }

        public static List<List<SplitVertex>> Split(SplitVertex polygon, Point p1, Point p2)
        {
            var ins = new List<SplitVertex>();

            //phase 1 get all intersection
            SplitVertex current = polygon;
            do
            {
                var next = current.Next;
                var cross = Line.IntersectWithSegment(p1, p2, current.ToPoint(), next.ToPoint());
                if (cross.HasValue)
                {
                    var iv = cross.Value.ToSplitVertex();
                    iv.IsIntersect = true;

                    iv.Next = next;
                    iv.Previous = current;

                    next.Previous = iv;
                    current.Next = iv;

                    if (ins.Count == 0)
                        ins.Add(iv);
                    else
                    {
                        int j = 0;
                        for (; j<ins.Count && (cross.Value.X > ins[j].X || 
                            (cross.Value.X == ins[j].X && cross.Value.Y >ins[j].Y)); 
                            j++);

                        if(j == ins.Count)
                            ins.Add(iv);
                        else
                            ins.Insert(j, iv);
                    }
                }
                current = next;
            } while (current != polygon);

            var result = new List<List<SplitVertex>>();
            if (ins.Count == 0)
                return result;

            //phase 2
            current = ins[0];
            bool? direction = polygon.GetDirection();
            do
            {
                //create new polygon when encouter convex intersection
                if(current.IsIntersect && current.GetDirection(current.GetNextIntersection()) == direction)
                {
                    var fi = current;
                    var sp = new List<SplitVertex>();
                    result.Add(sp);
                    do
                    {
                        sp.Add(current);
                        current = current.Next;
                    } while (!current.IsIntersect);
                    sp.Add(current);
                    ConnectIntersections(fi,current,ins,direction,sp);
                }
                else
                    current = current.Next;
            } while (current != ins[0]);

           
            return result;
        }
    }
}
