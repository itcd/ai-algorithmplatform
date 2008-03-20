using System;
using System.Collections.Generic;
using System.Text;
using Position_Connected_Interface;

namespace SearchEngineLibrary
{
    //AStar算法用到的标记
    [Serializable]
    public class Tag
    {
        public IPosition_Connected parent;
        public float f;
        public float g;
        //public int h;
        public bool closed;
        public ushort timeStamp;

        public void Clear()
        {
            this.parent = null;
            this.f = float.MaxValue;
            this.g = float.MaxValue;
            this.closed = false;
            this.timeStamp = 0;
        }

        public Tag()
        {
            Clear();
        }

        public Tag(IPosition_Connected parent, float f, float g, bool closed)
        {
            this.parent = parent;
            this.f = f;
            this.g = g;
            this.closed = closed;
            this.timeStamp = 0;
        }

        public Tag(IPosition_Connected parent, float g)
        {
            this.parent = parent;
            this.f = float.MaxValue;
            this.g = g;
            this.closed = false;
            this.timeStamp = 0;
        }

        override public string ToString()
        {
            return string.Format("{AStarTag:parent:{0},f:{1},g:{2}}", parent, f, g);
        }
    }
}
