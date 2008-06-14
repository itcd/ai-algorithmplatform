﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Real = System.Double;

namespace M2M.Position
{
    public class ManhattanDistanceEvaluator : IEvaluator
    {
        #region IEvaluator Members

        public Real GetDistance(IPosition p1, IPosition p2)
        {
            int d1 = p1.GetDimension();
            int d2 = p2.GetDimension();
            int d = d1 < d2 ? d1 : d2;
            Real r = 0;
            for (int i = 0; i < d; i++)
            {
                r += Math.Abs(p1.GetValue(i) - p2.GetValue(i));
            }
            return Math.Sqrt(r);
        }

        #endregion
    }
}
