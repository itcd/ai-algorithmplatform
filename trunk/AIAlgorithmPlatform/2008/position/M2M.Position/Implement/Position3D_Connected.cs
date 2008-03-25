using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using Real = System.Double;

namespace M2M.Position.Implement
{
    [Serializable]
    public class Position3D_Connected : Position3D, IPosition_Connected
    {
        protected List<IAdjacency> list = new List<IAdjacency>();

        #region IPosition_Connected Members

        public IEnumerable<IAdjacency> GetAdjacency()
        {
            return list;
        }

        #endregion
    }
}
