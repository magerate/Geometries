using System;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

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
            return $"({X.ToString()},{Y.ToString()})";
        }
    }

   
}
