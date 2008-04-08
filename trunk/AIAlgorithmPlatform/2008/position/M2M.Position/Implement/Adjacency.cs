using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using Real = System.Double;

namespace M2M.Position.Implement
{
    [Serializable]
    public struct Adjacency : IAdjacency
    {
        private IPosition_Connected p;
        private Real d;

        public Adjacency(IPosition_Connected pisition, Real distance)
        {
            p = pisition;
            d = distance;
        }

        #region IAdjacency Members

        public IPosition_Connected GetPosition_Connected()
        {
            return p;
        }

        public double GetDistance()
        {
            return d;
        }

        #endregion
    }
}
