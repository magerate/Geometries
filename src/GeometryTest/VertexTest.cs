using System;
using NUnit.Framework;

using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

namespace GeometryTest
{
	[TestFixture]
	public class VertexTest
	{
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
                    TestHelper.AlmostEqual(a, aa, 1e-10);
                    TestHelper.AlmostEqual(b, bb, 1e-10);
                }
            };

            action.RunBatch();
        }
    }
}

