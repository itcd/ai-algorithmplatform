using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Real = System.Double;

namespace M2M.Position
{
    [Serializable]
    public class Position3D_Connected : Position3D, IPosition_Connected
    {
        protected List<IAdjacency> list_out = new List<IAdjacency>();
        protected List<IAdjacency> list_in = new List<IAdjacency>();
        protected int index = -1;

        public Position3D_Connected(Real x, Real y) : base(x, y)
        {
        }

        public Position3D_Connected(Real x, Real y, Real z) : base(x, y, z)
        {
        }

        #region IPosition_Connected Members

        public ICollection<IAdjacency> GetAdjacencyOut()
        {
            return list_out;
        }

        public ICollection<IAdjacency> GetAdjacencyIn()
        {
            return list_in;
        }

        public int GetTagIndex()
        {
            return index;
        }

        public void SetTagIndex(int i)
        {
            index = i;
        }

        #endregion
    }
}