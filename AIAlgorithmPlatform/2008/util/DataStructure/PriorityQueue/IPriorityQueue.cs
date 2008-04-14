using System;
using System.Collections.Generic;
using System.Text;

/// 
/// Description:���ȶ��еĽӿ�
/// @date 2007.05.05
/// @author Zhou
/// 
namespace M2M.Util.DataStructure.PriorityQueue
{
    public interface IPriorityQueue<T>
    {
        bool add(T e);                           //����������ָ����Ԫ��
        bool addAll(IPriorityQueue<T> q);        //��ָ�������е�����Ԫ�ض���ӵ��˶�����
        bool addAll(IEnumerable<T> q);                  //��ָ�������е�����Ԫ����ӵ��˶�����
        void clear();                            //�����ȼ��������Ƴ�����Ԫ��
        T getFirst();                            //���������Ƴ��˶��е�ͷ
        T removeFirst();                         //�������Ƴ��˶��е�ͷ
        bool remove(T o);                        //ɾ��������ָ����Ԫ��o,ֻɾ��һ��Ԫ��
        bool removeAll(T o);                     //ɾ��������ָ����������Ԫ��o��ȵ�Ԫ�أ����ܻ�ɾ�����Ԫ��
        bool update(T o);                        //����Ԫ��o���ڶ����е�λ��
        int indexOf(T o);                        //����ָ��Ԫ���������е�λ��,��������ڷ���-1
        object[] getQueue();                     //���ش洢����Ԫ�ص�����
        int getSize();                           //���ض���Ԫ�ظ���
        IComparer<T> getComparator();            //���رȽ���
    }
}
