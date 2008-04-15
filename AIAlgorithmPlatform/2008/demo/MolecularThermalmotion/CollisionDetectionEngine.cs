using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MolecularThermalmotion
{
    public class CollisionDetectionEngine
    {
        static Dictionary<int, int>[, ,] gridMap;
        static List<Molecule> MoleculeSet;
        static int length, width, height, gridLength;

        public static void InitCollisionDetectionEngine(List<Molecule> mset, int l, int w, int h, int gl)
        {
            length = l;
            width = w;
            height = h;

            MoleculeSet = mset;
            gridMap = new Dictionary<int, int>[length, width, height];

            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                Dictionary<int, int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
                if (grid == null) grid = new Dictionary<int, int>();
                grid.Add(i, i);


            }
        }

        public static void UpdateGridmap()
        {
            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                if ((int)(m.oldPosition.X / gridLength) == (int)(m.oldPosition.X / gridLength) &&
                    (int)(m.oldPosition.Y / gridLength) == (int)(m.oldPosition.Y / gridLength) &&
                    (int)(m.oldPosition.Z / gridLength) == (int)(m.oldPosition.Z / gridLength))
                    continue;


                Dictionary<int, int> oldGrid = gridMap[(int)(m.oldPosition.X / gridLength), (int)(m.oldPosition.Y / gridLength), (int)(m.oldPosition.Z / gridLength)];
                oldGrid.Remove(i);

                Dictionary<int, int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
                if (grid == null) grid = new Dictionary<int, int>();
                grid.Add(i, i);
            }
        }

        public static void CollisionDetection()
        {
            for (int index = 0; index < MoleculeSet.Count; index++)
            {
                Molecule m = MoleculeSet[index];

                int x = (int)m.position.X / gridLength;
                int y = (int)m.position.Y / gridLength;
                int z = (int)m.position.Z / gridLength;

                Dictionary<int, int> grid = gridMap[x, y, z];

                for (int i = (x - 1) > 0 ? (x - 1) : 0; i <= ((x + 1) < (length - 1) ? (x + 1) : (length - 1)); i++)
                    for (int j = (y - 1) > 0 ? (y - 1) : 0; i <= ((y + 1) < (width - 1) ? (y + 1) : (width - 1)); j++)
                        for (int k = (z - 1) > 0 ? (z - 1) : 0; i <= ((z + 1) < (height - 1) ? (z + 1) : (height - 1)); k++)
                        {
                            Dictionary<int, int> neighborGrid = gridMap[i, j, k];
                            if (neighborGrid != null)
                            {
                                for (int l = 0; l < neighborGrid.Count; l++)
                                {
                                    int neighborIndex = neighborGrid.ElementAt(l).Key;
                                    if (neighborIndex <= index) continue;
                                    //Collide(index,neighborIndex);
                                }
                            }
                        }
            }
        }
    }
}
