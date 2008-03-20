using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    using real = System.Double;
    public interface IPosition3D : IPosition
    {
        real GetX();
        real GetY();
        real GetZ();
    }
}
