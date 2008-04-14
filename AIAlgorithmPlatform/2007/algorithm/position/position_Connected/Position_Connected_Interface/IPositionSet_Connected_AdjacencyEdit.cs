using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    /// <summary>
    /// ��ͼ�༭���õĽӿڣ��̳�Ѱ���㷨�õĽӿ�
    /// �����IPositionSet_Connected_AdjacencyEdit����IPositionSet_ConnectedEdit���༭���������һЩ�ڵ㡣Ȼ���������ΪIPositionSet_Connected_AdjacencyEdit���õĻ���ȱ���˵���Щ����ӽڵ�ľ�����Ϣ��
    /// </summary>
    public interface IPositionSet_Connected_AdjacencyEdit : IPositionSet_Connected_Adjacency
    {
        void AddAdjacency(IPosition_Connected adjacency, float distance);
        void RemoveAdjacency(IPosition_Connected adjacency);
        void ClearAdjacency();
        IPosition_Connected_Edit GetPosition_Connected_Edit();
    }
}
