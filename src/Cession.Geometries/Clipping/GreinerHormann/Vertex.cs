using System;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public sealed class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vertex Previous { get; set; }
        public Vertex Next { get; set; }
        //since return result is list, this field is not used anymore
        //public Vertex NextPolygon { get; set; }

        public bool IsIntersect { get; set; }
        public bool IsExit { get; set; }

        public Vertex Neibour { get; set; }
        public double Alpha { get; set; }
        public bool IsVisit { get; set; }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public Vertex NonIntersectionNext
        {
            get{
                Vertex v = this.Next;
                while (v.IsIntersect)
                    v = v.Next;
                return v;
            }
        }

        public void InsertTo(Vertex start,Vertex end)
        {
            Vertex prev, next = start;
            while (next != end && next.Alpha < Alpha)
                next = next.Next;
            prev = next.Previous;
            Next = next;
            Previous = prev;
            prev.Next = this;
            next.Previous = this;
        }

        public bool Contains(Vertex point)
        {
            int windNumber = 0;
            Vertex si = this;
            do
            {
                if (si.Y <= point.Y)
                {
                    if (si.Next.Y > point.Y)
                    {
                        if (Triangle.GetSignedArea(si.ToPoint(), si.Next.ToPoint(), point.ToPoint()) > 0)
                            windNumber++;
                    }
                }
                else
                {
                    if (si.Next.Y <= point.Y)
                        if (Triangle.GetSignedArea(si.ToPoint(), si.Next.ToPoint(), point.ToPoint()) < 0)
                            windNumber--;
                }
                si = si.Next;
            } while (si != this);


            return (windNumber & 1) != 0;
        }

        public static bool Intersects(Vertex p1, Vertex p2, Vertex q1, Vertex q2, ref double alphaP, ref double alphaQ)
        {
            double wecP1 = (p1.X - q1.X) * (q2.Y - q1.Y) - (p1.Y - q1.Y) * (q2.X - q1.X);
            double wecP2 = (p2.X - q1.X) * (q2.Y - q1.Y) - (p2.Y - q1.Y) * (q2.X - q1.X);
            if (wecP1 * wecP2 <= 0)
            {
                double wecQ1 = (q1.X - p1.X) * (p2.Y - p1.Y) - (q1.Y - p1.Y) * (p2.X - p1.X);
                double wecQ2 = (q2.X - p1.X) * (p2.Y - p1.Y) - (q2.Y - p1.Y) * (p2.X - p1.X);
                if (wecQ1 * wecQ2 <= 0)
                {
                    alphaP = wecP1 / (wecP1 - wecP2);
                    alphaQ = wecQ1 / (wecQ1 - wecQ2);
                    return true;
                }
            }
            return false;
        }

        public static Vertex Create(Vertex v1, Vertex v2, double alpha)
        {
            var v = v2.ToPoint() - v1.ToPoint();
            v = v / v.Length * alpha;

            var p = v1.ToPoint() + v;
            var vertex = new Vertex() { X = p.X, Y = p.Y, IsIntersect = true,Alpha = alpha};
            return vertex;
        }
    }


}
