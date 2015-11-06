using System;
using System.Collections.Generic;

namespace Cession.Geometries
{
    public static class PolygonAlgorithm
    {
        public static bool Contains (IList<Point> polygon, Point point)
        {
            if (null == polygon)
                throw new ArgumentNullException ("polygon");
            
            if (polygon.Count < 3)
                return false;
            
            int windNumber = GetWindNumber (polygon, point);
            return windNumber != 0;
        }

        //reference 
        //http://geomalgorithms.com/a03-_inclusion.html
        public static int GetWindNumber (IList<Point> polygon, Point point)
        {
            int windNumber = 0;

            for (int i = 0; i < polygon.Count; i++)
            { 
                if (polygon [i].Y <= point.Y)
                {         
                    if (polygon.NextPoint(i).Y > point.Y)
                    {
                        if (Triangle.GetSignedArea (polygon [i], polygon.NextPoint(i), point) > 0)
                            windNumber++;
                    }
                }
                else
                {
                    if (polygon.NextPoint(i).Y <= point.Y)
                        if (Triangle.GetSignedArea (polygon [i], polygon.NextPoint(i), point) < 0)
                        windNumber--;           
                }
            }
            return windNumber;
        }

        private static Point NextPoint(this IList<Point> polygon,int index)
        {
            return polygon[(index + 1) % polygon.Count];
        }

        public static Rect GetBounds(IList<Point> polygon)
        {
            if (null == polygon)
                throw new ArgumentNullException();
            if (polygon.Count < 3)
                throw new ArgumentException("polygon");

            double left = double.MaxValue;
            double right = double.MinValue;
            double top = double.MaxValue;
            double bottom = double.MinValue;

            for (int i = 0; i < polygon.Count; i++)
            {
                left = Math.Min(polygon[i].X, left);
                right = Math.Max(polygon[i].X, right);

                top = Math.Min(polygon[i].Y, top);
                bottom = Math.Max(polygon[i].Y, bottom);
            }

            if (left == right || top == bottom)
                return Rect.Empty;

            return Rect.FromLTRB(left, top, right, bottom);
        }
    }
}

