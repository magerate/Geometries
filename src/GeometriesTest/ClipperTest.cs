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
            var pa = result.Select(l => l.Select(v => v.Point).ToArray()).ToArray();

            return pa;
        }
        //[Test]
        //public void Clipper_IntersectsTest()
        //{
        //    Action action = () =>
        //    {
        //        var v1 = TestHelper.CreateRandomVertex();
        //        var v2 = TestHelper.CreateRandomVertex();

            //        var v3 = TestHelper.CreateRandomVertex();
            //        var v4 = TestHelper.CreateRandomVertex();

            //        double a = 0, b = 0;
            //        if (Clipper.Intersects(v1, v2, v3, v4, ref a, ref b))
            //        {
            //            //Assert.True(false);
            //            var cross = Segment.Intersect(v1.ToPoint(), v2.ToPoint(), v3.ToPoint(), v4.ToPoint());
            //            double aa = cross.Value.DistanceBetween(v1.ToPoint()) / v1.ToPoint().DistanceBetween(v2.ToPoint());

            //            double bb = cross.Value.DistanceBetween(v3.ToPoint()) / v3.ToPoint().DistanceBetween(v4.ToPoint());
            //            TestHelper.AlmostEqual(a, aa,1e-10);
            //            TestHelper.AlmostEqual(b, bb, 1e-10);
            //        }
            //    };

            //    action.RunBatch();
            //}

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
        public void Clipper_IntersectTest1()
        {
            string strSub = Resource1.subject1;
            var subject = StringToPolygon(strSub);

            string strClip = Resource1.clip1;
            var clip = StringToPolygon(strClip);

            var result = Intersect(subject, clip);

            string strResult = Resource1.result1;
            var cr = StringToPolygonArray(strResult);

            TestHelper.PolygonArrayAreEqual(cr, result);
        }

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
                var p1 = TestHelper.CreateRandomPointArray(100);
                var p2 = TestHelper.CreateRandomPointArray(100);

                var result = Clipper.Intersect(p1.ToLinkList(), p2.ToLinkList());

                foreach (var item in result)
                {
                    foreach (var v in item)
                    {
                        PolygonAlgorithm.Contains(p1, v.Point);
                        PolygonAlgorithm.Contains(p2, v.Point);
                    }
                }
            };

            test.RunBatch(100);
        }
    }
}

