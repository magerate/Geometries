using System;
using System.Linq;
using System.Xml.Serialization;

using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

using NUnit.Framework;

namespace GeometryTest
{
    //search \[.+\]\: \{(.+)\}
    //replace new Point $1,

    [TestFixture ()]
	public class ClipperTest
	{
        public static Point[][] Intersect(Point[] subject, Point[] clip)
        {
            var v1 = subject.ToLinkList();
            var v2 = clip.ToLinkList();
            var result = Clipper.Intersect(v1, v2);
            var pa = result.Select(l => l.Select(v => v.ToPoint()).ToArray()).ToArray();

            return pa;
        }
        [Test]
        public void Vertex_IntersectsTest()
        {
            Action action = () =>
            {
                var v1 = TestHelper.CreateRandomVertex();
                var v2 = TestHelper.CreateRandomVertex();

                var v3 = TestHelper.CreateRandomVertex();
                var v4 = TestHelper.CreateRandomVertex();

                double a = 0, b = 0;
                if (Vertex.Intersects(v1, v2, v3, v4, ref a, ref b))
                {
                    //Assert.True(false);
                    var cross = Segment.Intersect(v1.ToPoint(), v2.ToPoint(), v3.ToPoint(), v4.ToPoint());
                    double aa = cross.Value.DistanceBetween(v1.ToPoint()) / v1.ToPoint().DistanceBetween(v2.ToPoint());

                    double bb = cross.Value.DistanceBetween(v3.ToPoint()) / v3.ToPoint().DistanceBetween(v4.ToPoint());
                    //Assert.AreEqual(a, aa);
                    //Assert.AreEqual(b, bb);
                    TestHelper.AlmostEqual(a, aa,1e-10);
                    TestHelper.AlmostEqual(b, bb,1e-10);
                }
            };

            action.RunBatch();
        }

     
        [Test]
        public void Clipper_ClipTest1()
        {
            var p1 = new Point[]
            {
                new Point (-190,-194),
                new Point (-265,75),
                new Point (-40,-112),
                new Point (56,84),
                new Point (194,-114),
                new Point (292,-259),
                new Point (65,-163),
            };

            var p2 = new Point[]
            {
                new Point (-60,72),
                new Point (-6,-243),
                new Point (202,-21),
                new Point (294,-122),
                new Point (340,97),
                new Point (75,-45),
            };

            var cr = new Point[]
            {
                new Point (-31.4497354497354,-94.5432098765432),
                new Point (17.3065902578797,5.00095510983763),
                new Point (75,-45),
                new Point (126.627674631588,-17.3353592540172),
                new Point (160.243609022556,-65.5669172932331),
                new Point (67.8325800858418,-164.197919331457),
                new Point (65,-163),
                new Point (-17.9848534738229,-173.088354736033),
            };

            var l1 = p1.ToLinkList();
            var l2 = p2.ToLinkList();

            var result = Clipper.Intersect(l1, l2);

            Assert.AreEqual(result.Count, 1);
            TestHelper.PolygonAreEqual(result[0].ToPointArray(), cr);
        }

        [Test]
        public void Clipper_ClipTestN()
        {
            Action test = () =>
            {
                var p1 = TestHelper.CreateRandomPointArray();
                var p2 = TestHelper.CreateRandomPointArray();

                var result = Clipper.Intersect(p1.ToLinkList(), p2.ToLinkList());

                foreach (var item in result)
                {
                    foreach (var v in item)
                    {
                        PolygonAlgorithm.Contains(p1, v.ToPoint());
                        PolygonAlgorithm.Contains(p2, v.ToPoint());
                    }
                }
            };

            test.RunBatch();
        }
    }
}

