using System;
using System.Linq;
using System.Collections.Generic;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public static class VertexHelper
    {
        public static Vertex ToVertex(this Point point)
        {
            return new Vertex() { Point = point };
        }

        public static List<Point> ToList(this Vertex vertex)
        {
            List<Point> ps = new List<Point>();
            for (var si = vertex; si != null; si = si.Next == vertex ? null : si.Next)
            {
                ps.Add(si.Point);
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
        public static List<List<Vertex>> Intersect(Vertex subject,Vertex clip)
        {
            return Clip(subject, clip, true, true);
        }

        public static List<List<Vertex>> Union(Vertex subject, Vertex clip)
        {
            return Clip(subject, clip, false, false);
        }

        public static List<List<Vertex>> Diff(Vertex subject, Vertex clip)
        {
            return Clip(subject, clip, false, true);
        }

        public static List<List<Vertex>> Clip(Vertex subject, Vertex clip, bool subjectEntry, bool clipEntry)
        {
            //phase 1
            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                for (var cj = clip; cj != null; cj = cj.Next == clip ? null : cj.Next)
                {
                    var cross = Segment.Intersect (si.Point, si.Next.Point, cj.Point, cj.Next.Point);
                    if (cross.HasValue)
                    {
                        Vertex i1 = new Vertex () {
                           Point = cross.Value,
                            IsIntersect = true,
                            Previous = si,
                            Next = si.Next
                        };
                        Vertex i2 = new Vertex () {
                            Point = cross.Value,
                            IsIntersect = true,
                            Previous = cj,
                            Next = cj.Next
                        };
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
            bool status = subjectEntry ^ PolygonAlgorithm.EOContains(clip.ToList(), subject.Point);

            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect)
                {
                    si.IsExit = status;
                    status = !status;
                }
            }

            status = clipEntry ^ PolygonAlgorithm.EOContains(subject.ToList(), clip.Point);
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
            List<List<Vertex>> polygonList = new List<List<Vertex>>();

            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect && !si.IsVisit)
                {
                    current = si;

                    var polygon = new List<Vertex>();
                    polygonList.Add(polygon);

                    polygon.Add(current);
                    do
                    {
                        current.IsVisit = true;
                        if (current.IsExit)
                        {
                            do
                            {
                                current = current.Previous;
                                if(current.Point != polygon[0].Point)
                                    polygon.Add(current);
                                current.IsVisit = true;
                            } while (!current.IsIntersect);
                        }
                        else
                        {
                            do
                            {
                                current = current.Next;
                                if(current.Point != polygon[0].Point)
                                    polygon.Add(current);
                                current.IsVisit = true;
                            } while (!current.IsIntersect);
                        }

                        current = current.Neibour;
                    } while (!current.IsVisit);
                }
            }

            return polygonList;
        }


//        private static Vertex CreateVertex(Vertex v1, Vertex v2, double alpha)
//        {
//            var v = v2.ToPoint() - v1.ToPoint();
//            v = v / v.Length * alpha;
//
//            var p = v1.ToPoint() + v;
//            var vertex = new Vertex() { X = p.X, Y = p.Y, IsIntersect = true,Previous = v1,Next = v2, };
//            return vertex;
//        }

        //seems wrong
//        public static bool Intersects(Vertex p1,Vertex p2,Vertex q1,Vertex q2,ref double alphaP,ref double alphaQ)
//        {
//            double wecP1 = (p1.X - q1.X) * (q2.Y - q1.Y) - (p1.Y - q1.Y) *(q2.X - q1.X);
//            double wecP2 = (p2.X - q1.X) * (q2.Y - q1.Y) - (p2.Y - q1.Y) * (q2.X - q1.X);
//            if(wecP1 * wecP2 <=0)
//            {
//                double wecQ1 = (q1.X - p1.X) * (p2.Y - p1.Y) - (q1.Y - p1.Y) * (p2.X - p1.X);
//                double wecQ2 = (q2.X - p1.X) * (p2.Y - p1.Y) - (q2.Y - p1.Y) * (p2.X - p1.X);
//                if(wecQ1 * wecQ2 <= 0)
//                {
//                    alphaP = wecP1 / (wecP1 - wecP2);
//                    alphaQ = wecQ1 / (wecQ1 - wecQ2);
//                    return true;
//                }
//            }
//            return false;
//        }
    }
}
