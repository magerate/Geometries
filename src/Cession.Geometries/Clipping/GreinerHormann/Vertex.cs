using System;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public sealed class Vertex
    {
        public Point Point{ get; set; }

        public Vertex Previous { get; set; }
        public Vertex Next { get; set; }
        //since return result is list, this field is not used anymore
        public Vertex NextPolygon { get; set; }

        public bool IsIntersect { get; set; }
        public bool IsExit { get; set; }

        public Vertex Neibour { get; set; }
        public float Alpha { get; set; }
        public bool IsVisit { get; set; }

        public override string ToString()
        {
            return Point.ToString ();
        }

        public bool Contains(Point point)
        {
            int windNumber = 0;
            Vertex si = this;
            do
            {
                if (si.Point.Y <= point.Y)
                {
                    if (si.Next.Point.Y > point.Y)
                    {
                        if (Triangle.GetSignedArea(si.Point, si.Next.Point, point) > 0)
                            windNumber++;
                    }
                }
                else
                {
                    if (si.Next.Point.Y <= point.Y)
                        if (Triangle.GetSignedArea(si.Point, si.Next.Point, point) < 0)
                            windNumber--;
                }
                si = si.Next;
            } while (si != this);


            return (windNumber & 1) != 0;
        }
    }

   
}
