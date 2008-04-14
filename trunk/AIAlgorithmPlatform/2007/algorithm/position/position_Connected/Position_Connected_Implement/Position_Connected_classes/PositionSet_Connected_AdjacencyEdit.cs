using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    ///地图编辑器用的接口的实现
    [Serializable]
    public class PositionSet_Connected_AdjacencyEdit : IPositionSet_Connected_AdjacencyEdit
    {
        List<Position_Distance> pairList = new List<Position_Distance>();
        int index = -1;

        public void InitToTraverseSet()
        {
            index = -1;
        }

        public bool NextPosition()
        {
            int i = index + 1;
            if (i < pairList.Count)
            {
                index = i;
                return true;
            }
            else
                return false;
        }

        public int GetNum()
        {
            return pairList.Count;
        }

        public Array ToArray()
        {
            IPosition_Connected[] a = new IPosition_Connected[pairList.Count];
            for (int i = 0; i < pairList.Count; i++)
                a[i] = pairList[i].position;
            return a;
        }

        public IPosition GetPosition()
        {
            if (index >= 0)
                return pairList[index].position;
            else
                return null;
        }

        public IPosition_Connected GetPosition_Connected()
        {
            if (index >= 0)
                return pairList[index].position;
            else
                return null;
        }

        public IPosition_Connected_Edit GetPosition_Connected_Edit()
        {
            if (index >= 0)
                return (IPosition_Connected_Edit)pairList[index].position;
            else
                return null;
        }

        public float GetDistanceToAdjacency()
        {
            if (index >= 0)
                return pairList[index].distance;
            else
                return -1;
        }

        //interface IPositionSet_Connected_AdjacencyEdit
        public void AddAdjacency(IPosition_Connected adjacency, float distance)
        {
            pairList.Add(new Position_Distance(adjacency, distance));
        }

        public void RemoveAdjacency(IPosition_Connected adjacency)
        {
            for (int i = 0; i < pairList.Count; i++)
                if (pairList[i].position == adjacency)
                {
                    pairList.RemoveAt(i);
                    break;
                }
        }

        public void ClearAdjacency()
        {
            pairList.Clear();
        }
    }
}
