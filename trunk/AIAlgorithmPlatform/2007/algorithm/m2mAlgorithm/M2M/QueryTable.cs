using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace M2M
{
    interface QueryTable
    {
        /// <summary>
        /// 初始化表格,包括申请内存
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void InitTable(int width,int height);

        /// <summary>
        /// 如果该点所在的分块已经存在则返回该分块的指针,否则返回null
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IPart_Edit GetPartRef(int x,int y);

        /// <summary>
        /// 创建该分块并返回该分块的指针
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IPart_Edit CreateAndGetPartRef(int x, int y);

        /// <summary>
        /// 从表格中删除一个已有分块指针,并返回其指针.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        IPart_Edit RemovePart(int x, int y);
    }
}