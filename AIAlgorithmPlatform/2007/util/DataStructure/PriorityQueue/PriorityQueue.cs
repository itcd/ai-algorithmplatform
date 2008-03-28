using System;
using System.Collections.Generic;
using System.Text;

/// 
/// Description:用数组实现了优先队列
/// @date 2007.05.05
/// @author Zhou
/// 
namespace DataStructure.PriorityQueue
{
    /// <summary>
    /// 优先队列的数组实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private const int DEFAULT_INITIAL_CAPACITY = 11;     //默认初始数组容量为11
        private int size = 0;                                 //数组中元素个数
        private object[] queue;                               //用数组实现的二叉堆
        private IComparer<T> comparator;                      //比较器
        
        /// <summary>
        /// 使用默认的初始容量（11）创建一个 PriorityQueue
        /// 并根据其自然顺序来排序其元素（使用 Comparable）
        /// </summary>
        public PriorityQueue()
        {
            this.queue = new object[DEFAULT_INITIAL_CAPACITY];
            this.comparator = null;  
        }
        
        /// <summary>
        /// 使用指定的初始容量创建一个 PriorityQueue
        /// 并根据其自然顺序来排序其元素（使用 Comparable）
        /// </summary>
        /// <param name="initialCapacity"></param>
        public PriorityQueue(int initialCapacity)
        {
            if (initialCapacity < 1)
                throw new ArgumentException();
            this.queue = new object[initialCapacity];
            comparator = null;
        }
        
        /// <summary>
        /// 使用默认的初始容量（11）创建一个 PriorityQueue
        /// 并根据指定的比较器来排序其元素
        /// </summary>
        /// <param name="comparator"></param>
        public PriorityQueue(IComparer<T> comparator)
        {
            if (comparator == null)
                throw new ArgumentException();
            this.queue = new object[DEFAULT_INITIAL_CAPACITY];
            this.comparator = comparator;
        }
        
        /// <summary>
        /// 使用指定的初始容量创建一个 PriorityQueue
        /// 并根据指定的比较器来排序其元素
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="comparator"></param>
        public PriorityQueue(int initialCapacity, IComparer<T> comparator) 
        {
            if (initialCapacity < 1)
                throw new ArgumentException();
            this.queue = new object[initialCapacity];
            this.comparator = comparator;
        }
        
        /// <summary>
        /// 创建包含指定优先队列中元素的 PriorityQueue
        /// 根据指定优先队列的比较器来排序元素
        /// </summary>
        /// <param name="q"></param>
        public PriorityQueue(IPriorityQueue<T> q)
        {
            this.queue = new object[DEFAULT_INITIAL_CAPACITY];
            this.comparator = q.getComparator();
            object[] tempQueue = new object[q.getQueue().Length];
            q.getQueue().CopyTo(tempQueue,0);
            queue = tempQueue;
            size = q.getSize();
        }

        /// <summary>
        /// 向队列中添加指定的元素
        /// 如果比较器为空，采用自然排序
        /// 否则根据比较器来排序元素
        /// </summary>
        /// <param name="e">T e</param>
        /// <returns>bool</returns>
        public bool add(T e)
        {
            if (e == null)
            {
                throw new ArgumentNullException();
            }
            //自动增长数组大小
            if (size >= queue.Length)
                grow();

            if (size == 0)
            {
                queue[0] = e;
                size++;
                return true;
            }

            if (comparator != null)
                addUsingComparator(e);                //根据比较器来排序
            else
                addUsingComparable(e);                //比较器为空，自然排序

            return true;        
        }
        
        /// <summary>
        /// 根据比较器排序的函数，被add()调用
        /// </summary>
        /// <param name="e">T e</param>
        private void addUsingComparator(T e)
        {
                queue[size++] = e;                     //将元素添加到数组尾
                int i = size - 1;
                //如果该节点比它的父节点小，则与父节点交换位置，循环至到达根节点
                while (  (i > 0) && ( comparator.Compare( (T)queue[i], (T)queue[(i-1)/2] ) < 0 )  )
                {
                    T temp = (T)queue[i];
                    queue[i] = queue[(i - 1) / 2];
                    queue[(i - 1) / 2] = temp;
                    i = (i - 1) / 2;
                }
        }
        
        /// <summary>
        /// 采用自然排序的函数，被add()调用
        /// </summary>
        /// <param name="e"></param>
        private void addUsingComparable(T e)
        {
                queue[size++] = e;
                int i = size - 1;
                try
                {
                    while ((i > 0) && (((IComparable<T>)queue[i]).CompareTo((T)queue[(i - 1) / 2]) < 0))
                    {
                        T temp = (T)queue[i];
                        queue[i] = queue[(i - 1) / 2];
                        queue[(i - 1) / 2] = temp;
                        i = (i - 1) / 2;
                    }
                }
                catch (InvalidCastException){
                    //Console.WriteLine("不能使用自然排序，必须自定义Comparator\n");
                    removeFirst();
                    size--;
                }                
        }

