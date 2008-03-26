using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Algorithm.Pathfinding.Interface
{
    public interface ISearchPathEngine
    {
        void InitEngineForMap(IPosition_ConnectedSet map);
        //没有路径则返回null
        IPosition_ConnectedSet SearchPath(IPosition_Connected start, IPosition_Connected end);
    }
}
