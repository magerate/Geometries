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

        //reference http://geomalgorithms.com/a03-_inclusion.html
        public static int GetWindNumber (IList<Point> polygon, Point point)
        {
            int windNumber = 0;

            for (int i = 0; i < polygon.Count; i++)
            { 
                if (polygon [i].Y <= point.Y)
                {         
                    if (polygon [i + 1].Y > point.Y)
                    {
                        if (Triangle.GetSignedArea (polygon [i], polygon [i + 1], point) < 0)
                            windNumber++;
                    }
                }
                else
                {                        
                    if (Triangle.GetSignedArea (polygon [i], polygon [i + 1], point) < 0)
                        windNumber--;           
                }
            }
            return windNumber;
        }
    }
}

