using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.IPosition_Connected>;

namespace M2M.Position.RandomMap
{
    public class RandomMaze_IPosition_Connected_Config
    {
        static int width = 30;
        static int height = 20;
        static int depth = 10;
        static int branch_max = 2;
        static RandomMapStyle style = RandomMapStyle.RandomMaze;

        public IPosition_ConnectedSet Produce()
        {
            IPosition_ConnectedSet set = null;
            switch (style)
            {
                case RandomMapStyle.RandomMaze:
                    int[,] map = RandomMaze.generateMaze(width, height, branch_max);
                    set = RandomMaze_IPosition_Connected.GenerateMap_from_Array(map, width, height);
                    break;
                case RandomMapStyle.RandomMaze3D:
                    int[, ,] map3d = RandomMaze.generateMaze3D(width, height, depth, branch_max);
                    set = RandomMaze_IPosition_Connected.GenerateMap3D_from_Array(map3d, width, height, depth);
                    break;
            }
            return set;
        }

        [CategoryAttribute("Appearance")]
        public RandomMapStyle RandomMapStyle
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
