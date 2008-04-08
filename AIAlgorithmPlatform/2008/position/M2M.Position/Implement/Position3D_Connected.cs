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
        protected int index = -1;

        public Position3D_Connected(Real x, Real y) : base(x, y)
        {
        }

        public Position3D_Connected(Real x, Real y, Real z) : base(x, y, z)
        {
        }

        #region IPosition_Connected Members

        public ICollection<IAdjacency> GetAdjacency()
        {
            return list;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int i)
        {
            index = i;
        }

        #endregion
    }
}
