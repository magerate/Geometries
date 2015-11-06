using System;
using System.Collections.Generic;

namespace Cession.Geometries.Clipping.GreinerHormann
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

        public LinkedList<Vertex> NextPolygon { get; set; }

        public bool IsIntersect { get; set; }
        public bool IsExit { get; set; }

        public LinkedListNode<Vertex> Neibour { get; set; }
        public float Alpha { get; set; }
    }

   
}
