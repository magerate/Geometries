using NUnit.Framework;
using System;
using Cession.Geometries;

namespace GeometryTest
{
	[TestFixture ()]
	public class PolygonAlgorithmTest
    {
        [Test]
        public void ContainsTest()
        {
            var p1 = new Point(-1, -1);
            var p2 = new Point(1, 1);
            var p3 = new Point(1, -1);

            var polygon = new[] { p1, p2, p3 };

            Assert.True(PolygonAlgorithm.Contains(polygon,new Point(0,0)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(0.3, 0)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(0.3, 0.3)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(0.5, 0.5)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(0.8, 0.8)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(0.8, 0.8)));
            Assert.True(PolygonAlgorithm.Contains(polygon, new Point(-0.9, -0.9)));

            Assert.False(PolygonAlgorithm.Contains(polygon, new Point(0.8, 0.9)));
            Assert.False(PolygonAlgorithm.Contains(polygon, new Point(0.8, -1.1)));
        }
    }
}

