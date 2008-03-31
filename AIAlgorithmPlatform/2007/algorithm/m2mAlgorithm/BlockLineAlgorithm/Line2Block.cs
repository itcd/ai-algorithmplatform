using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using WriteLineInGridEngine;
using Position_Implement;

namespace BlockLineAlgorithm
{
    public class Line2Block:IWriteLineInGridEngine
    {
        /// <summary>
        /// 二维Line2Block实现
        /// </summary>
        /// <param name="gridWidth">格的宽度</param>
        /// <param name="gridHeight">格的高度</param>
        /// <param name="startPosition">线段开始点</param>
        /// <param name="endPosition">线段终止点</param>
        /// <returns>线段所经过的方块的中点坐标集合（不包括开始点和终止点所在的方块），如果是空集直接返回NULL</returns>
        
        public IPositionSet WriteLineInGrid(float gridWidth, float gridHeight, IPosition startPosition, IPosition endPosition)
        {
            
            
            //归一化处理
            float temp;
            float endx=endPosition.GetX()/gridWidth;
            float startx=startPosition.GetX()/gridWidth;
            float endy=endPosition.GetY()/gridHeight;
            float starty=startPosition.GetY()/gridHeight;

            float dx = endx-startx;
            float dy = endy-starty;

            //如果两点在同一个方块里就返回空
            if ((int)startx == (int)endx && (int)starty == (int)endy)
            {
                //positionSet.AddPosition(new Position_Point(((int)startx+0.5f)*gridWidth,((int)starty+0.5f)*gridHeight));
                return null;
            }

            PositionSetEdit_ImplementByICollectionTemplate positionSet = new PositionSetEdit_ImplementByICollectionTemplate();
          
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                //保证起点x坐标小于终点x坐标
                if (startx > endx)
                {
                    temp = startx; startx = endx; endx = temp;
                    temp = starty; starty = endy; endy = temp;
                    dx = -dx;
                    dy = -dy;
                }

                //添加起点所在的方块
                //positionSet.AddPosition(new Position_Point(((int)startx + 0.5f) * gridWidth, ((int)starty + 0.5f) * gridHeight));

                float slope = dy / dx;
                float x = (int)startx + 1;
                float y = (x - startx) * slope + starty;//算出第一个边界值
                float oldy = y;

                //添加可能出现的与起点所在分块同一列的方块
                if ((int)y != (int)starty)
                    positionSet.AddPosition(new Position_Point(((int)x-1 + 0.5f) * gridWidth, ((int)y + 0.5f) * gridHeight));
                
                //迭代所有边界值
                for (int i = 0; i < (int)endx - (int)startx-1; i++)
                {
                    oldy = y;
                    x += 1;
                    y += slope;
                    positionSet.AddPosition(new Position_Point(((int)x-1 + 0.5f) * gridWidth, ((int)oldy + 0.5f) * gridHeight));
                    if ((int)oldy != (int)y)
                        positionSet.AddPosition(new Position_Point(((int)x-1 + 0.5f) * gridWidth, ((int)y + 0.5f) * gridHeight));                                        
                }
                
                //添加可能出现的与终点所在分块同一列的方块
                if ((int)y != (int)endy)
                    positionSet.AddPosition(new Position_Point(((int)x + 0.5f) * gridWidth, ((int)y + 0.5f) * gridHeight));
            }
            else
            {

                //保证起点y坐标小于终点y坐标
                if (starty > endy)
                {
                    temp = startx; startx = endx; endx = temp;
                    temp = starty; starty = endy; endy = temp;
                    dx = -dx;
                    dy = -dy;
                }

                //添加起点所在的方块
                //positionSet.AddPosition(new Position_Point(((int)startx + 0.5f) * gridWidth, ((int)starty + 0.5f) * gridHeight));

                float slope = dx / dy;
                float y = (int)starty + 1;
                float x = (y - starty) * slope + startx;//算出第一个边界值
                float oldx = x;

                //添加可能出现的与终点所在分块同一行的方块
                if ((int)x != (int)startx)
                    positionSet.AddPosition(new Position_Point(((int)x + 0.5f) * gridWidth, ((int)y-1 + 0.5f) * gridHeight));               
                
                //迭代所有边界值
                for (int i = 0; i < (int)endy - (int)starty-1; i++)
                {
                    oldx = x;
                    x += slope;
                    y += 1;
                    
                    positionSet.AddPosition(new Position_Point(((int)oldx + 0.5f) * gridWidth, ((int)y-1 + 0.5f) * gridHeight));
                    if ((int)oldx != (int)x)
                        positionSet.AddPosition(new Position_Point(((int)x + 0.5f) * gridWidth, ((int)y-1 + 0.5f) * gridHeight));                    
                }

                //添加可能出现的与终点所在分块同一行的方块
                if ((int)x != (int)endx)
                    positionSet.AddPosition(new Position_Point(((int)x + 0.5f) * gridWidth, ((int)y + 0.5f) * gridHeight));
            }
            
            return positionSet.GetNum()==0?null:positionSet;
        }
        
    }
}
