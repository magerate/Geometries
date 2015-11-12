using System;
using System.Linq;

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
        public void Clipper_IntersectTest1()
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

            var result = Intersect(p1, p2);

            Assert.AreEqual(result.Length, 1);
            TestHelper.PolygonAreEqual(result[0], cr);
        }

        [Test]
        public void Clipper_IntersectTest2()
        {
            var p1 = new Point[]
            {
                new Point (-242,-220),
                new Point (-308,158),
                new Point (-166,-107),
                new Point (-135,188),
                new Point (-52,-160),
                new Point (2,238),
                new Point (76,-193),
                new Point (187,202),
                new Point (237,-80),
                new Point (277,159),
                new Point (332,-302),
            };

            var p2 = new Point[]
            {
                new Point (-377,-125),
                new Point (373,-230),
                new Point (341,-130),
                new Point (-328,-80),
                new Point (-335,-17),
                new Point (408,-72),
                new Point (331,16),
                new Point (-375,34),
            };

            var cr = new Point[][]{
                new Point[]{
                    new Point (-255.620240807029,-141.993166287016),
                    new Point (-265.630550280068,-84.6613938505181),
                    new Point (-174.312988806299,-91.4863237065546),
                    new Point (-166,-107),
                    new Point (-164.447233603982,-92.2236746185365),
                    new Point (-66.4176120212366,-99.5502532121647),
                    new Point (-52,-160),
                    new Point (-44.0253418698552,-101.223816003748),
                    new Point (61.5979749788328,-109.117935349689),
                    new Point (75.1942753898821,-188.307198554583),
                },
                new Point[]{
                    new Point (-276.690805990823,-21.3162929616483),
                    new Point (-285.952800361337,31.729674796748),
                    new Point (-239.706352216936,30.5505868837179),
                    new Point (-209.237961763228,-26.309437554539),
                },
                new Point[]{
                    new Point (-157.919846982661,-30.1082212866132),
                    new Point (-151.781102151053,28.3088666270807),
                    new Point (-96.5770575853625,26.9013980687486),
                    new Point (-81.6330772955799,-35.755290375159),
                },
                new Point[]{
                    new Point (-35.6049604264038,-39.1624861057171),
                    new Point (-26.8826074620514,25.1244857426585),
                    new Point (38.8370775761337,23.4489130363025),
                    new Point (50.6837485727299,-45.5499410114403),
                },
                new Point[]{
                    new Point (77.2383202611195,-188.593364836557),
                    new Point (98.7907822316117,-111.897666833454),
                    new Point (311.213797074518,-127.77382638823),
                    new Point (322.567615265828,-222.939466137216),
                },
                new Point[]{
                    new Point (116.075079275009,-50.3904836610034),
                    new Point (136.127832291325,20.9684122078699),
                    new Point (219.474570113829,18.8434245580044),
                    new Point (233.288495763532,-59.0671161063179),
                },
                new Point[]{
                    new Point (240.415119842917,-59.5946589385739),
                    new Point (253.398078534588,17.9785192441607),
                    new Point (293.948033136503,16.9446677104008),
                    new Point (303.637977248761,-64.2746820305274),
                },
            };

            var result = Intersect(p1, p2);
            TestHelper.PolygonArrayAreEqual(result, cr);
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

