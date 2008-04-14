using System;
using System.Collections.Generic;
using System.Text;

/// 
/// Description:������ʵ�������ȶ���
/// @date 2007.05.05
/// @author Zhou
/// 
namespace M2M.Util.DataStructure.PriorityQueue
{
    /// <summary>
    /// ���ȶ��е�����ʵ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private const int DEFAULT_INITIAL_CAPACITY = 11;     //Ĭ�ϳ�ʼ��������Ϊ11
        private int size = 0;                                 //������Ԫ�ظ���
        private object[] queue;                               //������ʵ�ֵĶ����
        private IComparer<T> comparator;                      //�Ƚ���
        
        /// <summary>
        /// ʹ��Ĭ�ϵĳ�ʼ������11������һ�� PriorityQueue
        /// ����������Ȼ˳����������Ԫ�أ�ʹ�� Comparable��
        /// </summary>
        public PriorityQueue()
        {
            this.queue = new object[DEFAULT_INITIAL_CAPACITY];
            this.comparator = null;  
        }
        
        /// <summary>
        /// ʹ��ָ���ĳ�ʼ��������һ�� PriorityQueue
        /// ����������Ȼ˳����������Ԫ�أ�ʹ�� Comparable��
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
        /// ʹ��Ĭ�ϵĳ�ʼ������11������һ�� PriorityQueue
        /// ������ָ���ıȽ�����������Ԫ��
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
        /// ʹ��ָ���ĳ�ʼ��������һ�� PriorityQueue
        /// ������ָ���ıȽ�����������Ԫ��
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
        /// ��������ָ�����ȶ�����Ԫ�ص� PriorityQueue
        /// ����ָ�����ȶ��еıȽ���������Ԫ��
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
        /// ����������ָ����Ԫ��
        /// ����Ƚ���Ϊ�գ�������Ȼ����
        /// ������ݱȽ���������Ԫ��
        /// </summary>
        /// <param name="e">T e</param>
        /// <returns>bool</returns>
        public bool add(T e)
        {
            if (e == null)
            {
                throw new ArgumentNullException();
            }
            //�Զ����������С
            if (size >= queue.Length)
                grow();

            if (size == 0)
            {
                queue[0] = e;
                size++;
                return true;
            }

            if (comparator != null)
                addUsingComparator(e);                //���ݱȽ���������
            else
                addUsingComparable(e);                //�Ƚ���Ϊ�գ���Ȼ����

            return true;        
        }
        
        /// <summary>
        /// ���ݱȽ�������ĺ�������add()����
        /// </summary>
        /// <param name="e">T e</param>
        private void addUsingComparator(T e)
        {
                queue[size++] = e;                     //��Ԫ����ӵ�����β
                int i = size - 1;
                //����ýڵ�����ĸ��ڵ�С�����븸�ڵ㽻��λ�ã�ѭ����������ڵ�
                while (  (i > 0) && ( comparator.Compare( (T)queue[i], (T)queue[(i-1)/2] ) < 0 )  )
                {
                    T temp = (T)queue[i];
                    queue[i] = queue[(i - 1) / 2];
                    queue[(i - 1) / 2] = temp;
                    i = (i - 1) / 2;
                }
        }
        
