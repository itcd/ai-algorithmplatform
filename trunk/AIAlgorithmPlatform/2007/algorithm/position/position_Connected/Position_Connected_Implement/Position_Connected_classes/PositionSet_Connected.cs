using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    //寻径算法用的接口的实现
    [Serializable]
    public class PositionSet_Connected : IPositionSet_Connected
    {
        List<IPosition_Connected> list;
        int index = -1;

        //IPositionSet_Connected
        #region IPositionSet_Connected

        public PositionSet_Connected()
        {
            list = new List<IPosition_Connected>();
        }

        public PositionSet_Connected(List<IPosition_Connected> list)
        {
            this.list = new List<IPosition_Connected>(list);
        }

        public PositionSet_Connected(List<IPosition_Connected_Edit> list)
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
    }
}
