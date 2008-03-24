using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;

namespace M2M.Position.Implement
{
    [Serializable]
    public class PositionEnumerator<T> : IEnumerator<T>
    {
        protected T[] list;
        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PositionEnumerator(T[] list)
        {
            this.list = list;
        }

        #region IEnumerator Members

        public bool MoveNext()
        {
            position++;
            return (position < list.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        #endregion

        #region IEnumerator<T> Members

        T IEnumerator<T>.Current
        {
            get
            {
                try
                {
                    return list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