        /// <summary>
        /// ������Ȼ����ĺ�������add()����
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
                    //Console.WriteLine("����ʹ����Ȼ���򣬱����Զ���Comparator\n");
                    removeFirst();
                    size--;
                }                
        }

        /// <summary>
        /// ��ָ�������е�����Ԫ�ض���ӵ��˶�����
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
        /// ��ָ�������е�����Ԫ����ӵ��˶�����
        /// Ark(2008-3-28)������������List<T>��ΪIEnumerable<T>
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
        /// �����ȼ��������Ƴ�����Ԫ��
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
        /// ��ȡ�����Ƴ��˶��е�ͷ
        /// </summary>
        /// <returns>T</returns>
        public T getFirst()
        {
            if (size == 0)
            {
                //Console.WriteLine("������Ԫ�ظ���Ϊ0");
                return default(T);
            }
            T result;
            result = (T)queue[0];
            return result;
        }
        
        /// <summary>
        /// ��ȡ���Ƴ��˶��е�ͷ
        /// �Ƴ������¶ѻ�
        /// </summary>
        /// <returns>T</returns>
        public T removeFirst()
        {
            if (comparator != null)
                return removeUsingComparator();       //ʹ�ñȽ������¶ѻ�
            else
                return removeUsingComparable();       //ʹ����Ȼ�������¶ѻ�
        }

        /// <summary>
        /// ��removeFirst()����
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
        /// ��removeFirst()����
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

            //���¶ѻ�
            shiftDownUsingComparator(i);

            return result;
        }

        /// <summary>
        /// ɾ��������ָ����Ԫ��o,ɾ���ɹ�����true,��Ԫ�ز����ڷ���false
        /// ɾ��ȫ��Ԫ��o
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
        /// ɾ��������ָ����Ԫ��o
        /// ֻɾ����һ��Ԫ��o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool remove(T o)
        {
            int i = indexOf(o);

            //��������ڸ�Ԫ���򷵻�false
            if (i == -1)
                return false;

            if (comparator != null)
            {
                removeHelperUsingComparator(i, o);                   //ʹ���Զ���Ƚ���
            }
            else
            {
                removeHelperUsingComparable(i, o);                   //��ʹ���Զ���Ƚ���
            }
            return true;
        }

        /// <summary>
        /// ����Ƚ���comparator��Ϊ�գ���remove()����
        /// ����ɾ��λ��i�е�Ԫ��
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ot"></param>
        /// <returns></returns>
        private void removeHelperUsingComparator(int i, T ot)
        {
            //�����Ԫ��������β��
            if (i == size - 1)
            {
                queue[--size] = null;
            }
            else
            {
                T moved = (T)queue[size - 1];                      //�����һ��Ԫ�ش���moved����
                queue[i] = moved;                                  //�����һ��Ԫ�ز��뵽iλ�õ���                        
                queue[size - 1] = null;                            //ɾ�����һ��Ԫ��
                size--;                                            //�����С��һ

                shiftDownUsingComparator(i);                       //��Ԫ�����Ƶ��ʵ�λ��

                //���Ԫ�ػ���iλ�ã��򽫸�Ԫ�������Ƶ��ʵ���λ����
                //if (comparator.Compare((T)queue[i], moved) == 0)
                if (((T)queue[i]).GetHashCode() == moved.GetHashCode())
                    shiftUpUsingComparator(i, moved);
            }          
        }

        /// <summary>
        /// �����Զ���Ƚ�������λ��i��Ԫ�����Ƶ��������ʵ���λ��
        /// </summary>
        /// <param name="i">λ��i</param>
        private void shiftDownUsingComparator(int i)
        {
            int j = 0;

            //���¶ѻ�,���������Ԫ�������Ƶ��ʵ���λ����
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
        /// ���ݱȽ�������λ��i��Ԫ�����Ƶ��������ʵ���λ��
        /// </summary>
        /// <param name="i">λ��i</param>
        /// <param name="x">ԭ������ĩβ��Ԫ��</param>
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
        /// �������comparatorΪ�գ���remove()����
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
        /// ������Ȼ���򣬽�λ��i��Ԫ�����Ƶ��������ʵ���λ��
        /// </summary>
        /// <param name="i">λ��i</param>
        private void shiftDownUsingComparable(int i)
        {
            int j = 0;

            //���¶ѻ�
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
        /// ������Ȼ���򣬽�λ��i��Ԫ�����Ƶ��������ʵ���λ��
        /// </summary>
        /// <param name="i">λ��i</param>
        /// <param name="x">ԭ������ĩβ��Ԫ��</param>
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
            shiftDownUsingComparator(i);                       //��Ԫ�����Ƶ��ʵ�λ��

            //���Ԫ�ػ���iλ�ã��򽫸�Ԫ�������Ƶ��ʵ���λ����
            if (comparator.Compare((T)queue[i], temp) == 0)
                shiftUpUsingComparator(i, temp);
            return true;
        }

        /// <summary>
        /// 
        /// ����ָ��Ԫ��o�������е�λ��
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int indexOf(T o)
        {
            return Array.IndexOf(queue, o);
        }
                
        /// <summary>
        /// �����Զ�������������������ʱ��add()����
        /// </summary>
        private void grow()
        {
            int oldCapacity = queue.Length;
            //��������ԭ����С����������
            int newCapacity = ( oldCapacity < 64 ? (oldCapacity + 1) * 2 : (oldCapacity / 2) * 3 );
            Array.Resize(ref queue, newCapacity);
        }
        
        /// <summary>
        /// ��˳��������������Ԫ�أ�����ʹ��
        /// </summary>
        public void output()
        {
            for (int i = 0; i < size; i++)
                Console.WriteLine("The " + i + " element is " + queue[i]);
            Console.WriteLine("\nThere are " + size + " elements");
            Console.WriteLine("Queue total length is: " + queue.Length);
        }
        
        //��������
        public object[] getQueue()
        {
            return queue;
        }
        
        //����������Ԫ�ظ���
        public int getSize()
        {
            return size;
        }
        
        //���رȽ���
        public IComparer<T> getComparator()
        {
            return comparator;
        }

    }


}
