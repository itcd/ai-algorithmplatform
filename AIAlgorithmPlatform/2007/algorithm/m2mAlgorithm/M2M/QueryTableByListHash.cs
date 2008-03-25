using System;
using System.Collections.Generic;
using System.Text;

namespace M2M
{
    class LinkedList
    {
        public IPart_Edit part;
        public LinkedList next;
        public LinkedList()
        {
            this.part = null;
            this.next = null;
        }
        public LinkedList(IPart_Edit part, LinkedList next)
        {
            this.part = part;
            this.next = next;
        }
    }
    class QueryTableByListHash : QueryTable
    {
        LinkedList[,] Part_list;
        IPart_Edit partInstance;
        int width;
        int height;
        int scale;
        int shift;
        public QueryTableByListHash(Type partType)
        {
            partInstance = (IPart_Edit)Activator.CreateInstance(partType);
        }
        /// <summary>
        /// 初始化表格,包括申请内存
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitTable(int width, int height)
        {
            this.width = width;
            this.height = height;
            scale = 8;
            shift = (int)Math.Log(scale, 2.0);
            int a = width / scale + 1;
            int b = height / scale + 1;
            Part_list = new LinkedList[a, b];
            //for (int i = 0; i < a; i++)
            //    for (int j = 0; j < b; j++)
            //        Part_list[i, j] = new LinkedList();
        }

        /// <summary>
        /// 如果该点所在的分块已经存在则返回该分块的指针,否则返回null
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPart_Edit GetPartRef(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= width) || (y >= height))
            {
                return null;
            }

            return GetPartFromList(x, y);
        }

        /// <summary>
        /// 创建该分块并返回该分块的指针
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPart_Edit CreateAndGetPartRef(int x, int y)
        {
            IPart_Edit part = partInstance.Create();
            int tableX = x / scale;
            int tableY = y / scale;
            LinkedList l = Part_list[tableX, tableY];
            if(l == null)
            {
                l = new LinkedList(part, null);
                Part_list[tableX, tableY] = l;
                return part;
            }
           
            if (l.part == null)
            {
                l.part = part;
            }
            else
            {
                LinkedList newHead = new LinkedList(part, null);
                LinkedList temp = l.next;
                newHead.next = temp;
                l.next = newHead;
            }
            return part;
        }

        public IPart_Edit RemovePart(int x, int y)
        {
            LinkedList tempPartList = GetList(x, y);
            IPart_Edit part = GetPart(tempPartList, x, y);
            if (part != null)
            {
                //删除链表中的某一结点
                LinkedList currentList = tempPartList.next;
                LinkedList tempList = tempPartList;
                while (currentList != null)
                {
                    if (currentList.part.GetX() == part.GetX() && currentList.part.GetY() == part.GetY())
                    {
                        tempList = currentList.next;
                        currentList.next = null;
                    }
                    tempList = currentList;
                    currentList = currentList.next;
                }

            }
            return part;
        }
        private IPart_Edit GetPartFromList(int x, int y)
        {
            LinkedList tempPartList = GetList(x, y);
            return GetPart(tempPartList, x, y);
        }
        private LinkedList GetList(int x, int y)
        {
            return Part_list[x / scale, y / scale];
        }
        private IPart_Edit GetPart(LinkedList al, int x, int y)
        {
            LinkedList temp = al.next;
            while (temp != null)
            {
                if (temp.part.GetX() == x && temp.part.GetY() == y)
                    return temp.part;
                temp = temp.next;
            }
            return null;
        }
    }
}
