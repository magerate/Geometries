using System;
using System.Linq;
using System.Xml.Serialization;

using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

using NUnit.Framework;

namespace GeometryTest
{
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
                [0]: {(-190,-194)}
                [1]: {(-265,75)}
                [2]: {(-40,-112)}
                [3]: {(56,84)}
                [4]: {(194,-114)}
                [5]: {(292,-259)}
                [6]: {(65,-163)}
            };

            var p2 = new Point[]
            {
                [0]: {(-60,72)}
                [1]: {(-6,-243)}
                [2]: {(202,-21)}
                [3]: {(294,-122)}
                [4]: {(340,97)}
                [5]: {(75,-45)}
            };

            var cr = new Point[]
            {
                [0]: {(-31.4497354497354,-94.5432098765432)}
                [1]: {(17.3065902578797,5.00095510983763)}
                [2]: {(75,-45)}
                [3]: {(126.627674631588,-17.3353592540172)}
                [4]: {(160.243609022556,-65.5669172932331)}
                [5]: {(67.8325800858418,-164.197919331457)}
                [6]: {(65,-163)}
                [7]: {(-17.9848534738229,-173.088354736033)}
            };

            var l1 = p1.ToLinkList();
            var l2 = p2.ToLinkList();

            var result = Clipper.Clip(l1, l2);

            Assert.AreEqual(result.Count, 1);
            TestHelper.PolygonAreEqual(result[0].ToPointArray(), cr);
        }
//        [Test]
//        public void Clipper_IntersectTest1()
//        {
//            string strSub = Resource1.subject1;
//            var subject = StringToPolygon(strSub);
//
//            string strClip = Resource1.clip1;
//            var clip = StringToPolygon(strClip);
//
//            var result = Intersect(subject, clip);
//
//            string strResult = Resource1.result1;
//            var cr = StringToPolygonArray(strResult);
//
//            TestHelper.PolygonArrayAreEqual(cr, result);
//        }

        private Point[][] StringToPolygonArray(string str)
        {
            var textReader = new System.IO.StringReader(str);
            var serializer = new XmlSerializer(typeof(Point[][]));
            return serializer.Deserialize(textReader) as Point[][];
        }

        private Point[] StringToPolygon(string str)
        {
            var textReader = new System.IO.StringReader(str);
            var serializer = new XmlSerializer(typeof(Point[]));
            return serializer.Deserialize(textReader) as Point[];
        }

        [Test]
        public void Clipper_ClipTestN()
        {
            Action test = () =>
            {
                var p1 = TestHelper.CreateRandomPointArray(200);
                var p2 = TestHelper.CreateRandomPointArray(200);

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

            test.RunBatch(1000);
        }
    }
}

