using System;

using System.Threading;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Diagnostics;

namespace Position_Interface
{
    public interface IPositionSet
    {
        void InitToTraverseSet();
        bool NextPosition();
        IPosition GetPosition();
        int GetNum();
        Array ToArray();
    }

    //public class PositionSet_ImplementByList : IPositionSet
    //{
    //    protected IList<IPosition> PositionList = null;

    //    protected PositionSet_ImplementByList()
    //    { }

    //    public PositionSet_ImplementByList(IList<IPosition> positionList)
    //    {
    //        PositionList = positionList;
    //    }

    //    protected int index = -1;

    //    public void InitToTraverseSet()
    //    {
    //        index = -1;
    //    }

    //    public bool NextPosition()
    //    {
    //        index++;
    //        if (index >= PositionList.Count)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public IPosition GetPosition()
    //    {
    //        return PositionList[index];
    //    }

    //    /// <summary>
    //    /// 这个接口不是必须实现的，通过其它接口函数也可以实现其功能，但是如果可以快速得到结果，可以另外提供。
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetNum()
    //    {
    //        return PositionList.Count;
    //    }

    //    public Array ToArray()
    //    {
    //        if (PositionList is List<IPosition>)
    //        {
    //            return ((List<IPosition>)PositionList).ToArray();
    //        }
    //        else
    //        {
    //            throw new Exception("positionList is not List<IPosition>");
    //        }
    //    }
    //}

    //public class PositionSet_Connected_ImplementByList : IPositionSet
    //{
    //    protected List<IPosition_Connected> ConnectedPositionList = null;

    //    public PositionSet_Connected_ImplementByList(List<IPosition_Connected> positionList)
    //    {
    //        ConnectedPositionList = positionList;
    //    }

    //    protected int index = -1;

    //    public void InitToTraverseSet()
    //    {
    //        index = -1;
    //    }

    //    public bool NextPosition()
    //    {
    //        index++;
    //        if (index == ConnectedPositionList.Count)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public IPosition GetPosition()
    //    {
    //        return ConnectedPositionList[index];
    //    }

    //    /// <summary>
    //    /// 这个接口不是必须实现的，通过其它接口函数也可以实现其功能，但是如果可以快速得到结果，可以另外提供。
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetNum()
    //    {
    //        return ConnectedPositionList.Count;
    //    }

    //    public Array ToArray()
    //    {
    //        return ConnectedPositionList.ToArray();
    //    }
    //}

    //public interface IPositionAndDistanceSet_Connected : IPositionSet
    //{
    //    float GetDistanceToPosition();
    //}

    //public class PositionAndDistanceSet_Connected_ImplementByList : PositionSet_Connected_ImplementByList, IPositionAndDistanceSet_Connected
    //{
    //    protected List<float> DistanceToPositionList = null;

    //    public PositionAndDistanceSet_Connected_ImplementByList(List<IPosition_Connected> positionList, List<float> distanceToPositionList)
    //        : base(positionList)
    //    {
    //        DistanceToPositionList = distanceToPositionList;
    //    }

    //    public float GetDistanceToPosition()
    //    {
    //        return DistanceToPositionList[index];
    //    }
    //}

    //public interface IPosition_Connected : IPosition
    //{
    //    IPositionAndDistanceSet_Connected GetConnectedPositionSet();
    //}

    //public class Position_Connected_ImplementByList : Position_Point, IPosition_Connected
    //{
    //    PositionAndDistanceSet_Connected_ImplementByList PositionAndDistanceSet_Connected_List = null;

    //    public Position_Connected_ImplementByList(float x, float y, List<IPosition_Connected> positionList, List<float> distanceToPositionList)
    //        : base()
    //    {
    //        base.SetX(x);
    //        base.SetY(y);

    //        PositionAndDistanceSet_Connected_ImplementByList temp = new PositionAndDistanceSet_Connected_ImplementByList(positionList, distanceToPositionList);

    //        PositionAndDistanceSet_Connected_List = temp;
    //    }

    //    public IPositionAndDistanceSet_Connected GetConnectedPositionSet()
    //    {
    //        return PositionAndDistanceSet_Connected_List;
    //    }
    //}
}