using System;
using System.Linq;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    using Polygon = System.Collections.Generic.LinkedList<Vertex>;
    using Node = System.Collections.Generic.LinkedListNode<Vertex>;

    public static class VertexHelper
    {
        public static Point ToPoint(this Vertex vertex)
        {
            return new Point(vertex.X, vertex.Y);
        }

        public static bool Contains(this Polygon polygon, Vertex point)
        {
            var ps = polygon.Select(p => p.ToPoint()).ToArray();
            return PolygonAlgorithm.Contains(ps, point.ToPoint());
        }
    }

    //
    public static class Clipper
    {
        public static Polygon Clip(Polygon subject,Polygon clip)
        {
            //phase 1
            for (var si = subject.First; si != null; si = si.Next)
            {
                for (var cj = clip.First; cj != null; cj = cj.Next)
                {
                    double a = 0, b = 0;
                    if (Intersects(si.Value, si.Next.Value, cj.Value, cj.Next.Value, ref a, ref b))
                    {
                        Vertex i1 = CreateVertex(si.Value, si.Next.Value, a);
                        Vertex i2 = CreateVertex(cj.Value, cj.Next.Value, b);
                        Node n1 = subject.AddAfter(si, i1);
                        Node n2 = subject.AddAfter(cj, i2);
                        i1.Neibour = n2;
                        i2.Neibour = n1;
                    }
                    if (cj == clip.First)
                        cj = null;
                }
                if (si == subject.First)
                    si = null;
            }


            //phase 2
            Vertex p0 = subject.First.Value;
            for (var si = subject.First; si != null; si = si.Next)
            {
                //true exit false entry
                bool status = clip.Contains(p0);

                if(si.Value.IsIntersect)
                {
                    si.Value.IsExit = status;
                    status = !status;
                }
                if (si == subject.First)
                    si = null;
            }
            return null;
        }

        private static Vertex CreateVertex(Vertex v1, Vertex v2, double alpha)
        {
            var v = v2.ToPoint() - v1.ToPoint();
            v = v / v.Length * alpha;

            var p = v1.ToPoint() + v;
            var vertex = new Vertex() { X = p.X, Y = p.Y, IsIntersect = true };
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
