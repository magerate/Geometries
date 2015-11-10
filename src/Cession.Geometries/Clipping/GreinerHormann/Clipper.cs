using System;
using System.Linq;
using System.Collections.Generic;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public static class VertexHelper
    {
        public static Point ToPoint(this Vertex vertex)
        {
            return new Point(vertex.X, vertex.Y);
        }

        public static Vertex ToVertex(this Point point)
        {
            return new Vertex() { X = point.X, Y = point.Y };
        }

        public static List<Point> ToList(this Vertex vertex)
        {
            List<Point> ps = new List<Point>();
            for (var si = vertex; si != null; si = si.Next == vertex ? null : si.Next)
            {
                ps.Add(si.ToPoint());
            }
            return ps;
        }


        public static Vertex ToLinkList(this Point[] polygon)
        {
            Vertex result = null;
            Vertex current = null;
            for (int i = 0; i < polygon.Length; i++)
            {
                Point p = polygon[i];
                var v = p.ToVertex();
                if (current != null) {
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

    //
    public static class Clipper
    {
        public static List<List<Vertex>> Clip(Vertex subject, Vertex clip)
        {
            //phase 1
            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                for (var cj = clip; cj != null; cj = cj.Next == clip ? null : cj.Next)
                {
                    double a = 0, b = 0;

                    if (Intersects(si, si.Next, cj, cj.Next, ref a, ref b))
                    {
                        Vertex i1 = CreateVertex(si, si.Next, a);
                        Vertex i2 = CreateVertex(cj, cj.Next, b);
                        i1.Neibour = i2;
                        i2.Neibour = i1;

                        si.Next.Previous = i1;
                        si.Next = i1;

                        cj.Next.Previous = i2;
                        cj.Next = i2;

                        si = i1;
                        cj = i2;
                    }
                }
            }

            //phase 2
            //true exit false entry
            bool status = PolygonAlgorithm.EOContains(clip.ToList(), subject.ToPoint());

            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect)
                {
                    si.IsExit = status;
                    status = !status;
                }
            }

            status = PolygonAlgorithm.EOContains(subject.ToList(), clip.ToPoint());
            for (var cj = clip; cj != null; cj = cj.Next == clip ? null : cj.Next)
            {
                if (cj.IsIntersect)
                {
                    cj.IsExit = status;
                    status = !status;
                }
            }

            //phase 3
            Vertex current;
            Vertex start;
            List<List<Vertex>> polygonList = new List<List<Vertex>>();

            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect && !si.IsVisit)
                {
                    start = si;
                    current = start;
                    current.IsVisit = true;

                    var polygon = new List<Vertex>();
                    polygonList.Add(polygon);

                    polygon.Add(current);
                    do
                    {
                        if (current.IsExit)
                        {
                            do
                            {
                                current = current.Previous;
                                polygon.Add(current);
                            } while (!current.IsIntersect);
                        }
                        else
                        {
                            do
                            {
                                current = current.Next;
                                polygon.Add(current);
                            } while (!current.IsIntersect);
                        }
                        if (current.IsIntersect)
                        {
                            current.IsVisit = true;
                        }
                        current = current.Neibour;
                    } while (current != start);
                }
            }

            return polygonList;
        }


        private static Vertex CreateVertex(Vertex v1, Vertex v2, double alpha)
        {
            var v = v2.ToPoint() - v1.ToPoint();
            v = v / v.Length * alpha;

            var p = v1.ToPoint() + v;
            var vertex = new Vertex() { X = p.X, Y = p.Y, IsIntersect = true,Previous = v1,Next = v2, };
            return vertex;
        }

        public static bool Intersects(Vertex p1,Vertex p2,Vertex q1,Vertex q2,ref double alphaP,ref double alphaQ)
        {
            double wecP1 = (p1.X - q1.X) * (q2.Y - q1.Y) - (p1.Y - q1.Y) *(q2.X - q1.X);
            double wecP2 = (p2.X - q1.X) * (q2.Y - q1.Y) - (p2.Y - q1.Y) * (q2.X - q1.X);
            if(wecP1 * wecP2 <=0)
            {
                double wecQ1 = (q1.X - p1.X) * (p2.Y - p1.Y) - (q1.Y - p1.Y) * (p2.X - p1.X);
                double wecQ2 = (q2.X - p1.X) * (p2.Y - p1.Y) - (q2.Y - p1.Y) * (p2.X - p1.X);
                if(wecQ1 * wecQ2 <= 0)
                {
                    alphaP = wecP1 / (wecP1 - wecP2);
                    alphaQ = wecQ1 / (wecQ1 - wecQ2);
                    return true;
                }
            }
            return false;
        }
    }
}
