using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    //基于List实现的优先队列
    [Serializable]
    public class Priority_List<T> : IPriorityQueue<T>
    {
        List<T> list;
        IComparer<T> com;

        public Priority_List(IComparer<T> comparator)
        {
            com = comparator;
            list = new List<T>();
        }

        public Priority_List(int initialCapacity, IComparer<T> comparator)
        {
            if (initialCapacity < 1)
                throw new ArgumentException();
            com = comparator;
            list = new List<T>(initialCapacity);
        }

        public Priority_List(List<T> list, IComparer<T> comparer)
        {
            com = comparer;
            this.list = new List<T>(list);        
        }

        public Priority_List(IPriorityQueue<T> q)
        {
            com = q.getComparator();
            int len = q.getSize();
            list = new List<T>(len);
            object[] a = q.getQueue();
            if (a != null)
                for (int i = 0; i < len; i++)
                        list.Add((T)a[i]);
        }

        public bool addAll(List<T> l)
        {
            for (int i = 0; i < l.Count; i++)
                add(l[i]);
            return true;
        }

        public bool addAll(IPriorityQueue<T> q)
        {
            object[] ary = q.getQueue();
            for (int i = 0; i < ary.Length; i++)
                add((T)ary[i]);
            return true;
        }

        public bool add(T value)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (com.Compare(value, list[i]) <= 0)
                    {
                        list.Insert(i, value);
                        return true;
                    }
                }
            }
            list.Add(value);
            return true;
        }

        public T getFirst()
        {
            if (list.Count > 0)
                return list[0];
            else
                return default(T);
        }

        public T removeFirst()
        {
            if (list.Count > 0)
            {
                T value = list[0];
                list.Remove(value);
                return value;
            }
            else
                return default(T);
        }

        public bool remove(T value)
        {
            return list.Remove(value);
        }

        public bool removeAll(T value)
        {
            bool result = true;
            while(list.IndexOf(value) != -1)
            {
                if (!list.Remove(value))
                    result = false;
            }
            return result;
        }

        public int indexOf(T value)
        {
            return list.IndexOf(value);
        }

        public object[] getQueue()
        {
            object[] a = new object[list.Count];
            for (int i = 0; i < list.Count; i++)
                a[i] = list[i];
            return a;
        }

        public void sort()
        {
            list.Sort(com);
        }

        public void clear()
        {
            list.Clear();
        }

        public int getSize()
        {
            return list.Count;
        }

        public List<T> getList()
        {
            return list;
        }

        public T get(int index)
        {
            if (index >= 0 && index < list.Count)
                return list[index];
            else
                return default(T);
        }

        public IComparer<T> getComparator()
        {
            return com;
        }

        public bool update(T e)
        {
            if (remove(e))
            {
                add(e);
                return true;
            }
            else
                return false;
        }
    }
}
