using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using C5;

namespace DataStructure.PriorityQueue
{
    /// <summary>
    /// A PriorityQueue class based on C5.IntervalHeap<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class IntervalHeap_C5<T> : IPriorityQueue<T>, System.Collections.Generic.ICollection<T>
    {
        C5.IDictionary<T, IPriorityQueueHandle<T>> dict = new C5.HashDictionary<T, IPriorityQueueHandle<T>>();
        C5.IPriorityQueue<T> queue;
        IComparer<T> com = null;
        int size = -1;

        /// <summary>
        /// 使用默认的初始容量（11）创建一个 PriorityQueue
        /// 并根据其自然顺序来排序其元素（使用 Comparable）
        /// </summary>
        public IntervalHeap_C5()
        {
            queue = new C5.IntervalHeap<T>();
        }

        /// <summary>
        /// 使用指定的初始容量创建一个 PriorityQueue
        /// 并根据其自然顺序来排序其元素（使用 Comparable）
        /// </summary>
        /// <param name="initialCapacity"></param>
        public IntervalHeap_C5(int initialCapacity)
        {
            size = initialCapacity;
            queue = new C5.IntervalHeap<T>(size);
        }

        /// <summary>
        /// 使用默认的初始容量（11）创建一个 PriorityQueue
        /// 并根据指定的比较器来排序其元素
        /// </summary>
        /// <param name="comparator"></param>
        public IntervalHeap_C5(IComparer<T> comparator)
        {
            com = comparator;
            queue = new C5.IntervalHeap<T>(com);
        }

        /// <summary>
        /// 使用指定的初始容量创建一个 PriorityQueue
        /// 并根据指定的比较器来排序其元素
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="comparator"></param>
        public IntervalHeap_C5(int initialCapacity, IComparer<T> comparator)
        {
            com = comparator;
            size = initialCapacity;
            queue = new C5.IntervalHeap<T>(size, com);
        }

        /// <summary>
        /// 创建包含指定优先队列中元素的 PriorityQueue
        /// 根据指定优先队列的比较器来排序元素
        /// </summary>
        /// <param name="q"></param>
        public IntervalHeap_C5(IPriorityQueue<T> q)
        {
            com = q.getComparator();
            size = q.getSize();
            queue = new C5.IntervalHeap<T>(size, com);
        }


        #region IPriorityQueue<T> Members

        public bool add(T e)
        {
            IPriorityQueueHandle<T> h = null;
            bool r = queue.Add(ref h, e);
            dict.Add(e, h);
            return r;
        }

        public bool addAll(IPriorityQueue<T> q)
        {
            object[] a = q.getQueue();
            bool r = true;
            foreach (object item in a)
            {
                r = r && add((T)item);
            }
            return r;
        }

        public bool addAll(IEnumerable<T> q)
        {
            bool r = true;
            foreach (T item in q)
            {
                r = r && add((T)item);
            }
            return r;
        }

        public void clear()
        {
            dict.Clear();
            if (size > 0)
                if (com != null)
                    queue = new C5.IntervalHeap<T>(size, com);
                else
                    queue = new C5.IntervalHeap<T>(size);
            else
                if (com != null)
                    queue = new C5.IntervalHeap<T>(com);
                else
                    queue = new C5.IntervalHeap<T>();
        }

        public T getFirst()
        {
            return queue.FindMin();
        }

        public T removeFirst()
        {
            return queue.DeleteMin();
        }

        public bool remove(T o)
        {
            return queue.Delete(dict[o]) != null;
        }

        public bool removeAll(T o)
        {
            clear();
            return true;
        }

        public bool update(T o)
        {
            return queue.Replace(dict[o], o) != null;
        }

        public int indexOf(T o)
        {
            int i = 0;
            foreach (T t in queue)
            {
                if (t.Equals(o))
                    return i;
                i++;
            }
            return i;
        }

        public object[] getQueue()
        {
            T[] ary = queue.ToArray();
            object[] a = new object[ary.Length];
            for (int i = 0; i < ary.Length; i++)
                a[i] = ary[i];
            return a;
        }

        public int getSize()
        {
            return queue.Count;
        }

        public IComparer<T> getComparator()
        {
            return queue.Comparer;
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            add(item);
        }

        public void Clear()
        {
            clear();
        }

        public bool Contains(T item)
        {
            return dict[item] != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] ary = queue.ToArray();
            foreach (T t in ary)
            {
                array[arrayIndex++] = t;
            }
        }

        public bool Remove(T item)
        {
            return remove(item);
        }

        public int Count
        {
            get { return queue.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion
    }
}
