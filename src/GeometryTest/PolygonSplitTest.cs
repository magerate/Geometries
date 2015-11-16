using System;
using System.Linq;

using Cession.Geometries;

using NUnit.Framework;

namespace GeometryTest
{
    //search \[.+\]\: \{(.+)\}
    //replace new Point $1,
    [TestFixture()]
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

            var p1 = new Point(-350, -52);
            var p2 = new Point(257, -30);

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

            var result = Polygon.Split(polygon1, p1, p2);

            TestHelper.PolygonArrayAreEqual(result, sr);
        }

        [Test]
        public void Polygon_SplitTest2()
        {
            var polygon1 = new Point[]
            {
                new Point  (552,808),
    new Point (910,859),
    new Point (674,633),
    new Point (909,732),
    new Point (1717,613),
    new Point (1082,644),
    new Point (1709,456),
    new Point (1049,272),
    new Point (1652,161),
    new Point (874,76),
    new Point (1116,430),
    new Point (713,288),
    new Point (1113,557),
    new Point (103,501),
    new Point (311,181),
    new Point (679,176),
    new Point (670,449),
    new Point (410,244),
    new Point (285,406),
    new Point (743,482),
    new Point (695,236),
    new Point (776,120),
    new Point (149,74),
    new Point (40,585),
    new Point (280,750),
            };

            var p1 = new Point(575, 59);
            var p2 = new Point(832, 945);

            var sr = new Point[][]{
                new Point[]{
                     new Point (588.70839985286,106.259308442156),
    new Point (149,74),
new Point (40,585),
    new Point (280,750),
    new Point (552,808),
    new Point (802.616835645765,843.702398374117),
    new Point (767.4598381607,722.499675526772),
    new Point (674,633),
    new Point (750.895391399979,665.394228717438),
    new Point (713.020875261793,534.822939618476),
    new Point (103,501),
    new Point (311,181),
    new Point (609.212963556989,176.948193429932),
                },
                new Point[]{
                     new Point (802.616835645765,843.702398374117),
    new Point (910,859),
    new Point (767.4598381607,722.499675526772),
                },
                new Point[]{
                   new Point (750.895391399979,665.394228717438),
    new Point (909,732),
    new Point (1717,613),
    new Point (1082,644),
    new Point (1709,456),
    new Point (1049,272),
    new Point (1652,161),
    new Point (874,76),
    new Point (1116,430),
    new Point (713,288),
    new Point (1113,557),
    new Point (713.020875261793,534.822939618476),
                },
                new Point[]{
                    new Point (671.849875215972,392.887118448839),
    new Point (670,449),
    new Point (410,244),
    new Point (285,406),
    new Point (695.40786939232,474.102615881695),
                },
     new Point[]{
                    new Point (695.40786939232,474.102615881695),
    new Point (743,482),
    new Point (695,236),
    new Point (776,120),
    new Point (588.70839985286,106.259308442156),
    new Point (609.212963556989,176.948193429932),
    new Point (679,176),
    new Point (671.849875215972,392.887118448839),
                },
            };

            var result = Polygon.Split(polygon1, p1, p2);

            TestHelper.PolygonArrayAreEqual(result, sr);
        }


    }
}

