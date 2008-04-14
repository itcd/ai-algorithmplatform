using System;
using System.Collections.Generic;
using System.Text;

namespace Position_Connected_Implement
{
    [Serializable]
    public class FullMap
    {
        public static int generateFullMap(int width, int height, out int[,] maze)
        {
            maze = new int[width, height];//��¼�Թ�·���ڵ�
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    maze[i, j] = 1;
            return width * height;
        }
    }
}
