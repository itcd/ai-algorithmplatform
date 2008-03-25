using System;
using System.Collections.Generic;
using System.Text;

namespace BFS_AStarforGrid
{
    //表示一个点结构
    [Serializable()]
    public class MyPoint
    {
        public int x;
        public int y;

        public MyPoint()
        {
            x = y = 0;
        }

        public MyPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //地图接口
    public interface Map
    {
        void setStart(int x, int y);
        void setEnd(int x, int y);
        MyPoint getStart();
        MyPoint getEnd();
        int getWidth();
        int getHeight();
        int getElementCount();
        //int getValue(int x, int y);
        //void setValue(int x, int y, int value);
        void assignMap(Map map);
        int getMaxValue();
        void setObstacle();
        void clear();
        bool isEmpty(int x, int y);
        void setObstacle(int x, int y);
        void setEmpty(int x, int y);
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //数组实现的地图类
    [Serializable()]
    public class ArrayMap : Map
    {
        int height = 5;
        int width = 10;
        int ElementCount;
        int[,] map;
        Random r = null;
        MyPoint start = null;
        MyPoint end = null;

        public void setStart(int x, int y)
        {
            start.x = x;
            start.y = y;
        }

        public void setEnd(int x, int y)
        {
            end.x = x;
            end.y = y;
        }

        public MyPoint getStart()
        {
            return start;
        }

        public MyPoint getEnd()
        {
            return end;
        }

        protected void init()
        {
            ElementCount = height * width;
            map = new int[height, width];
            start = new MyPoint(0, 0);
            end = new MyPoint(width - 1, height - 1);
        }

        public ArrayMap()
        {
            init();
        }

        public ArrayMap(int mapWidth, int mapHeight)
        {
            height = mapHeight;
            width = mapWidth;
            init();
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public int getElementCount()
        {
            return ElementCount;
        }

        public int getValue(int x, int y)
        {
            return map[y, x];
        }

        public void setValue(int x, int y, int value)
        {
            map[y, x] = value;
        }

        public bool isEmpty(int x, int y)
        {
            return map[y, x] == 0;
        }

        public void setObstacle(int x, int y)
        {
            map[y, x] = -1;
        }

        public void setEmpty(int x, int y)
        {
            map[y, x] = 0;
        }

        public void assignMap(Map map)
        {
            int i, j;
            int width = getWidth();
            int height = getHeight();

            start.x = map.getStart().x;
            start.y = map.getStart().y;
            end.x = map.getEnd().x;
            end.y = map.getEnd().y;

            if (map.getWidth() < width)
                width = map.getWidth();
            if (map.getHeight() < height)
                height = map.getHeight();

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    if (map.isEmpty(j, i))
                        this.map[i, j] = 0;
                    else
                        this.map[i, j] = -1;
                }
            }
        }

        public int getMaxValue()
        {
            int result = getValue(0, 0);
            int temp;
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth(); j++)
                {
                    temp = getValue(j, i);
                    if (temp > result)
                        result = temp;
                }
            }
            return result;
        }

        public void show()
        {
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth() - 1; j++)
                {
                    Console.Write(map[i, j]);
                    Console.Write("\t");
                }
                Console.Write(map[i, j]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void clear()
        {
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth(); j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        protected int getPartialRandom(int max)
        {
            int result = r.Next(max);
            int limit = max / 3;
            if (result > limit && result < max - limit)
            {
                switch (r.Next(max))
                {
                    case 0:
                        result = 0;
                        break;
                    case 1:
                        result = max - 1;
                        break;
                    default:
                        result = r.Next(max);
                        break;
                }
            }
            return result;
        }

        protected void setColObstacle()
        {
            int i;
            int row1;
            int row2;
            int col;

            do
            {
                row1 = getPartialRandom(getHeight());
                row2 = getPartialRandom(getHeight());
                col = getPartialRandom(getWidth());
                if (row1 > row2)
                {
                    i = row1;
                    row1 = row2;
                    row2 = i;
                }
            } while ((row2 - row1 > getHeight() / 3) || (col == start.x && row1 <= start.y && row2 >= start.y) || (col == end.x && row1 <= end.y && row2 >= end.y));
            for (i = row1; i < row2; i++)
            {
                setValue(col, i, -1);
            }
        }

        protected void setRowObstacle()
        {
            int i;
            int row;
            int col1;
            int col2;

            do
            {
                row = getPartialRandom(getHeight());
                col1 = getPartialRandom(getWidth());
                col2 = getPartialRandom(getWidth());
                if (col1 > col2)
                {
                    i = col1;
                    col1 = col2;
                    col2 = i;
                }
            } while ((col2 - col1 > getWidth() / 3) || (row == start.y && col1 <= start.x && col2 >= start.x) || (row == end.y && col1 <= end.x && col2 >= end.x));
            for (i = col1; i < col2; i++)
            {
                setValue(i, row, -1);
            }
        }

        public void setObstacle()
        {
            r = new Random();
            int i;
            for (i = 0; i < getWidth() / 2; i++)
            {
                setColObstacle();
            }
            for (i = 0; i < getHeight() / 2; i++)
            {
                setRowObstacle();
            }
        }
    }

    public class MapMark
    {
        int height = 5;
        int width = 10;
        int ElementCount;
        int[,] map;

        protected void init()
        {
            ElementCount = height * width;
            map = new int[height, width];
        }

        public MapMark()
        {
            init();
        }

        public MapMark(int mapWidth, int mapHeight)
        {
            height = mapHeight;
            width = mapWidth;
            init();
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public int getElementCount()
        {
            return ElementCount;
        }

        public int getValue(int x, int y)
        {
            return map[y, x];
        }

        public void setValue(int x, int y, int value)
        {
            map[y, x] = value;
        }

        public int getMaxValue()
        {
            int result = getValue(0, 0);
            int temp;
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth(); j++)
                {
                    temp = getValue(j, i);
                    if (temp > result)
                        result = temp;
                }
            }
            return result;
        }

        public void clear()
        {
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth(); j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        public void show()
        {
            int i, j;
            for (i = 0; i < getHeight(); i++)
            {
                for (j = 0; j < getWidth() - 1; j++)
                {
                    Console.Write(map[i, j]);
                    Console.Write("\t");
                }
                Console.Write(map[i, j]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
