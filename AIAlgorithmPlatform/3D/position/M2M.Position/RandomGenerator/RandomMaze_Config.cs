using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace M2M.Position.RandomGenerator
{
    public enum mazeStyle
    {
        RandomMaze,
        RandomMaze3D
    }

    public class RandomMaze_Config
    {
        int width = 30, height = 20, depth = 10, branch_max = 2;
        int[,] map;
        int[,,] map3D;
        mazeStyle style = mazeStyle.RandomMaze;

        public void Produce()
        {
            if (style == mazeStyle.RandomMaze)
            {
                map = RandomMaze.generateMaze(width, height, branch_max);
            }
            else
            {
                map3D = RandomMaze.generateMaze3D(width, height, depth, branch_max);
            }
        }

        public int[,] Map
        {
            get { return map; }
        }

        public int[,,] Map3D
        {
            get { return map3D; }
        }

        [CategoryAttribute("Appearance")]
        public mazeStyle MazeStyle
        {
            get { return style; }
            set { style = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        [CategoryAttribute("Appearance")]
        public int Branch_max
        {
            get { return branch_max; }
            set { branch_max = value; }
        }
    }
}