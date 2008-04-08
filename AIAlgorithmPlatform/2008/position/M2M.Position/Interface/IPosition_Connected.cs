using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2M.Position.Interface
{
    public interface IPosition_Connected : IPosition
    {
        ICollection<IAdjacency> GetAdjacency();
        int GetIndex();
        void SetIndex(int index);
    }
}
