using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Geometries;
using Cession.Geometries.Clipping.GreinerHormann;

namespace GeometryTest
{
	public static class TestHelper
	{
		private static Random random = new Random();

		public static double NextDouble(double maxValue){
			return random.NextDouble () * maxValue;
		}

		public static double NextDouble(){
			return random.NextDouble ();
		}

        public static int Next()
        {
            return random.Next();
        }

        public static int Next(int min,int max)
        {
            return random.Next(min,max);
        }

        public static Point CreateRandomPoint(){
			var x = random.NextDouble ();
			var y = random.NextDouble ();
			return new Point (x, y);
		}

        public static Vertex CreateRandomVertex()
        {
            var x = random.NextDouble();
            var y = random.NextDouble();
            return new Vertex() { X = x,Y = y };
        }

        public static Point CreateRandomPoint(double maxValue){
			var x = random.NextDouble () * maxValue;
			var y = random.NextDouble () * maxValue;
			return new Point (x, y);
		}

        public static Point[] CreateRandomPointArray()
        {
            int count = 10;
            //int count = Next(100, 200);

            Point[] ps = new Point[count];
            for (int i = 0; i < count; i++)
            {
                ps[i] = CreateRandomPoint();
            }
            return ps;
        }

        public static Point[] CreateRandomPointArray(int count)
        {
            Point[] ps = new Point[count];
            for (int i = 0; i < count; i++)
            {
                ps[i] = CreateRandomPoint();
            }
            return ps;
        }

        public static void AlmostEqual(double left,double right){
			AlmostEqual (left, right, 1e-10);
		}

		public static void AlmostEqual(double left,double right,double delta){
			Assert.LessOrEqual (Math.Abs (left - right), delta);
		}

		public static void AlmostZero(double value){
			Assert.LessOrEqual(Math.Abs (value),1e-5);
		}

		public static void RunBatch(this Action action,int count = 10000){
			for (int i = 0; i < count; i++) {
				action ();
			}
		}

		public static void PointAreEqual(Point p1,Point p2){
			Assert.AreEqual (p1.X, p2.X);
			Assert.AreEqual (p1.Y, p2.Y);
		}

        public static void PointAreAlmostEqual(Point p1, Point p2)
        {
            AlmostEqual(p1.X, p2.X,1e-12);
            AlmostEqual(p1.Y, p2.Y,1e-12);
        }

        public static Point[] ToPointArray(this List<Vertex> polygon)
        {
            return polygon.Select(p => p.ToPoint()).ToArray();
        }

        public static void PolygonAreEqual(Point[] p1,Point[] p2)
        {
            Assert.AreEqual(p1.Length, p2.Length);

            for (int i = 0; i < p1.Length; i++)
            {
                PointAreAlmostEqual(p1[i], p2[i]);
            }
        }

        public static void PolygonArrayAreEqual(Point[][] p1, Point[][] p2)
        {
            Assert.AreEqual(p1.Length, p2.Length);
            for (int i = 0; i < p1.Length; i++)
            {
                PolygonAreEqual(p1[i], p2[i]);
            }
        }

    }
}

