using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    public interface IPosition_Connected : IPosition
    {
        IEnumerable<IAdjacency> GetAdjacency();
    }
}
