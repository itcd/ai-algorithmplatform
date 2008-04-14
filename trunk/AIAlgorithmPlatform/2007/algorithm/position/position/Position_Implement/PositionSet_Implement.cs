using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Position_Interface;

namespace Position_Implement
{
    /// <summary>
    /// 利用接口函数，实现了GetNum()和ToArray()函数的一般实现，不保证性能上最优。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class APositionSet<T>
    {
        public abstract void InitToTraverseSet();
        public abstract bool NextPosition();
        public abstract T GetPosition();

        public virtual int GetNum()
        {
            int n = 0;
            InitToTraverseSet();
            while (NextPosition())
            {
                n++;
            }
            return n;
        }

        public virtual Array ToArray()
        {
            T[] aryPositionSet = new T[GetNum()];

            int n = 0;
            InitToTraverseSet();
            while (NextPosition())
            {
                aryPositionSet[n] = GetPosition();
                n++;
            }

            return aryPositionSet;
        }
    }


    /// <summary>
    /// 该类可以把所有实现了IEnumerable<IPosition>接口的类型转换成IPositionSet类型，包括list等范型类。
    /// </summary>
    [Serializable]
    public class PositionSet_ImplementByIEnumerableTemplate : APositionSet<IPosition>, IPositionSet
    {
        protected IEnumerable<IPosition> positionSet;

        protected IEnumerator<IPosition> enumerator;

        protected PositionSet_ImplementByIEnumerableTemplate()
        { }

        public PositionSet_ImplementByIEnumerableTemplate(IEnumerable<IPosition> positionSet)
        {
            this.positionSet = positionSet;
        }

        override public void InitToTraverseSet()
        {
            enumerator = positionSet.GetEnumerator();
            enumerator.Reset();
        }

        override public bool NextPosition()
        {
            return enumerator.MoveNext();
        }

        override public IPosition GetPosition()
        {
            return enumerator.Current;
        }

        /// <summary>
        /// 如果传入的IEnumerable是ICollection，则快速返回ICollection的元素个数。
        /// </summary>
        /// <returns></returns>
        override public int GetNum()
        {
            if (enumerator is ICollection<IPosition>)
            {
                return ((ICollection<IPosition>)enumerator).Count;
            }
            else
            {
                return base.GetNum();
            }
        }

        /// <summary>
        /// 如果传入的IEnumerable是IList，则快速返回IList。
        /// </summary>
        /// <returns></returns>
        public IList<IPosition> ToIlist()
        {
            if (positionSet is IList<IPosition>)
            {
                return (IList<IPosition>)positionSet;
            }
            else
            {
                throw new Exception("positionList is not List<IPosition>");
            }
        }
    }


    /// <summary>
    /// 该类可以把所有实现了IEnumerable接口的类型转换成IPositionSet类型，包括array等类型。
    /// </summary>
    [Serializable]
    public class PositionSet_ImplementByIEnumerable : APositionSet<IPosition>, IPositionSet
    {
        protected IEnumerable positionSet;

        protected IEnumerator enumerator;

        protected PositionSet_ImplementByIEnumerable()
        { }

        public PositionSet_ImplementByIEnumerable(IEnumerable positionSet)
        {
            this.positionSet = positionSet;
        }

        override public void InitToTraverseSet()
        {
            enumerator = positionSet.GetEnumerator();
            enumerator.Reset();
        }

        override public bool NextPosition()
        {
            return enumerator.MoveNext();
        }

        override public IPosition GetPosition()
        {
            return (IPosition)enumerator.Current;
        }

        /// <summary>
        /// 如果传入的IEnumerable是ICollection，则快速返回ICollection的元素个数。
        /// </summary>
        /// <returns></returns>
        override public int GetNum()
        {
            if (enumerator is ICollection)
            {
                return ((ICollection)enumerator).Count;
            }
            else
            {
                return base.GetNum();
            }
        }

        /// <summary>
        /// 如果传入的IEnumerable是Array，则快速返回Array本身。
        /// </summary>
        /// <returns></returns>
        override public Array ToArray()
        {
            if (positionSet is Array)
            {
                return (Array)positionSet;
            }
            else
            {
                return base.ToArray();
            }
        }
    }

    /// <summary>
    /// 把多个positionSet合并成一个positionSet，合并无须遍历每个点集
    /// </summary>
    public class PositionSetSet : APositionSet<IPosition>, IPositionSet
    {
        protected List<IPositionSet> positionSetList = new List<IPositionSet>();

        int currentSequence = 0;

        public void Clear()
        {
            positionSetList.Clear();
        }

        public virtual void AddPositionSet(IPositionSet positionSet)
        {
            positionSetList.Add(positionSet);
        }

        override public void InitToTraverseSet()
        {
            if (positionSetList.Count > 0)
            {
                currentSequence = 0;
                positionSetList[0].InitToTraverseSet();
            }
        }

        override public bool NextPosition()
        {
            if (positionSetList.Count > 0)
            {
                if (positionSetList[currentSequence].NextPosition())
                {
                    return true;
                }
                else
                {
                    while (currentSequence < positionSetList.Count - 1)
                    {
                        currentSequence++;

                        positionSetList[currentSequence].InitToTraverseSet();

                        if (positionSetList[currentSequence].NextPosition())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        override public IPosition GetPosition()
        {
            return (IPosition)positionSetList[currentSequence].GetPosition();
        }
    }


    public class PositionSet_Transformed : APositionSet<IPosition>, IPositionSet
    {
        IPositionSet positionSet;
        Transform transform;
        bool isPositionEdit = false;

        public PositionSet_Transformed(IPositionSet positionSet, Transform transform)
        {
            this.positionSet = positionSet;
            this.transform = transform;
            positionSet.InitToTraverseSet();
            positionSet.NextPosition();
            if (positionSet.GetPosition() is IPosition_Edit)
            {
                isPositionEdit = true;
            }
        }

        override public void InitToTraverseSet()
        {
            positionSet.InitToTraverseSet();
        }

        override public bool NextPosition()
        {
            return positionSet.NextPosition();
        }

        override public IPosition GetPosition()
        {
            if (isPositionEdit)
            {
                return new Position_Edit_Transformed((IPosition_Edit)positionSet.GetPosition(), transform);
            }
            else
            {
                return new Position_Transformed(positionSet.GetPosition(), transform);
            }
        }
    }

    /// <summary>
    /// 利用成员变量positionSet，把继承于该类的子类装扮成positionSet
    /// </summary>
    public abstract class APositionSet_PositionSet : APositionSet<IPosition>
    {
        protected IPositionSet positionSet;

        override public void InitToTraverseSet()
        {
            positionSet.InitToTraverseSet();
        }

        override public bool NextPosition()
        {
            return positionSet.NextPosition();
        }

        override public IPosition GetPosition()
        {
            return positionSet.GetPosition();
        }
    }
}
