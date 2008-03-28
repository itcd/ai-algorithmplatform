using System;
using System.Collections.Generic;
using System.Text;

/// 
/// Description:优先队列的接口
/// @date 2007.05.05
/// @author Zhou
/// 
namespace DataStructure.PriorityQueue
{
    public interface IPriorityQueue<T>
    {
        bool add(T e);                           //向队列中添加指定的元素
        bool addAll(IPriorityQueue<T> q);        //将指定队列中的所有元素都添加到此队列中
        bool addAll(IEnumerable<T> q);                  //将指定数组中的所有元素添加到此队列中
        void clear();                            //从优先级队列中移除所有元素
        T getFirst();                            //检索但不移除此队列的头
        T removeFirst();                         //检索并移除此队列的头
        bool remove(T o);                        //删除队列中指定的元素o,只删除一个元素
        bool removeAll(T o);                     //删除队列中指定的所有与元素o相等的元素，可能会删除多个元素
        bool update(T o);                        //更新元素o所在队列中的位置
        int indexOf(T o);                        //返回指定元素在数组中的位置,如果不存在返回-1
        object[] getQueue();                     //返回存储队列元素的数组
        int getSize();                           //返回队列元素个数
        IComparer<T> getComparator();            //返回比较器
    }
}
