using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Algorithm.Pathfinding.Interface;
using M2M.Position.Interface;

namespace M2M.Algorithm.Pathfinding.Implement
{
    public class Dijkstra : ISearchPathEngine
    {
        #region ISearchPathEngine Members

        public void InitEngineForMap(ICollection<IPosition_Connected> map)
        {
            throw new NotImplementedException();
        }

        public ICollection<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
