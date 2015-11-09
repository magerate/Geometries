using NUnit.Framework;
using System;
using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

namespace GeometryTest
{
	[TestFixture ()]
	public class ClipperTest
	{
        [Test]
        public void Clipper_IntersectsTest()
        {
            Action action = () =>
            {
                var v1 = TestHelper.CreateRandomVertex();
                var v2 = TestHelper.CreateRandomVertex();

                var v3 = TestHelper.CreateRandomVertex();
                var v4 = TestHelper.CreateRandomVertex();

                double a = 0, b = 0;
                if (Clipper.Intersects(v1, v2, v3, v4, ref a, ref b))
                {
                    var cross = Segment.Intersect(v1.ToPoint(), v2.ToPoint(), v3.ToPoint(), v4.ToPoint());
                    double aa = cross.Value.DistanceBetween(v1.ToPoint()) / v1.ToPoint().DistanceBetween(v2.ToPoint());

                    double bb = cross.Value.DistanceBetween(v3.ToPoint()) / v3.ToPoint().DistanceBetween(v4.ToPoint());
                    TestHelper.AlmostEqual(a, aa,1e-10);
                    TestHelper.AlmostEqual(b, bb, 1e-10);
                }
            };

            action.RunBatch();
        }

        //[Test]
        //public void Clipper_ClipTest1()
        //{
        //    var p1 = new Point[]
        //    {
        //        new Point(0,0),
        //        new Point(1,0),
        //        new Point(1,1),
        //        new Point(0,1),
        //    };

        //    var p2 = new Point[]
        //    {
        //        new Point(0.5,0.5),
        //        new Point(1.5,0.5),
        //        new Point(1.5,1.5),
        //        new Point(0.5,1.5),
        //    };

        //    var cr = new Point[]
        //    {
        //        new Point(0.5,0.5),
        //        new Point(1,0.5),
        //        new Point(1,1),
        //        new Point(0.5,1),
        //    };

        //    var l1 = p1.ToLinkList();
        //    var l2 = p2.ToLinkList();

        //    var result = Clipper.Clip(l1, l2);

        //    Assert.AreEqual(result.Count, 1);
        //    TestHelper.PolygonAreEqual(result[0].ToPointArray(), cr);
        //}

        [Test]
        public void Clipper_ClipTest2()
        {
            Action test = () =>
            {
                var p1 = TestHelper.CreateRandomPointArray();
                var p2 = TestHelper.CreateRandomPointArray();

                var result = Clipper.Clip(p1.ToLinkList(), p2.ToLinkList());

                foreach (var item in result)
                {
                    foreach (var v in item)
                    {
                        PolygonAlgorithm.Contains(p1, v.ToPoint());
                        PolygonAlgorithm.Contains(p2, v.ToPoint());
                    }
                }
            };

            test.RunBatch(10);
        }
    }
}

