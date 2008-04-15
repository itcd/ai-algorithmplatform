using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace MolecularThermalmotion
{
    public class CollisionDetectionEngine
    {
        static List<int>[, ,] gridMap;
        static List<Molecule> MoleculeSet;
        static int length, width, height, gridLength;

        public delegate void dCollisionResponse(int index1, int index2);
        public static event dCollisionResponse CollisionResponse;

        public static void InitCollisionDetectionEngine(List<Molecule> mset, int l, int w, int h, int gl)
        {
            length = l;
            width = w;
            height = h;
            gridLength = gl;

            MoleculeSet = mset;
            gridMap = new List<int>[length, width, height];

            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                List<int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
                if (grid == null) grid = new List<int>();
                grid.Add(i);


            }
        }

        public static void UpdateToGridmap(int moleculeIndex)
        {
            Molecule m = MoleculeSet[moleculeIndex];

            if ((int)(m.oldPosition.X / gridLength) == (int)(m.oldPosition.X / gridLength) &&
                (int)(m.oldPosition.Y / gridLength) == (int)(m.oldPosition.Y / gridLength) &&
                (int)(m.oldPosition.Z / gridLength) == (int)(m.oldPosition.Z / gridLength))
                return;


            List<int> oldGrid = gridMap[(int)(m.oldPosition.X / gridLength), (int)(m.oldPosition.Y / gridLength), (int)(m.oldPosition.Z / gridLength)];
            for(int i=0;i<oldGrid.Count;i++)
                if (oldGrid[i] == moleculeIndex)
                {
                    oldGrid.Remove(i);
                    break;
                }


            List<int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
            if (grid == null) grid = new List<int>();
            grid.Add(moleculeIndex);

        }

        public static void CollisionDetection()
        {
            for (int index = 0; index < MoleculeSet.Count; index++)
            {
                Molecule m = MoleculeSet[index];
                
                if (!m.isMoved) continue; //只对运动物体碰撞检测

                int x = (int)m.position.X / gridLength;
                int y = (int)m.position.Y / gridLength;
                int z = (int)m.position.Z / gridLength;

                List<int> grid = gridMap[x, y, z];

                for (int i = (x - 1) > 0 ? (x - 1) : 0; i <= ((x + 1) < (length - 1) ? (x + 1) : (length - 1)); i++)
                    for (int j = (y - 1) > 0 ? (y - 1) : 0; i <= ((y + 1) < (width - 1) ? (y + 1) : (width - 1)); j++)
                        for (int k = (z - 1) > 0 ? (z - 1) : 0; i <= ((z + 1) < (height - 1) ? (z + 1) : (height - 1)); k++)
                        {
                            List<int> neighborGrid = gridMap[i, j, k];
                            if (neighborGrid != null)
                            {
                                for (int l = 0; l < neighborGrid.Count; l++)
                                {
                                    int neighborIndex = neighborGrid[l];
                                    if (MoleculeSet[neighborIndex].isMoved && neighborIndex <= index) continue; //避免两个运动物体重复碰撞检测
                                    if (Collide(index, neighborIndex)) ;
                                        CollisionResponse(index, neighborIndex);
                                    
                                }
                            }
                        }
            }
        }

        static bool Collide(int index1, int index2)
        {

            Point3D p1 = MoleculeSet[index1].position;
            Point3D p2 = MoleculeSet[index2].position;
            if ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z)
                < (MoleculeSet[index1].radius + MoleculeSet[index1].radius) * (MoleculeSet[index1].radius + MoleculeSet[index1].radius))
                return true;
            return false;
        }
    }
}
