using System;
using System.Collections.Generic;
using System.Text;

namespace M2M.Util.DataStructure
{
    [Serializable]
    public class EuclideanDistance
    {
        public static float getDistance(float x1, float y1, float x2, float y2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
