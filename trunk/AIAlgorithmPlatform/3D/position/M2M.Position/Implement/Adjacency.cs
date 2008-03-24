using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using Real = System.Double;

namespace M2M.Position.Implement
{
    [Serializable]
    public class Adjacency : IAdjacency
    {
        protected IPosition_Connected p;
        protected Real d;

        public Adjacency(IPosition_Connected pisition, Real distance)
        {
            p = pisition;
            d = distance;
        }

        #region IAdjacency Members

        public IPosition_Connected GetPosition()
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
