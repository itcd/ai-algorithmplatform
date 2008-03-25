using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    /// <summary>
    /// 地图编辑器用的接口，继承寻径算法用的接口
    /// 如果把IPositionSet_Connected_AdjacencyEdit当作IPositionSet_ConnectedEdit来编辑，比如添加一些节点。然后如果再作为IPositionSet_Connected_AdjacencyEdit来用的话，缺少了到这些新添加节点的距离信息。
    /// </summary>
    public interface IPositionSet_Connected_AdjacencyEdit : IPositionSet_Connected_Adjacency
    {
        void AddAdjacency(IPosition_Connected adjacency, float distance);
        void RemoveAdjacency(IPosition_Connected adjacency);
        void ClearAdjacency();
        IPosition_Connected_Edit GetPosition_Connected_Edit();
    }
}
