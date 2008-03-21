using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.Interface
{
    public interface IPosition_Connected : IPosition, IConnected
    {
        new IPosition_ConnectedSet GetAdjacency();
       
    }
}
