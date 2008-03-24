using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace M2M
{
    class QueryTableByArray : QueryTable
    {
        //Type partType;
        IPart_Edit partInstance;

        IPart_Edit[,] array = null;

        int width;
        int height;

        public QueryTableByArray(Type partType)
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
            array = new IPart_Edit[height, width];
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
            
            return array[y, x];
        }

        /// <summary>
        /// 创建该分块并返回该分块的指针
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPart_Edit CreateAndGetPartRef(int x, int y)
        {
            IPart_Edit temp = array[y, x];
            array[y, x] = partInstance.Create();
            return array[y, x];
        }

        public IPart_Edit RemovePart(int x, int y)
        {
            IPart_Edit tempPosition = array[y, x];
            array[y, x] = null;
            return tempPosition;
        }
    }
}