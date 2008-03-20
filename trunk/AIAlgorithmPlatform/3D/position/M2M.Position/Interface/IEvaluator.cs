using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    public interface IEvaluator
    {
        double GetDistance(IPosition p1, IPosition p2);
    }
}
