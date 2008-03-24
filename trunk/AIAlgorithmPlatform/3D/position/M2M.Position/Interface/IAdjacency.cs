using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Real = System.Double;

namespace M2M.Position.Interface
{
    public interface IAdjacency
    {
        IPosition_Connected GetPosition();
        Real GetDistance();
    }
}
