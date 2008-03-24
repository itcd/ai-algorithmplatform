using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Real = System.Double;

namespace M2M.Position.Interface
{
    public interface IPosition
    {
        int GetDimension();
        Real GetValue(int dimensionIndex);
    }
}
