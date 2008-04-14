using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.IPosition>;

namespace M2M.Position.Implement
{
    [Serializable]
    public class PositionSet_List : AbstractPositionSet, IPositionSet
    {
        protected List<IPosition> list;

        public PositionSet_List()
        {
            list = new List<IPosition>();
        }

        public PositionSet_List(int size)
        {
            list = new List<IPosition>(size);
        }

        #region ICollection<IPosition> Members

        public override void Add(IPosition item)
        {
            list.Add(item);
        }

        public override void Clear()
        {
            list.Clear();
        }

        public override bool Contains(IPosition item)
        {
            return list.Contains(item);
        }

        public override void CopyTo(IPosition[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public override bool Remove(IPosition item)
        {
            return list.Remove(item);
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<IPosition> Members

        IEnumerator<IPosition> IEnumerable<IPosition>.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return ((IEnumerable<IPosition>)this).GetEnumerator();
        }

        #endregion
    }
}
