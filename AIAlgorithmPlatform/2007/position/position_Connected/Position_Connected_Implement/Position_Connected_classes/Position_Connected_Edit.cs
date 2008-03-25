using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    //地图编辑器用的接口的实现
    [Serializable]
    public class Position_Connected_Edit : IPosition_Connected_Edit
    {
        IPositionSet_Connected_AdjacencyEdit positionSet = new PositionSet_Connected_AdjacencyEdit();
        float x, y;
        Object o = null;

        public Position_Connected_Edit()
        {
            this.x = 0;
            this.y = 0;
        }

        public Position_Connected_Edit(float X, float Y)
        {
            this.x = X;
            this.y = Y;
        }

        //interface IPosition
        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        //interface IPosition_Connected
        public IPositionSet_Connected_Adjacency GetAdjacencyPositionSet()
        {
            return positionSet;
        }

        public Object GetAttachment()
        {
            return o;
        }

        public void SetAttachment(Object o)
        {
            this.o = o;
        }

        //interface IPosition_Connected_Edit
        public IPositionSet_Connected_AdjacencyEdit GetAdjacencyPositionSetEdit()
        {
            return positionSet;
        }

        public void SetX(float value)
        {
            x = value;
        }
        public void SetY(float value)
        {
            y = value;
        }

        //其他
        override public string ToString()
        {
            return "(Position:" + GetX().ToString() + "," + GetY().ToString() + ")";
        }
    }
}
