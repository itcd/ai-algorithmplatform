using System;
using System.Collections.Generic;
using System.Text;
using Position_Connected_Interface;
using Position_Interface;

namespace M2M
{
    class PartSet_Connected_Adjacency : IPartSet_Connected_Adjacency
    {
        List<Part_Distance> pairList = new List<Part_Distance>();
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
            IPart_Connected[] a = new IPart_Connected[pairList.Count];
            for (int i = 0; i < pairList.Count; i++)
                a[i] = pairList[i].part;
            return a;
        }

        public IPosition GetPosition()
        {
            return pairList[index].part;
        }

        public IPosition_Connected GetPosition_Connected()
        {
            return pairList[index].part;
        }

        public IPart_Connected GetPart_Connected()
        {
            return pairList[index].part;
        }

        public IPosition_Connected_Edit GetPosition_Connected_Edit()
        {
            return pairList[index].part;
        }

        public float GetDistanceToAdjacency()
        {
            return pairList[index].distance;
        }

        //interface IPositionSet_Connected_AdjacencyEdit
        public void AddAdjacency(IPart_Connected adjacency, float distance)
        {
            pairList.Add(new Part_Distance(adjacency, distance));
        }

        public void RemoveAdjacency(IPosition_Connected adjacency)
        {
            for (int i = 0; i < pairList.Count; i++)
                if (pairList[i].part == adjacency)
                {
                    pairList.RemoveAt(i);
                    break;
                }
        }

        public void ClearAdjacency()
        {
            pairList.Clear();
        }

        #region IPositionSet_Connected_AdjacencyEdit ³ÉÔ±

        public void AddAdjacency(IPosition_Connected adjacency, float distance)
        {
            pairList.Add(new Part_Distance((IPart_Connected)adjacency, distance));
        }

        

        #endregion
    }

    [Serializable]
    public struct Part_Distance
    {
        public IPart_Connected part;
        public float distance;

        public Part_Distance(IPart_Connected part, float distance)
        {
            this.part = part;
            this.distance = distance;
        }
    }
}
