using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.RandomGenerator
{
    public class RandomMaze_IPosition_Connected_Config
    {
        int width = 30, height = 20, depth = 10, branch_max = 2;
        MazeStyle style = RandomGenerator.MazeStyle.RandomMaze;

        public IPosition_ConnectedSet Produce()
        {
            IPosition_ConnectedSet set;
            if (style == RandomGenerator.MazeStyle.RandomMaze)
            {
                int[,] map = RandomMaze.generateMaze(width, height, branch_max);
                set = RandomMaze_IPosition_Connected.GenerateMap_from_Array(map, width, height);
            }
            else
            {
                int[, ,] map = RandomMaze.generateMaze3D(width, height, depth, branch_max);
                set = RandomMaze_IPosition_Connected.GenerateMap3D_from_Array(map, width, height, depth);
            }
            return set;
        }

        [CategoryAttribute("Appearance")]
        public MazeStyle MazeStyle
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

        [Category("Appearance")]
        public int Branch_max
        {
            get { return branch_max; }
            set { branch_max = value; }
        }
    }
}
