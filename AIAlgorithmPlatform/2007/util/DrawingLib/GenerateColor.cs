using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DrawingLib
{
    public static class GenerateColor
    {
        //static Color[] colorList = {Color.Blue, Color.Coral, Color.DarkBlue, Color.DarkOrange, Color.DeepPink, Color.ForestGreen, Color.IndianRed, Color.LightGreen, Color.Olive,Color.SaddleBrown, Color.SandyBrown, Color.Turquoise, Color.RosyBrown};

        //static Random r = new Random((int)DateTime.Now.Ticks);

        static ColorSet colorSet = new Rainbow();

        static int max = colorSet.getMax();

        static int currentSequence = (int)(DateTime.Now.Ticks % max);

        static int differentStep = 260;

        static int similarStep = 20;

        public static Color GetNextDifferentColor()
        {
            //currentSequence += differentStep;

            //if(currentSequence <= max)
            //{
            //    return colorSet.getColor(currentSequence);
            //}
            //else
            //{
            //    currentSequence -= max;
            //    return colorSet.getColor(currentSequence); 
            //}

            currentSequence = (currentSequence + differentStep) % max;
            return colorSet.getColor(currentSequence);
        }

        public static Color GetNextSimilarColor()
        {
            //currentSequence += similarStep;

            //if (currentSequence <= max)
            //{
            //    return colorSet.getColor(currentSequence);
            //}
            //else
            //{
            //    currentSequence -= max;
            //    return colorSet.getColor(currentSequence);
            //}

            currentSequence = (currentSequence + similarStep) % max;
            return colorSet.getColor(currentSequence);
        }

        public static Color GetSimilarColor(Color color)
        {
            return GetSimilarColor(color, 40);
        }

        public static Color GetSimilarColor(Color color, int step)
        {
            int maxMatter = Math.Max(color.R, Math.Max(color.G, color.B));
            int minMatter = Math.Min(color.R, Math.Min(color.G, color.B));

            int matter = color.R;
            if (matter > minMatter && matter <= maxMatter)
            {
                if (matter + step < maxMatter)
                {
                    return Color.FromArgb(color.R + step, color.G, color.B);
                }
                else if (matter - step > minMatter)
                {
                    return Color.FromArgb(color.R - step, color.G, color.B);
                }
                else
                {
                    return Color.FromArgb(color.G, color.R, color.B);
                }
            }

            matter = color.G;
            if (matter > minMatter && matter <= maxMatter)
            {
                if (matter + step < maxMatter)
                {
                    return Color.FromArgb(color.R, color.G + step, color.B);
                }
                else if (matter - step > minMatter)
                {
                    return Color.FromArgb(color.R, color.G - step, color.B);
                }
                else
                {
                    return Color.FromArgb(color.R, color.B, color.G);
                }
            }

            matter = color.B;
            if (matter > minMatter && matter <= maxMatter)
            {
                if (matter + step < maxMatter)
                {
                    return Color.FromArgb(color.R, color.G, color.B + step);
                }
                else if (matter - step > minMatter)
                {
                    return Color.FromArgb(color.R, color.G, color.B - step);
                }
                else
                {
                    return Color.FromArgb(color.B, color.G, color.R);
                }
            }

            ///RGB相等
            if (color.R < 123)
            {
                return Color.FromArgb(color.R + step / 5, color.G + step / 3, color.B + step / 2);
            }
            else
            {
                return Color.FromArgb(color.R - step / 5, color.G - step / 3, color.B - step / 2);
            }
        }
    }
}