        /// <summary>
        /// 将指定队列中的所有元素都添加到此队列中
        /// </summary>
        /// <param name="q">IPriorityQueue<T> q</param>
        /// <returns>bool</returns>
        public bool addAll(IPriorityQueue<T> q)
        {
            if (q == null)
                throw new NullReferenceException();
            object[] que = q.getQueue();
            bool modified = false;
            for (int i = 0; i < q.getSize(); i++)
            {
                if (add((T)que[i]))
                    modified = true;
            }
            return modified;
        }

        /// <summary>
        /// 将指定数组中的所有元素添加到此队列中
        /// Ark(2008-3-28)将参数类型由List<T>改为IEnumerable<T>
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public bool addAll(IEnumerable<T> q)
        {
            //if (l == null)
            //    throw new NullReferenceException();
            bool modified = false;
            //for(int i = 0; i < l.Count; i++)
            //{
            //    if ( add(l[i]) )
            //        modified = true;
            //}
            foreach (T t in q)
            {
                modified = modified || add(t);
            }
            return modified;
        }

        /// <summary>
        /// 从优先级队列中移除所有元素
        /// </summary>
        public void clear()
        {
            for (int i = 0; i < size; i++)
            {
                queue[i] = null;
            }
            size = 0;
        }

        /// <summary>
        /// 获取但不移除此队列的头
        /// </summary>
        /// <returns>T</returns>
        public T getFirst()
        {
            if (size == 0)
            {
                //Console.WriteLine("数组中元素个数为0");
                return default(T);
            }
            T result;
            result = (T)queue[0];
            return result;
        }
        
        /// <summary>
        /// 获取并移除此队列的头
        /// 移除后重新堆化
        /// </summary>
        /// <returns>T</returns>
        public T removeFirst()
        {
            if (comparator != null)
                return removeUsingComparator();       //使用比较器重新堆化
            else
                return removeUsingComparable();       //使用自然排序重新堆化
        }

