using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace MolecularThermalmotion
{
    public class CollisionDetectionEngine
    {
        List<int>[, ,] gridMap;
        List<Molecule> moleculeSet;
        int length, width, height;
        double gridLength;
        List<int> movingList;

        public delegate void dCollisionResponse(int index1, int index2);
        public event dCollisionResponse CollisionResponse;

        public delegate void dCollideWithWall(int index);
        public event dCollideWithWall CollideWithWall;

        public void InitCollisionDetectionEngine(List<Molecule> mset, double l, double w, double h, double gl)
        {
            movingList = new List<int>();
            
            length = (int)Math.Ceiling(l/gl);
            width = (int)Math.Ceiling(w/gl);
            height = (int)Math.Ceiling(h/gl);
            gridLength = gl;

            moleculeSet = mset;
            gridMap = new List<int>[length, width, height];

            for (int i = 0; i < moleculeSet.Count; i++)
            {
                Molecule m = moleculeSet[i];

                int x=(int)(m.position.X / gridLength);
                int y=(int)(m.position.Y / gridLength);
                int z=(int)(m.position.Z / gridLength);
                if(gridMap[x,y,z]==null)gridMap[x,y,z]=new List<int>();

                gridMap[x, y, z].Add(i);


            }
        }

        public void UpdateToGridmap(int moleculeIndex)
        {
            if(CollideWithWall!=null)
                CollideWithWall(moleculeIndex);
            
            Molecule m = moleculeSet[moleculeIndex];

            int x = (int)(m.position.X / gridLength);
            int y = (int)(m.position.Y / gridLength);
            int z = (int)(m.position.Z / gridLength);

            int oldx = (int)(m.oldPosition.X / gridLength);
            int oldy = (int)(m.oldPosition.Y / gridLength);
            int oldz = (int)(m.oldPosition.Z / gridLength);


            if (oldx == x &&
                oldy == y &&
                oldz == z)
                return;


            List<int> oldGrid = gridMap[oldx, oldy, oldz];
            for(int i=0;i<oldGrid.Count;i++)
                if (oldGrid[i] == moleculeIndex)
                {
                    oldGrid.Remove(i);
                    break;
                }
     
            
            if (gridMap[x, y, z] == null) gridMap[x, y, z] = new List<int>();
            gridMap[x, y, z].Add(moleculeIndex);

            movingList.Add(moleculeIndex);

        }

        public void CollisionDetection()
        {
            if (CollisionResponse != null)
            {
                foreach (int index in movingList)
                {
                    moleculeSet[index].isNeedDetected = true;
                }

                foreach (int index in movingList)
                {
                    
                    Molecule m = moleculeSet[index];

                    if (!m.isNeedDetected) continue; //只对运动物体碰撞检测

                    int x = (int)(m.position.X / gridLength);
                    int y = (int)(m.position.Y / gridLength);
                    int z = (int)(m.position.Z / gridLength);

                    List<int> grid = gridMap[x, y, z];
                    //if (grid == null) continue;
                    for (int i = ((x - 1) > 0 ? (x - 1) : 0); i <= ((x + 1) < (length - 1) ? (x + 1) : (length - 1)); i++)
                        for (int j = ((y - 1) > 0 ? (y - 1) : 0); j <= ((y + 1) < (width - 1) ? (y + 1) : (width - 1)); j++)
                            for (int k = ((z - 1) > 0 ? (z - 1) : 0); k <= ((z + 1) < (height - 1) ? (z + 1) : (height - 1)); k++)
                            {

                                List<int> neighborGrid = gridMap[i, j, k];
                                
                                if (neighborGrid != null)
                                {
                                    
                                    for (int l = 0; l < neighborGrid.Count; l++)
                                    {
                                        
                                        int neighborIndex = neighborGrid[l];

                                        //if (moleculeSet[neighborIndex].isNeedDetected) continue; //避免两个运动物体重复碰撞检测

                                        if (Collide(index, neighborIndex))
                                        {
                                            moleculeSet[neighborIndex].isNeedDetected = false;
                                            moleculeSet[index].isNeedDetected = false;

                                            CollisionResponse(index, neighborIndex);
                                        }

                                    }
                                }
                            }
                }
            }
            movingList.Clear();
        }

        bool Collide(int index1, int index2)
        {

            Point3D p1 = moleculeSet[index1].position;
            Point3D p2 = moleculeSet[index2].position;
            if ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z)
                < (moleculeSet[index1].radius + moleculeSet[index2].radius) * (moleculeSet[index1].radius + moleculeSet[index2].radius))
                return true;
            return false;
        }
    }
}
