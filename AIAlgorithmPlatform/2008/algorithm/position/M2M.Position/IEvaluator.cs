﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Real = System.Double;

namespace M2M.Position
{
    public interface IEvaluator
    {
        Real GetDistance(IPosition p1, IPosition p2);
    }
}