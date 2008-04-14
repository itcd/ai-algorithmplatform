using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Real = System.Double;

namespace M2M.Position
{
    public interface IPosition2D : IPosition
    {
        Real GetX();
        Real GetY();
    }
}