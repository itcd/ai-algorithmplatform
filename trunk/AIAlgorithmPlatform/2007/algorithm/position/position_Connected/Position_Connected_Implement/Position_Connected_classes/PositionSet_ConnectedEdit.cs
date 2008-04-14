using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    //地图编辑器用的接口的实现
    [Serializable]
    public class PositionSet_ConnectedEdit : IPositionSet_ConnectedEdit, IPositionSetEdit
    {
        List<IPosition_Connected> list;
        int index = -1;

        // IPositionSet_Connected
        #region IPositionSet_Connected

        public PositionSet_ConnectedEdit()
        {
            list = new List<IPosition_Connected>();
        }

        public PositionSet_ConnectedEdit(List<IPosition_Connected> list)
        {
            this.list = new List<IPosition_Connected>(list);
        }

        public PositionSet_ConnectedEdit(List<IPosition_Connected_Edit> list)
        {
            this.list = new List<IPosition_Connected>();
            foreach (IPosition_Connected_Edit p in list)
                this.list.Add(p);
        }

        public void InitToTraverseSet()
        {
            index = -1;
        }

        public bool NextPosition()
        {
            int i = index + 1;
            if (i < list.Count)
            {
                index = i;
                return true;
            }
            else
                return false;
        }

        public int GetNum()
        {
            return list.Count;
        }

        public Array ToArray()
        {
            return list.ToArray();
        }

        public IPosition GetPosition()
        {
            if (index >= 0)
                return list[index];
            else
                return null;
        }

        public IPosition_Connected GetPosition_Connected()
        {
            if (index >= 0)
                return list[index];
            else
                return null;
        }

        #endregion

        // IPositionSet_ConnectedEdit
        #region IPositionSet_ConnectedEdit

        public IPosition_Connected_Edit GetPosition_Connected_Edit()
        {
            if (index >= 0)
                return (IPosition_Connected_Edit)list[index];
            else
                return null;
        }

        public void AddPosition_Connected(IPosition_Connected p)
        {
            list.Add(p);
        }

        public void RemovePosition_Connected(IPosition_Connected p)
        {
            IPositionSet_Connected_AdjacencyEdit adjSet = ((IPosition_Connected_Edit)p).GetAdjacencyPositionSetEdit();
            adjSet.InitToTraverseSet();
            while (adjSet.NextPosition())
            {
                IPosition_Connected_Edit adj = adjSet.GetPosition_Connected_Edit();
                adj.GetAdjacencyPositionSetEdit().RemoveAdjacency(p);
                adjSet.RemoveAdjacency(adj);
            }
            list.Remove(p);
        }

        public void Clear()
        {
            list.Clear();
        }

        #endregion

        // IPositionSetEdit
        #region IPositionSetEdit
        public void AddPosition(IPosition p)
        {
            AddPosition_Connected((IPosition_Connected)p);
        }

        public void RemovePosition(IPosition p)
        {
            RemovePosition_Connected((IPosition_Connected)p);
        }

        #endregion
    }
}
