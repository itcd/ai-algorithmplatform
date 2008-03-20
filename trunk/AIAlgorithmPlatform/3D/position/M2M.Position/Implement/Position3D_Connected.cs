using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;
using Real = System.Double;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.Implement
{
    [Serializable]
    public class Position3D_Connected : Position3D, IPosition_Connected
    {
        #region IPosition_Connected Members

        IPosition_ConnectedSet IPosition_Connected.GetAdjacency()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConnected Members

        public IPositionSet GetAdjacency()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
