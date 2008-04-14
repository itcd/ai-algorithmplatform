using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Position_Interface;

namespace Position_Implement
{
    /// <summary>
    /// ���ýӿں�����ʵ����GetNum()��ToArray()������һ��ʵ�֣�����֤���������š�
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
    /// ������԰�����ʵ����IEnumerable<IPosition>�ӿڵ�����ת����IPositionSet���ͣ�����list�ȷ����ࡣ
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
        /// ��������IEnumerable��ICollection������ٷ���ICollection��Ԫ�ظ�����
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
        /// ��������IEnumerable��IList������ٷ���IList��
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
    /// ������԰�����ʵ����IEnumerable�ӿڵ�����ת����IPositionSet���ͣ�����array�����͡�
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
        /// ��������IEnumerable��ICollection������ٷ���ICollection��Ԫ�ظ�����
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
        /// ��������IEnumerable��Array������ٷ���Array����
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
    /// �Ѷ��positionSet�ϲ���һ��positionSet���ϲ��������ÿ���㼯
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
    /// ���ó�Ա����positionSet���Ѽ̳��ڸ��������װ���positionSet
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
