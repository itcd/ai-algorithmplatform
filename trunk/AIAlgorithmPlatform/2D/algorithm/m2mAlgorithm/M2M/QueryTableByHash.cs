using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace M2M
{
    class QueryTableByHash : QueryTable
    {
        Hashtable ht_Part = null;
        IPart_Edit partInstance;
        int width;
        int height;
        public QueryTableByHash(Type partType)
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
            ht_Part = new Hashtable();
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
            int key = y * this.width + x;
            return (IPart_Edit)ht_Part[key];
        }

        /// <summary>
        /// 创建该分块并返回该分块的指针
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPart_Edit CreateAndGetPartRef(int x, int y)
        {
            int key = y * this.width + x;
            IPart_Edit part = partInstance.Create();
            ht_Part.Add(key, part);
            return part;
        }

        public IPart_Edit RemovePart(int x, int y)
        {
            int key = y * this.width + x;
            IPart_Edit tempPosition = (IPart_Edit)ht_Part[key];
            ht_Part.Remove(key);
            return tempPosition;
        }
    }
}
