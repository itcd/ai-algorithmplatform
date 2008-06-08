using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Implement
{
    /// <summary>
    /// ������԰�����ʵ����ICollection<IPosition>�ӿڵ�����ת����IPositionSetEdit���ͣ�����list�ȷ����ࡣ
    /// </summary>
    public class PositionSetEdit_ImplementByICollectionTemplate : PositionSet_ImplementByIEnumerableTemplate, IPositionSetEdit
    {
        protected ICollection<IPosition> positionSetEdit;

        /// <summary>
        /// �๹�캯��
        /// </summary>
        /// <param name="positionSetEdit">�����ICollection<IPosition>��ÿ��Ԫ�������õı�����IPosition_Edit</param>
        public PositionSetEdit_ImplementByICollectionTemplate(ICollection<IPosition> positionSetEdit)
            : base(positionSetEdit)
        {
            this.positionSetEdit = positionSetEdit;
        }

        public PositionSetEdit_ImplementByICollectionTemplate()
            : base()
        {
            this.positionSetEdit = new List<IPosition>();
            this.positionSet = positionSetEdit;
        }

        public void AddPosition(IPosition p)
        {
            positionSetEdit.Add(p);
        }

        public void RemovePosition(IPosition p)
        {
            positionSetEdit.Remove(p);
        }

        public void Clear()
        {
            positionSetEdit.Clear();
        }
    }

    public class PositionSet_Cloned : APositionSet_PositionSet, IPositionSetEdit
    {
        IPositionSetEdit positionSetEdit;

        public PositionSet_Cloned(IPositionSet positionSet)
        {
            List<IPosition> positionList = new List<IPosition>();

            positionSet.InitToTraverseSet();

            while (positionSet.NextPosition())
            {
                positionList.Add(new Position_Point(positionSet.GetPosition().GetX(), positionSet.GetPosition().GetY()));
            }

            this.positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate(positionList);
            this.positionSet = positionSetEdit;
        }

        public void AddPosition(IPosition p)
        {
            positionSetEdit.AddPosition(p);
        }

        public void RemovePosition(IPosition p)
        {
            positionSetEdit.RemovePosition(p);
        }

        public void Clear()
        {
            positionSetEdit.Clear();
        }
    }

    /// <summary>
    /// �Ѷ��positionSet�ϲ���һ��positionSet���ϲ��������ÿ���㼯
    /// </summary>
    public class PositionSetEditSet : PositionSetSet, IPositionSetEdit
    {
        PositionSetEdit_ImplementByICollectionTemplate positionSetEdit = new PositionSetEdit_ImplementByICollectionTemplate();

        public PositionSetEditSet()
        {
            positionSetList.Add(positionSetEdit);
        }

        public void AddPosition(IPosition p)
        {
            positionSetEdit.AddPosition(p);
        }

        public void RemovePosition(IPosition p)
        {
            foreach (IPositionSet positionSet in positionSetList)
            {
                if (positionSet is IPositionSetEdit)
                {
                    ((IPositionSetEdit)positionSet).RemovePosition(p);
                }
            }
        }

        public void Clear()
        {
            positionSetList.Clear();
            positionSetEdit.Clear();
            positionSetList.Add(positionSetEdit);
        }

        public void AddPositionSet(IPositionSetEdit positionSetEdit)
        {
            positionSetList.Add(positionSetEdit);
        }

        public override void AddPositionSet(IPositionSet positionSet)
        {
            throw new Exception("PositionSetEditSet Can not Add IPositionSet");
        }


    }

    /// <summary>
    /// ���ó�Ա����positionSetEdit���Ѽ̳��ڸ��������װ���positionSetEdit
    /// </summary>
    public abstract class APositionSetEdit_PositionSetEdit : APositionSet<IPosition>
    {
        protected IPositionSetEdit positionSetEdit;

        override public void InitToTraverseSet()
        {
            positionSetEdit.InitToTraverseSet();
        }

        override public bool NextPosition()
        {
            return positionSetEdit.NextPosition();
        }

        override public IPosition GetPosition()
        {
            return positionSetEdit.GetPosition();
        }

        public virtual void AddPosition(IPosition p)
        {
            positionSetEdit.AddPosition(p);
        }

        public virtual void RemovePosition(IPosition p)
        {
            positionSetEdit.RemovePosition(p);
        }

        public virtual void Clear()
        {
            positionSetEdit.Clear();
        }
    }
}
