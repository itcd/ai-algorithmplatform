using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using BFS_AStarforGrid;

namespace DrawingLib
{
    //对画图层的封装
    public class Canvas
    {
        Graphics g = null;
        ColorBrushSet brushMap = null;

        public Canvas(Graphics graphics)
        {
            g = graphics;
            brushMap = new SolidColorBrushSet();
        }

        //填充矩形
        public void fillRectangle(int colorValue, int x, int y, int width, int height)
        {
            g.FillRectangle(brushMap.getBrush(colorValue), x, y, width, height);
        }

        //以反色填充矩形
        public void fillRectangleReversely(int colorValue, int x, int y, int width, int height)
        {
            g.FillRectangle(brushMap.getReverseBrush(colorValue), x, y, width, height);
        }

        public ColorBrushSet getBrushMap()
        {
            return brushMap;
        }
    }

    //绘画地图和路径
    public class MapDrawer
    {
        //左边界和定边界的距离
        int left = 0;
        int top = 0;
        //每个小方块的大小
        int blockWidth = 4;
        int blockHeight = 4;

        //画地图
        public void drawMap(Map map, Canvas canvas)
        {
            canvas.getBrushMap().setMaxValue(map.getMaxValue());
            int width = map.getWidth();
            int height = map.getHeight();
            int i, j;
            int temp;

            //根据是否有边距，在不同的位置画图
            if (left == 0 && top == 0)
            {
                for (i = 0; i < height; i++)
                    for (j = 0; j < width; j++)
                    {
                        temp = map.isEmpty(j, i) ? 0 : -1;
                        canvas.fillRectangle(temp, j * blockWidth, i * blockHeight, blockWidth, blockHeight);
                    }
            }
            else
            {
                for (i = 0; i < height; i++)
                    for (j = 0; j < width; j++)
                    {
                        temp = map.isEmpty(j, i) ? 0 : -1;
                        canvas.fillRectangle(temp, j * blockWidth + left, i * blockHeight + top, blockWidth, blockHeight);
                    }
            }
        }

        //画地图
        public void drawMap(MapMark map, Canvas canvas)
        {
            canvas.getBrushMap().setMaxValue(map.getMaxValue());
            int width = map.getWidth();
            int height = map.getHeight();
            int i, j;

            //根据是否有边距，在不同的位置画图
            if (left == 0 && top == 0)
            {
                for (i = 0; i < height; i++)
                    for (j = 0; j < width; j++)
                    {
                        canvas.fillRectangle(map.getValue(j, i), j * blockWidth, i * blockHeight, blockWidth, blockHeight);
                    }
            }
            else
            {
                for (i = 0; i < height; i++)
                    for (j = 0; j < width; j++)
                    {
                        canvas.fillRectangle(map.getValue(j, i), j * blockWidth + left, i * blockHeight + top, blockWidth, blockHeight);
                    }
            }
        }

        //画路径。应在画地图之后画路径，否则路径会被地图覆盖。
        public void drawPath(List<MyPoint> path, MapMark map, Canvas canvas)
        {
            if (path == null)
                return;

            int i;
            int halfBlockWidth = (int)(blockWidth / 2.0 + 0.5);
            int halfBlockHeight = (int)(blockHeight / 2.0 + 0.5);
            int blockLeft = blockWidth / 4;
            int blockTop = blockHeight / 4;

            //根据是否有边距，改变表示路径的方块的画图位置
            if (left != 0)
                blockLeft += left;
            if (top != 0)
                blockTop += top;

            for (i = 0; i < path.Count; i++)
            {
                canvas.fillRectangleReversely(map.getValue(path[i].x, path[i].y), path[i].x * blockWidth + blockLeft, path[i].y * blockHeight + blockTop, halfBlockWidth, halfBlockHeight);
            }
        }

        public void setDrawingPosition(int x, int y)
        {
            left = x;
            top = y;
        }

        public void setBlockSize(int width, int height)
        {
            blockWidth = width;
            blockHeight = height;
        }
    }
}
