using System;

namespace Cession.Geometries
{
    public struct Ray : IEquatable<Ray>
    {
        private Point _point;
        private Vector _direction;

        public Point Point {
            get { return _point; }
            set { _point = value; }
        }

        public Vector Direction {
            get { return _direction; }
            set { _direction = value; }
        }

        public Ray (Point point, Vector direction)
        {
            this._point = point;
            this._direction = direction;
        }

        public bool Equals (Ray ray)
        {
            return _point == ray._point && _direction.CrossProduct (ray._direction) == 0;
        }

        public override bool Equals (object obj)
        {
            if (null == obj || !(obj is Ray))
                return false;
            return Equals ((Ray)obj);
        }

        public override int GetHashCode ()
        {
            return _point.GetHashCode () ^ _direction.Angle.GetHashCode ();
        }

        public static double GetRayRange(double value1,double value2)
        {
            if (value2 > value1)
                return double.PositiveInfinity;
            else if (value2 < value1)
                return double.NegativeInfinity;
            else
                return value2;
        }

        public static Point? IntersectWithSegment(Point p1,Point p2,Point p3,Point p4)
        {
            var cross = Line.Intersect(p1, p2, p3, p4);

            if (cross != null &&
                 Range.Contains(p3.X, p4.X, cross.Value.X) &&
                 Range.Contains(p3.Y, p4.Y, cross.Value.Y)&&

                 Range.Contains(p1.X, GetRayRange(p1.X,p2.X), cross.Value.X) &&
                 Range.Contains(p1.Y, GetRayRange(p1.Y,p2.Y), cross.Value.Y))
                return cross;
            return null;
        }
    }
}
