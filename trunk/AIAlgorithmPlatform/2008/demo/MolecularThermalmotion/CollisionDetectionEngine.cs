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

        Point3D gridMapOrigin;

        int TransferToGridX(double x)
        {
            return (int)((x - gridMapOrigin.X) / gridLength);
        }

        int TransferToGridY(double y)
        {
            return (int)((y - gridMapOrigin.Y) / gridLength);
        }

        int TransferToGridZ(double z)
        {
            return (int)((z - gridMapOrigin.Z) / gridLength);
        }

        public delegate void dCollisionResponse(int index1, int index2);
        public event dCollisionResponse CollisionResponse;

        public delegate void dCollideWithWall(int index);
        public event dCollideWithWall CollideWithWall;

        

        public void InitCollisionDetectionEngine(List<Molecule> mset, Point3D gmorigin, double l, double w, double h, double gl)
        {
            movingList = new List<int>();

            gridMapOrigin = gmorigin;
            
            length = (int)Math.Ceiling(l/gl);
            width = (int)Math.Ceiling(w/gl);
            height = (int)Math.Ceiling(h/gl);
            gridLength = gl;

            moleculeSet = mset;


            gridMap = new List<int>[length, width, height];

            for (int i = 0; i < moleculeSet.Count; i++)
            {
                Molecule m = moleculeSet[i];

                int x = TransferToGridX(m.position.X);
                int y = TransferToGridY(m.position.Y);
                int z = TransferToGridZ(m.position.Z);
                if(gridMap[x,y,z]==null)gridMap[x,y,z]=new List<int>();

                gridMap[x, y, z].Add(i);


            }
        }

        public void UpdateToGridmap(int moleculeIndex)
        {
            movingList.Add(moleculeIndex);

            if(CollideWithWall!=null)
                CollideWithWall(moleculeIndex);
            
            Molecule m = moleculeSet[moleculeIndex];

            int x = TransferToGridX(m.position.X);
            int y = TransferToGridY(m.position.Y);
            int z = TransferToGridZ(m.position.Z);

            //oldPosition没赋值
            int oldx = TransferToGridX(m.oldPosition.X);
            int oldy = TransferToGridY(m.oldPosition.Y);
            int oldz = TransferToGridZ(m.oldPosition.Z);


            if (oldx == x &&
                oldy == y &&
                oldz == z)
                return;

            //要注意remove和removeAt的用法
            gridMap[oldx, oldy, oldz].Remove(moleculeIndex);
            
            //List<int> oldGrid = gridMap[oldx, oldy, oldz];
            //for(int i=0;i<oldGrid.Count;i++)
            //    if (oldGrid[i] == moleculeIndex)
            //    {
            //        oldGrid.RemoveAt(i);
            ;            //        break;
            //    }
     
            
            if (gridMap[x, y, z] == null) gridMap[x, y, z] = new List<int>();

            gridMap[x, y, z].Add(moleculeIndex);
        }

        public void CollisionDetection()
        {
            if (CollisionResponse != null)
            {
                foreach (int index in movingList)
                {
                    
                    Molecule m = moleculeSet[index];

                    if (!m.isNeedDetected) continue; //只对运动物体碰撞检测

                    int x = TransferToGridX(m.position.X);
                    int y = TransferToGridY(m.position.Y);
                    int z = TransferToGridZ(m.position.Z);

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

                                            if (neighborIndex == index) continue;

                                            //if (!moleculeSet[neighborIndex].isNeedDetected) continue; //避免两个运动物体重复碰撞检测

                                            if (Collide(index, neighborIndex))
                                            {
                                                //moleculeSet[neighborIndex].isNeedDetected = false;
                                                //moleculeSet[index].isNeedDetected = false;

                                                CollisionResponse(index, neighborIndex);


                                                //每一帧对于每个球只跟另外的一个球进行碰撞
                                                //goto jumpOut;
                                            }
                                        }
                                    }
                                }
                    jumpOut:
                        {
                    }

                
                }

                //foreach (int index in movingList)
                //{
                //    moleculeSet[movingList[index]].isNeedDetected = true;
                //}

                foreach (Molecule m in moleculeSet)
                {
                    m.isNeedDetected = true;
                }
            }
            movingList.Clear();
        }

        bool Collide(int index1, int index2)
        {

            Point3D p1 = moleculeSet[index1].position;
            Point3D p2 = moleculeSet[index2].position;
            if ((p1 - p2).Length <= moleculeSet[index1].radius + moleculeSet[index2].radius)
            {
                return true;
            }
            //if ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z)
            //    < (moleculeSet[index1].radius + moleculeSet[index2].radius) * (moleculeSet[index1].radius + moleculeSet[index2].radius))
            //    return true;
            return false;
        }
    }
}
