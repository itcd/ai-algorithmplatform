using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    /// <summary>
    /// BinaryHeap 的摘要说明。-------二叉堆(基于数组的实现)
    /// </summary>
    public class BinaryHeap : IPriorityQueue
    {
        protected ArrayList array;
        //建立一个最多容纳_length个对象的空二叉堆
        public BinaryHeap(uint _length)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            array = new ArrayList((int)_length);
            array.Capacity = (int)_length;
        }

        //堆中对象个数
        public virtual int Count
        {
            get { return this.array.Count; }
        }

        //将成员数组变成用1为基数表达的形式
        public virtual object Item(int _i)
        {
            if (_i >= this.array.Capacity)
                throw new Exception("My:out of index");//不能出界
            return this.array[_i - 1];
        }

        #region IPriorityQueue 成员

        //先将空洞放在数组的下一个位置上，也就是i(注：基数是1),然后和[i/2]位置上的数比较，如果小于则将空洞上移到[i/2]位置，而原先[i/2]位置上的对象则移到[i]上，否则就将空洞变为_obj----如此递归
        public void Enqueue(Object _obj)
        {
            // TODO: 添加 BinaryHeap.Enqueue 实现
            if (this.array.Count == this.array.Capacity)
                throw new Exception("My:priority queue is full");//如果优先队列已满，则抛出异常
            this.array.Add(new object());
            int i = this.array.Count;
            while (i > 1 && Comparer.Default.Compare(this.array[i / 2 - 1], _obj) > 0)
            {
                //this.Item(i)=this.Item(i/2);
                this.array[i - 1] = this.array[i / 2 - 1];
                i /= 2;
            }
            this.array[i - 1] = _obj;
        }

        public object FindMin()
        {
            // TODO: 添加 BinaryHeap.FindMin 实现
            if (this.array.Count == 0)
                throw new Exception("My:priority queue is empty");//如果队列是空的，则抛出异常
            return this.array[0];
        }

        public object DequeueMin()
        {
            // TODO: 添加 BinaryHeap.DequeueMin 实现
            object tmpObj = this.FindMin();
            int i = 1;
            while ((2 * i + 1) <= this.array.Count)
            {
                if (Comparer.Default.Compare(this.array[2 * i - 1], this.array[2 * i]) <= 0)
                {
                    this.array[i - 1] = this.array[2 * i - 1];
                    this.array[2 * i - 1] = tmpObj;
                    i = 2 * i;
                }
                else
                {
                    this.array[i - 1] = this.array[2 * i];
                    this.array[2 * i] = tmpObj;
                    i = 2 * i + 1;
                }
            }

            object delObj = this.array[i - 1];//暂时储存要删去的元素

            if (i != this.array.Count)//如果搜索到的对象就是数组的最后一个对象，则什么都不要做
            {
                this.array[i - 1] = this.array[this.array.Count - 1];//添补空洞
            }
            this.array.RemoveAt(this.array.Count - 1);//将最后一个对象删除
            return delObj;
        }

        #endregion
    }
}
