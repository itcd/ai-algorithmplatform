using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    using real = System.Double;
    public interface IPosition2D : IPosition
    {
        real GetX();
        real GetY();
    }
}
