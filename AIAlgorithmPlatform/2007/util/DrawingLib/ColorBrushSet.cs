using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DrawingLib
{
    /**
     * 彩色画刷集
     * 设定颜色的最大值max之后，从1至max为红橙黄绿青蓝紫红渐变色谱
     * 
     */
    //----------------------------------------------------------------
    public interface ColorBrushSet
    {
        Brush getBrush(int index);
        Brush getReverseBrush(int index);
        void setMaxValue(int max);
        int getMax();
        int getHalfMax();
    }
    //----------------------------------------------------------------

    //----------------------------------------------------------------
    public class SolidColorBrushSet : ColorBrushSet
    {
        int max;
        int halfMax;
        int maxValue;//地图的格子上的最大的数值
        ColorSet colorMap = null;
        Brush[] brushMap = null;
        float rate = 1;

        public SolidColorBrushSet()
        {
            colorMap = new Rainbow();
            max = colorMap.getMax();
            halfMax = colorMap.getHalfMax();
            setMaxValue(max);
            brushMap = new SolidBrush[max];
            for (int i = 0; i < max; i++)
            {
                brushMap[i] = new SolidBrush(colorMap.getColor(i + 1));
            }
        }

        public void setMaxValue(int max)
        {
            maxValue = max;
            rate = this.max / (float)maxValue;
        }

        //获得索引为index的颜色的画刷
        public Brush getBrush(int index)
        {
            if (index > 0)
            {
                if (maxValue != max)
                    return brushMap[(int)((index - 1) * rate) % max];
                else
                    return brushMap[(index - 1) % max];
            }
            else
            {
                if (index == 0)
                    return Brushes.White;
                else
                    return Brushes.Black;
            }
        }

        //获得索引为index的颜色的反色画刷
        public Brush getReverseBrush(int index)
        {
            if (index > 0)
            {
                if (maxValue != max)
                    return brushMap[((int)((index - 1) * rate) + halfMax) % max];
                else
                    return brushMap[(index - 1 + halfMax) % max];
            }
            else
            {
                if (index == 0)
                    return Brushes.White;
                else
                    return Brushes.Black;
            }
        }

        public int getMax()
        {
            return max;
        }

        public int getHalfMax()
        {
            return halfMax;
        }
    }
}
