using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.Implement
{
    [Serializable]
    public abstract class AbstractPositionSet : IPositionSet
    {
        static protected string errorStr = "A collection that is read-only does not allow the addition, removal, or modification of elements after the collection is created.";

        #region ICollection<IPosition> Members

        public virtual void Add(IPosition item)
        {
            throw new NotImplementedException(errorStr);
        }

        public virtual void Clear()
        {
            throw new NotImplementedException(errorStr);
        }

        public abstract bool Contains(IPosition item);

        public abstract void CopyTo(IPosition[] array, int arrayIndex);

        public virtual bool Remove(IPosition item)
        {
            throw new NotImplementedException(errorStr);
        }

        public abstract int Count{ get; }

        public virtual bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region IEnumerable<IPosition> Members

        IEnumerator<IPosition> IEnumerable<IPosition>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        public virtual IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
