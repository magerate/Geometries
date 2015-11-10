using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

using GeometryTest;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }

        static void Test2()
        {
            var p1 = new Point[]
           {
                new Point(0,0),
                new Point(1,0),
                new Point(1,1),
                new Point(0,1),
           };

            var p2 = new Point[]
            {
                new Point(0.5,0.5),
                new Point(1.5,0.5),
                new Point(1.5,1.5),
                new Point(0.5,1.5),
            };

            var l1 = p1.ToLinkList();
            var l2 = p2.ToLinkList();

            var result = Clipper.Clip(l1, l2);
            Console.WriteLine();

        }

        static void Test1()
        {
            var p1 = TestHelper.CreateRandomPointArray();
            var p2 = TestHelper.CreateRandomPointArray();

            var result = Clipper.Clip(p1.ToLinkList(), p2.ToLinkList());
            Console.WriteLine();
        }
    }
}
