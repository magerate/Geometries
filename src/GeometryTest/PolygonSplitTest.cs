﻿using System;
using System.Linq;

using Cession.Geometries;

using NUnit.Framework;

namespace GeometryTest
{
    //search \[.+\]\: \{(.+)\}
    //replace new Point $1,
    [TestFixture ()]
	public class SplitPolygonTest
	{
        [Test]
        public void Polygon_SplitTest1()
        {
            var polygon1 = new Point[]
            {
                new Point (-256,44),
                new Point (-165,-153),
                new Point (-80,34),
                new Point (12,-160),
                new Point (-17,65),
                new Point (165,-235),
                new Point (163,110),
            };

            var p1 = new Point(-350,-52);
            var p2 = new Point(257,-30);

            var sr = new Point[][]{
                new Point[]{
                    new Point (-213.932867800067,-47.0684070701837),
                    new Point (-165,-153),
                    new Point (-115.223085122583,-43.4907872696817),
                },
                new Point[]{
                    new Point (-44.4679167153663,-40.9263495349886),
                    new Point (12,-160),
                    new Point (-3.53846938701144,-39.4429099283596),
                },
                new Point[]{
                    new Point (45.2884301250913,-37.6732364699308),
                    new Point (165,-235),
                    new Point (163.831169823211,-33.3767945039363),
                },
                new Point[]{
                    new Point (163.831169823211,-33.3767945039363),
                    new Point (163,110),
                    new Point (-256,44),
                    new Point (-213.932867800067,-47.0684070701837),
                    new Point (-115.223085122583,-43.4907872696817),
                    new Point (-80,34),
                    new Point (-44.4679167153663,-40.9263495349886),
                    new Point (-3.53846938701144,-39.4429099283596),
                    new Point (-17,65),
                    new Point (45.2884301250913,-37.6732364699308),
                },
            };

            var result = Polygon.Split(polygon1,p1, p2);

            TestHelper.PolygonArrayAreEqual(result, sr);
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

//            var result = Intersect(p1, p2);
//            TestHelper.PolygonArrayAreEqual(result, cr);
        }
    }
}

