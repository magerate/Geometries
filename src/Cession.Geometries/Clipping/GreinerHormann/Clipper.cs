﻿using System;
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
            return new Vertex() { X = point.X,Y = point.Y };
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

        public static List<Vertex> ToVertexList(this Vertex vertex)
        {
            List<Vertex> ps = new List<Vertex>();
            Vertex si = vertex;
            do
            {
                ps.Add(si);
                si = si.Next;
            } while (si != vertex);
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

    public enum ClipType
    {
        Inersection,
        Union,
        Difference,
    }
    //
    public static class Clipper
    {
        public static List<List<Vertex>> Intersect(Vertex subject,Vertex clip)
        {
            return Clip(subject, clip, ClipType.Inersection);
        }

        public static List<List<Vertex>> Union(Vertex subject, Vertex clip)
        {
            return Clip(subject, clip, ClipType.Union);
        }

        public static List<List<Vertex>> Diff(Vertex subject, Vertex clip)
        {
            return Clip(subject, clip, ClipType.Difference);
        }

        public static List<List<Vertex>> Clip(Vertex subject, Vertex clip, ClipType clipType)
        {
            bool isIntersect = false;
            //phase 1
            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect)
                    continue;
                
                for (var cj = clip; cj != null; cj = cj.Next == clip ? null : cj.Next)
                {
                    if (cj.IsIntersect)
                        continue;
                    
                    var cross = Segment.Intersect (si.ToPoint(), si.NonIntersectionNext.ToPoint(), 
                            cj.ToPoint(), cj.NonIntersectionNext.ToPoint());
                    if (cross.HasValue)
                    {
                        double a = cross.Value.DistanceBetween (si.ToPoint ()) /
                                   si.ToPoint ().DistanceBetween (si.NonIntersectionNext.ToPoint ());
                        double b = cross.Value.DistanceBetween (cj.ToPoint ()) /
                            cj.ToPoint ().DistanceBetween (cj.NonIntersectionNext.ToPoint ());

                        Vertex i1 = new Vertex () {
                            X = cross.Value.X,
                            Y = cross.Value.Y,
                            IsIntersect = true,
                            Alpha = a,
                        };
                        Vertex i2 = new Vertex () {
                            X = cross.Value.X,
                            Y = cross.Value.Y,
                            IsIntersect = true,
                            Alpha = b,
                        };

                        i1.Neibour = i2;
                        i2.Neibour = i1;

                        i1.InsertTo (si, si.NonIntersectionNext);
                        i2.InsertTo (cj, cj.NonIntersectionNext);
                        isIntersect = true;
                    }
                }
            }

            if(!isIntersect)
            {
                List<List<Vertex>> result = new List<List<Vertex>>();
                if (subject.Contains(clip))
                {
                    result.Add(clip.ToVertexList());
                }
                else if (clip.Contains(subject))
                {
                    result.Add(subject.ToVertexList());
                }
                return result;
            }

            //phase 2
            //true exit false entry
            bool status = clip.Contains(subject);
            if (clipType == ClipType.Union || clipType == ClipType.Difference)
                status = !status;


            for (var si = subject; si != null; si = si.Next == subject ? null : si.Next)
            {
                if (si.IsIntersect)
                {
                    si.IsExit = status;
                    status = !status;
                }
            }

            status = subject.Contains(clip);
            if (clipType == ClipType.Union)
                status = !status;
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
                                if(current.Neibour!= polygon[0])
                                    polygon.Add(current);
                                current.IsVisit = true;
                            } while (!current.IsIntersect);
                        }
                        else
                        {
                            do
                            {
                                current = current.Next;
                                if(current.Neibour != polygon[0])
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
        //public static bool Intersects(Vertex p1, Vertex p2, Vertex q1, Vertex q2, ref double alphaP, ref double alphaQ)
        //{
        //    double wecP1 = (p1.X - q1.X) * (q2.Y - q1.Y) - (p1.Y - q1.Y) * (q2.X - q1.X);
        //    double wecP2 = (p2.X - q1.X) * (q2.Y - q1.Y) - (p2.Y - q1.Y) * (q2.X - q1.X);
        //    if (wecP1 * wecP2 <= 0)
        //    {
        //        double wecQ1 = (q1.X - p1.X) * (p2.Y - p1.Y) - (q1.Y - p1.Y) * (p2.X - p1.X);
        //        double wecQ2 = (q2.X - p1.X) * (p2.Y - p1.Y) - (q2.Y - p1.Y) * (p2.X - p1.X);
        //        if (wecQ1 * wecQ2 <= 0)
        //        {
        //            alphaP = wecP1 / (wecP1 - wecP2);
        //            alphaQ = wecQ1 / (wecQ1 - wecQ2);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
