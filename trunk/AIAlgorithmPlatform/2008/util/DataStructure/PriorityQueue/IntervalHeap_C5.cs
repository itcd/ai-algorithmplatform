using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C5;

namespace DataStructure.PriorityQueue
{
    /// <summary>
    /// A PriorityQueue class based on C5.IntervalHeap<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class IntervalHeap_C5<T> : IPriorityQueue<T>
    {
        C5.IPriorityQueue<T> q = new C5.IntervalHeap<T>();

        #region IPriorityQueue<T> Members

        public bool add(T e)
        {
            return q.Add(e);
        }

        public bool addAll(IPriorityQueue<T> q)
        {
            throw new NotImplementedException();
        }

        public bool addAll(List<T> l)
        {
            throw new NotImplementedException();
        }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public T getFirst()
        {
            return q.FindMin();
        }

        public T removeFirst()
        {
            return q.DeleteMin();
        }

        public bool remove(T o)
        {
            throw new NotImplementedException();
        }

        public bool removeAll(T o)
        {
            throw new NotImplementedException();
        }

        public bool update(T o)
        {
            throw new NotImplementedException();
        }

        public int indexOf(T o)
        {
            throw new NotImplementedException();
        }

        public object[] getQueue()
        {
            T[] ary = q.ToArray();
            object[] a = new object[ary.Length];
            for (int i = 0; i < ary.Length; i++)
                a[i] = ary[i];
            return a;
        }

        public int getSize()
        {
            return q.Count;
        }

        public IComparer<T> getComparator()
        {
            return q.Comparer;
        }

        #endregion
    }
}
