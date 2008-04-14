using System;
using System.Collections.Generic;
using System.Text;

namespace KDNN
{
    public interface IPosition
    {
        double GetX();
        double GetY();
    }

    public class Position_Point : IPosition
    {
        double x;
        double y;

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public void SetX(double x)
        {
            this.x = x;
        }

        public void SetY(double y)
        {
            this.y = y;
        }
    }

    public interface IPositionSet
    {
        void InitToTraverseSet();
        bool NextPosition();
        IPosition GetPosition();
        int GetNum();
        Array ToArray();
    }

    public class PositionSet_ImplementByList : IPositionSet
    {
        protected List<IPosition> PositionList = null;

        public PositionSet_ImplementByList(List<IPosition> positionList)
        {
            PositionList = positionList;
        }

        protected int index = -1;

        public void InitToTraverseSet()
        {
            index = -1;
        }

        public bool NextPosition()
        {
            index++;
            if (index == PositionList.Count)
            {
                return false;
            }
            return true;
        }

        public IPosition GetPosition()
        {
            return PositionList[index];
        }

        /// <summary>
        /// 这个接口不是必须实现的，通过其它接口函数也可以实现其功能，但是如果可以快速得到结果，可以另外提供。
        /// </summary>
        /// <returns></returns>
        public int GetNum()
        {
            return PositionList.Count;
        }

        public Array ToArray()
        {
            return PositionList.ToArray();
        }
    }


    public class PositionSet_Connected_ImplementByList : IPositionSet
    {
        protected List<IPosition_Connected> ConnectedPositionList = null;

        public PositionSet_Connected_ImplementByList(List<IPosition_Connected> positionList)
        {
            ConnectedPositionList = positionList;
        }

        protected int index = -1;

        public void InitToTraverseSet()
        {
            index = -1;
        }

        public bool NextPosition()
        {
            index++;
            if (index == ConnectedPositionList.Count)
            {
                return false;
            }
            return true;
        }

        public IPosition GetPosition()
        {
            return (IPosition)ConnectedPositionList[index];
        }

        /// <summary>
        /// 这个接口不是必须实现的，通过其它接口函数也可以实现其功能，但是如果可以快速得到结果，可以另外提供。
        /// </summary>
        /// <returns></returns>
        public int GetNum()
        {
            return ConnectedPositionList.Count;
        }

        public Array ToArray()
        {
            return ConnectedPositionList.ToArray();
        }
    }

    public interface IPositionAndDistanceSet_Connected : IPositionSet
    {
        double GetDistanceToPosition();
    }

    public class PositionAndDistanceSet_Connected_ImplementByList : PositionSet_Connected_ImplementByList, IPositionAndDistanceSet_Connected
    {
        protected List<double> DistanceToPositionList = null;

        public PositionAndDistanceSet_Connected_ImplementByList(List<IPosition_Connected> positionList, List<double> distanceToPositionList)
            : base(positionList)
        {
            DistanceToPositionList = distanceToPositionList;
        }

        public double GetDistanceToPosition()
        {
            return DistanceToPositionList[index];
        }
    }

    public interface IPosition_Connected : IPosition
    {
        IPositionAndDistanceSet_Connected GetConnectedPositionSet();
    }

    public class Position_Connected_ImplementByList : Position_Point, IPosition_Connected
    {
        PositionAndDistanceSet_Connected_ImplementByList PositionAndDistanceSet_Connected_List = null;

        public Position_Connected_ImplementByList(double x, double y, List<IPosition_Connected> positionList, List<double> distanceToPositionList)
            : base()
        {
            base.SetX(x);
            base.SetY(y);

            PositionAndDistanceSet_Connected_ImplementByList temp = new PositionAndDistanceSet_Connected_ImplementByList(positionList, distanceToPositionList);

            PositionAndDistanceSet_Connected_List = temp;
        }

        public IPositionAndDistanceSet_Connected GetConnectedPositionSet()
        {
            return PositionAndDistanceSet_Connected_List;
        }
    }
}