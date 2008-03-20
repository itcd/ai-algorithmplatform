using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DrawingLib
{
    /**
     * 彩色颜色集
     * 设定颜色的最大值max之后，从1至max为红橙黄绿青蓝紫红渐变色谱
     * 
     */

    //提供颜色映射的接口
    public interface ColorSet
    {
        Color getColor(int index);
        Color getReverseColor(int index);
        void setMaxValue(int max);
        int getMax();
        int getHalfMax();
        //int getColorIndex(Color c);
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    //按色谱渐变映射颜色：红橙黄绿青蓝紫
    public class Rainbow : ColorSet
    {
        const int MAX = 1530;//255*6=1530
        const int HALF_MAX = MAX / 2;
            const int COLOR_MAX = 255;
            const int COLOR_MIN = 0;
        Color[] mapping = new Color[MAX];//index:0~1529
        int maxValue = MAX;//地图的格子上的最大的数值
        float rate = 1;
        public Rainbow()
        {
            int i = 0;
            int r = 255, g = 0, b = 0;
            //生成色谱
            //r,g,b:255,0,0
            for (g = 0; g < COLOR_MAX; g++)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:255,255,0
            for (r = 255; r > COLOR_MIN; r--)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:0,255,0
            for (b = 0; b < COLOR_MAX; b++)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:0,255,255
            for (g = 255; g > COLOR_MIN; g--)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:0,0,255
            for (r = 0; r < COLOR_MAX; r++)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:255,0,255
            for (b = 255; b > COLOR_MIN; b--)
            {
                mapping[i] = Color.FromArgb(r, g, b);
                i++;
            }
            //r,g,b:255,0,0
        }

        //获得索引为index的颜色
        public Color getColor(int index)
        {
            if (index > 0)
            {
                if (maxValue != MAX)
                    return mapping[(int)((index - 1) * rate) % MAX];
                else
                    return mapping[(index - 1) % MAX];
            }
            else
            {
                if (index == 0)
                    return Color.White;
                else
                    return Color.Black;
            }
        }

        //获得索引为index的颜色的反色
        public Color getReverseColor(int index)
        {
            if (index > 0)
            {
                if (maxValue != MAX)
                    return mapping[((int)((index - 1) * rate) + HALF_MAX) % MAX];
                else
                    return mapping[(index - 1 + HALF_MAX) % MAX];
            }
            else
            {
                if (index == 0)
                    return Color.White;
                else
                    return Color.Black;
            }
        }

        public void setMaxValue(int max)
        {
            maxValue = max;
            rate = MAX / (float)maxValue;
        }

        public int getMax()
        {
            return MAX;
        }

        public int getHalfMax()
        {
            return HALF_MAX;
        }

        int getColorIndex(Color c)
        {
            const int M = 3;
            int[] a = {c.R, c.G, c.B};
            int[] ai = {0, 1, 2};

            int i, j;
            int index;
            for (i = 0; i < M - 1; i++)
            {
                index = i;
                for (j = i + 1; j < M; j++)
                {
                    if (a[j] < a[index])
                        index = i;
                }
                if (index != i)
                {
                    j = a[index];
                    a[index] = a[i];
                    a[i] = j;
                    j = ai[index];
                    ai[index] = ai[i];
                    ai[i] = j;
                }
            }

            a[0] = COLOR_MIN;
            a[M - 1] = COLOR_MAX;

            int r = a[ai[0]], g = a[ai[1]], b = a[ai[2]];

            return 0;
        }
    }
    //----------------------------------------------------------------
}
