using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngineLibrary
{
    [Serializable]
    public class ManhattanDistance
    {
        public static float getDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static float getApproxDistance(float x1, float y1, float x2, float y2)
        {
            float dx = Math.Abs(x1 - x2);
            float dy = Math.Abs(y1 - y2);
            return 1.414f * Math.Min(dx, dy) + Math.Abs(dx - dy);
        }
    }
}