        /// <summary>
        /// 被removeFirst()调用
        /// </summary>
        /// <returns></returns>
        private T removeUsingComparable()
        {
            T result;
            int i = 0;
            int j = 0;
            if (size == 0)
            {
                //Console.WriteLine("There is no element in the queue");
                return default(T);
            }
            else
            {
                result = (T)queue[0];
                queue[0] = queue[size - 1];
                queue[size - 1] = null;
                size--;
                while ((2 * i + 1) < size)
                {
                    if ((2 * i + 1 == size - 1) || (  ((IComparable<T>)queue[2 * i + 1]).CompareTo( (T)queue[2 * i + 2] ) ) < 0  )
                    {
                        j = 2 * i + 1;
                    }
                    else
                    {
                        j = 2 * i + 2;
                    }

                    if (  ( ((IComparable<T>)queue[j]).CompareTo((T)queue[i]) ) < 0  )
                    {
                        T temp = (T)queue[i];
                        queue[i] = queue[j];
                        queue[j] = temp;
                        i = j;
                    }
                    else
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 被removeFirst()调用
        /// </summary>
        /// <returns></returns>
        private T removeUsingComparator()
        {
            T result;
            int i = 0;

            if (size == 0)
            {
                //Console.WriteLine("There is no element in the queue");
                return default(T);
            }

            result = (T)queue[0];
            queue[0] = queue[size - 1];
            queue[size - 1] = null;
            size--;

            //重新堆化
            shiftDownUsingComparator(i);

            return result;
        }

        /// <summary>
        /// 删除队列中指定的元素o,删除成功返回true,该元素不存在返回false
        /// 删除全部元素o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool removeAll(T o)
        {
            if (!remove(o))
                return false;
            while(remove(o));
            return true;
        }

        /// <summary>
        /// 删除队列中指定的元素o
        /// 只删除第一个元素o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool remove(T o)
        {
            int i = indexOf(o);

            //如果不存在该元素则返回false
            if (i == -1)
                return false;

            if (comparator != null)
            {
                removeHelperUsingComparator(i, o);                   //使用自定义比较器
            }
            else
            {
                removeHelperUsingComparable(i, o);                   //不使用自定义比较器
            }
            return true;
        }

        /// <summary>
        /// 如果比较器comparator不为空，由remove()调用
        /// 用于删除位置i中的元素
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ot"></param>
        /// <returns></returns>
        private void removeHelperUsingComparator(int i, T ot)
        {
            //如果该元素在数组尾部
            if (i == size - 1)
            {
                queue[--size] = null;
            }
            else
            {
                T moved = (T)queue[size - 1];                      //将最后一个元素存入moved当中
                queue[i] = moved;                                  //将最后一个元素插入到i位置当中                        
                queue[size - 1] = null;                            //删除最后一个元素
                size--;                                            //数组大小减一

                shiftDownUsingComparator(i);                       //将元素下移到适当位置

                //如果元素还在i位置，则将该元素往上移到适当的位置中
                //if (comparator.Compare((T)queue[i], moved) == 0)
                if (((T)queue[i]).GetHashCode() == moved.GetHashCode())
                    shiftUpUsingComparator(i, moved);
            }          
        }

        /// <summary>
        /// 根据自定义比较器，将位置i的元素下移到队列中适当的位置
        /// </summary>
        /// <param name="i">位置i</param>
        private void shiftDownUsingComparator(int i)
        {
            int j = 0;

            //重新堆化,将被插入的元素往下移到适当的位置中
            while ((2 * i + 1) < size)
            {
                if ((2 * i + 1 == size - 1) || (comparator.Compare((T)queue[2 * i + 1], (T)queue[2 * i + 2]) < 0))
                {
                    j = 2 * i + 1;
                }
                else
                {
                    j = 2 * i + 2;
                }

                if ((comparator.Compare((T)queue[j], (T)queue[i]) < 0))
                {
                    T temp = (T)queue[i];
                    queue[i] = queue[j];
                    queue[j] = temp;
                    i = j;
                }
                else
                    return;
            }
        }

        /// <summary>
        /// 根据比较器，将位置i的元素上移到队列中适当的位置
        /// </summary>
        /// <param name="i">位置i</param>
        /// <param name="x">原来队列末尾的元素</param>
        private void shiftUpUsingComparator(int i, T x)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                T e = (T)queue[parent];
                if (comparator.Compare(x, (T)e) >= 0)
                    break;
                queue[i] = e;
                i = parent;
            }
            queue[i] = x;
        }

         /// <summary>
        /// 如果比器comparator为空，由remove()调用
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ot"></param>
        /// <returns></returns>
        private void removeHelperUsingComparable(int i, T ot)
        {
            if (i == size - 1)
            {
                queue[--size] = null;
            }
            else 
            {
                T moved = (T)queue[size - 1];
                queue[i] = moved;
                queue[size - 1] = null;
                size--;

                shiftDownUsingComparable(i);

                if (((IComparable<T>)queue[i]).CompareTo(moved) == 0)
                    shiftUpUsingComparable(i, moved);
            }         
        }

        /// <summary>
        /// 根据自然排序，将位置i的元素下移到队列中适当的位置
        /// </summary>
        /// <param name="i">位置i</param>
        private void shiftDownUsingComparable(int i)
        {
            int j = 0;

            //重新堆化
            while ((2 * i + 1) < size)
            {
                if ((2 * i + 1 == size - 1) || (((IComparable<T>)queue[2 * i + 1]).CompareTo((T)queue[2 * i + 2])) < 0)
                {
                    j = 2 * i + 1;
                }
                else
                {
                    j = 2 * i + 2;
                }

                if (((IComparable<T>)queue[j]).CompareTo((T)queue[i]) < 0)
                {
                    T temp = (T)queue[i];
                    queue[i] = queue[j];
                    queue[j] = temp;
                    i = j;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 根据自然排序，将位置i的元素上移到队列中适当的位置
        /// </summary>
        /// <param name="i">位置i</param>
        /// <param name="x">原来队列末尾的元素</param>
        private void shiftUpUsingComparable(int i, T x)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                T e = (T)queue[parent];
                if (((IComparable<T>)x).CompareTo(e) >= 0)
                    break;
                queue[i] = e;
                i = parent;
            }
            queue[i] = x;
        }

        public bool update(T e)
        {
            int i = indexOf(e);
            if (i == -1)
            {
                //Console.WriteLine("No this element");
                return false;
            }
            if (comparator == null)
            {
                //Console.WriteLine("You must provide a comparator");
                return false;
            }
            T temp = (T)queue[i];
            shiftDownUsingComparator(i);                       //将元素下移到适当位置

            //如果元素还在i位置，则将该元素往上移到适当的位置中
            if (comparator.Compare((T)queue[i], temp) == 0)
                shiftUpUsingComparator(i, temp);
            return true;
        }

        /// <summary>
        /// 
        /// 返回指定元素o在数组中的位置
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int indexOf(T o)
        {
            return Array.IndexOf(queue, o);
        }
                
        /// <summary>
        /// 数组自动增长函数，当数组满时被add()调用
        /// </summary>
        private void grow()
        {
            int oldCapacity = queue.Length;
            //根据数组原来大小分配新容量
            int newCapacity = ( oldCapacity < 64 ? (oldCapacity + 1) * 2 : (oldCapacity / 2) * 3 );
            Array.Resize(ref queue, newCapacity);
        }
        
        /// <summary>
        /// 按顺序输出数组的所有元素，测试使用
        /// </summary>
        public void output()
        {
            for (int i = 0; i < size; i++)
                Console.WriteLine("The " + i + " element is " + queue[i]);
            Console.WriteLine("\nThere are " + size + " elements");
            Console.WriteLine("Queue total length is: " + queue.Length);
        }
        
        //返回数组
        public object[] getQueue()
        {
            return queue;
        }
        
        //返回数组中元素个数
        public int getSize()
        {
            return size;
        }
        
        //返回比较器
        public IComparer<T> getComparator()
        {
            return comparator;
        }

    }


}
