using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media.Media3D;
using M2M.Position.Interface;
using M2M.Position.Implement;


namespace M2M.Util
{
    public class CalculateGridInSurfaceEngine
    {
        public List<IPosition3D> CalculateGridInSurface(IList<Point3D> vertexs, IList<int> triangleIndices, Point3D RelativePoint, double gridLength)
        {
            List<IPosition3D> point3DList = new List<IPosition3D>();
            for (int i = 0; i < triangleIndices.Count / 3; i+=3)
            {
                IList<Point3D> triangle=new List<Point3D>();
                triangle.Add(new Point3D(vertexs[triangleIndices[i]].X - RelativePoint.X, vertexs[triangleIndices[i]].Y - RelativePoint.Y, vertexs[triangleIndices[i]].Z - RelativePoint.Z));
                triangle.Add(new Point3D(vertexs[triangleIndices[i+1]].X - RelativePoint.X, vertexs[triangleIndices[i+1]].Y - RelativePoint.Y, vertexs[triangleIndices[i+1]].Z - RelativePoint.Z));
                triangle.Add(new Point3D(vertexs[triangleIndices[i+2]].X - RelativePoint.X, vertexs[triangleIndices[i+2]].Y - RelativePoint.Y, vertexs[triangleIndices[i+2]].Z - RelativePoint.Z));
                CalculateGridInSurface(triangle, gridLength, point3DList);
            }

            return point3DList;
        }


        public void CalculateGridInSurface(IList<Point3D> vertexs, double gridLength, List<IPosition3D> point3DList)
        {
            double maxz = double.MinValue;
            double minz = double.MaxValue;
            int maxi = 0;
            int mini = 0;

            //找最值
            for (int i = 0; i < vertexs.Count; i++)
            {
                if (vertexs[i].Z > maxz) { maxz = vertexs[i].Z; maxi = i; }
                if (vertexs[i].Z <= minz) { minz = vertexs[i].Z; mini = i; }
            }

            //构造从最小值到最大值的一条路径
            IList<Point3D> vertexs1 = new List<Point3D>();
            for (int i = mini; i != maxi; i = (i + 1) % vertexs.Count)
                vertexs1.Add(vertexs[i]);
            vertexs1.Add(vertexs[maxi]);


            //构造从最小值到最大值的另一条路径
            IList<Point3D> vertexs2 = new List<Point3D>();
            for (int i = mini; i != maxi; i = (i - 1 + vertexs.Count) % vertexs.Count)
                vertexs2.Add(vertexs[i]);
            vertexs2.Add(vertexs[maxi]);

            CalculateGridInSurface(vertexs1, vertexs2, gridLength, point3DList);
        }


        private void CalculateGridInSurface(IList<Point3D> vertexs1, IList<Point3D> vertexs2, double gridLength, List<IPosition3D> point3DList)
        {
            
            //归一化处理
            double startx = vertexs1[0].X / gridLength;
            double starty = vertexs1[0].Y / gridLength;
            double startz = vertexs1[0].Z / gridLength;

            double endx = vertexs1[vertexs1.Count - 1].X / gridLength;
            double endy = vertexs1[vertexs1.Count - 1].Y / gridLength;
            double endz = vertexs1[vertexs1.Count - 1].Z / gridLength;

            double oldx1 = startx;
            double oldy1 = starty;
            double oldz1 = startz;

            double x1;
            double y1;
            double z1;

            double oldx2 = startx;
            double oldy2 = starty;
            double oldz2 = startz;

            double x2;
            double y2;
            double z2;

            double tempz;

            int p1 = 1;
            x1 = vertexs1[p1].X / gridLength;
            y1 = vertexs1[p1].Y / gridLength;
            z1 = vertexs1[p1].Z / gridLength;


            int p2 = 1;
            x2 = vertexs2[p2].X / gridLength;
            y2 = vertexs2[p2].Y / gridLength;
            z2 = vertexs2[p2].Z / gridLength;


            //迭代所有边界值
            for (int i = 0; i < Math.Abs((int)endz - (int)startz) + 1; i++)
            {
                List<Point3D> cutVertexs1 = new List<Point3D>();
                List<Point3D> cutVertexs2 = new List<Point3D>();

                cutVertexs1.Add(new Point3D(oldx1, oldy1, oldz1));

                while ((int)z1 == (int)startz + i)//当前列仍有点
                {
                    oldx1 = x1;
                    oldy1 = y1;
                    oldz1 = z1;

                    cutVertexs1.Add(new Point3D(oldx1, oldy1, oldz1));

                    if (++p1 == vertexs1.Count) goto q1;//遇到终点                    
                    x1 = vertexs1[p1].X / gridLength;
                    y1 = vertexs1[p1].Y / gridLength;
                    z1 = vertexs1[p1].Z / gridLength;//遍历下一点
                }

                tempz = (int)oldz1 + 1;
                oldx1 = (tempz - oldz1) * (x1 - oldx1) / (z1 - oldz1) + oldx1;
                oldy1 = (tempz - oldz1) * (y1 - oldy1) / (z1 - oldz1) + oldy1;
                oldz1 = tempz;
                //计算边界点

                cutVertexs1.Add(new Point3D(oldx1, oldy1, oldz1));//插入点
            
            q1:

                cutVertexs2.Add(new Point3D(oldx2, oldy2, oldz2));

                while ((int)z2 == (int)startz + i)//当前列仍有点
                {
                    oldx2 = x2;
                    oldy2 = y2;
                    oldz2 = z2;

                    cutVertexs2.Add(new Point3D(oldx2, oldy2, oldz2));

                    if (++p2 == vertexs2.Count) goto q2;//遇到终点                    
                    x2 = vertexs2[p2].X / gridLength;
                    y2 = vertexs2[p2].Y / gridLength;
                    z2 = vertexs2[p2].Z / gridLength;//遍历下一点
                }

                tempz = (int)oldz2 + 1;
                oldx2 = (tempz - oldz2) * (x2 - oldx2) / (z2 - oldz2) + oldx2;
                oldy2 = (tempz - oldz2) * (y2 - oldy2) / (z2 - oldz2) + oldy2;                
                oldz2 = tempz;
                //计算边界点

                cutVertexs2.Add(new Point3D(oldx2, oldy2, oldz2));//插入点
            
            q2:

                for (int j = cutVertexs2.Count - 1; j >= 0; j--) 
                    cutVertexs1.Add(cutVertexs2[j]);
                CalculateGridInSurface2D(cutVertexs1, gridLength, (int)startz + i, point3DList);
                //合并结果并求切面内所经过的方块
            }

        }

        
        private void CalculateGridInSurface2D(IList<Point3D> vertexs, double gridLength, double z, IList<IPosition3D> point3DList)
        {
            
            double maxx = double.MinValue;
            double minx = double.MaxValue;
            int maxi=0;
            int mini=0;

            //找最值
            for (int i = 0; i < vertexs.Count; i++)
            {
                if (vertexs[i].X > maxx) { maxx = vertexs[i].X; maxi = i; }
                if (vertexs[i].X <= minx) { minx = vertexs[i].X; mini = i; }
            }

            //构造从最小值到最大值的一条路径
            IList<Point3D> vertexs1 = new List<Point3D>();
            for (int i = mini; i != maxi; i = (i + 1) % vertexs.Count)
                vertexs1.Add(vertexs[i]);
            vertexs1.Add(vertexs[maxi]);

            //构造从最小值到最大值的另一条路径
            IList<Point3D> vertexs2 = new List<Point3D>();
            for (int i = mini; i != maxi; i = (i - 1 + vertexs.Count ) % vertexs.Count)
                vertexs2.Add(vertexs[i]);
            vertexs2.Add(vertexs[maxi]);

            CalculateGridInSurface2D(vertexs1, vertexs2, gridLength, z, point3DList);
        }


        private void CalculateGridInSurface2D(IList<Point3D> vertexs1, IList<Point3D> vertexs2, double gridLength, double z, IList<IPosition3D> point3DList)
        {
            double gridLength2 = 1;
            
            //归一化处理
            double startx = vertexs1[0].X / gridLength2;
            double endx = vertexs1[vertexs1.Count - 1].X / gridLength2;

            double starty = vertexs1[0].Y / gridLength2;
            double endy = vertexs1[vertexs1.Count - 1].Y / gridLength2;
            
            double oldx1 = startx;
            double oldy1 = starty;
            double x1;
            double y1;

            double oldx2 = startx;
            double oldy2 = starty;
            double x2;
            double y2;

            double tempx;

            int p1 = 1;
            x1 = vertexs1[p1].X / gridLength2;
            y1 = vertexs1[p1].Y / gridLength2;

            int p2 = 1;
            x2 = vertexs2[p2].X / gridLength2;
            y2 = vertexs2[p2].Y / gridLength2;

            
            //迭代所有边界值
            for (int i = 0; i < Math.Abs((int)endx - (int)startx) + 1; i++)
            {
                double maxy = oldy1;
                double miny = oldy1;

                while ((int)x1 == (int)startx + i)//当前列仍有点
                {
                    oldx1 = x1;
                    oldy1 = y1;
                    if (oldy1 > maxy) maxy = oldy1;
                    if (oldy1 < miny) miny = oldy1;////更新上一点的最值

                    if (++p1==vertexs1.Count) goto q1;//遇到终点                    
                    x1 = vertexs1[p1].X / gridLength2;
                    y1 = vertexs1[p1].Y / gridLength2;//遍历下一点
                }
                
                tempx = (int)oldx1 + 1;
                oldy1 = (tempx - oldx1) * (y1 - oldy1)/(x1 - oldx1)  + oldy1;
                oldx1 = tempx;
                //计算边界点

                if (oldy1 > maxy) maxy = oldy1;
                if (oldy1 < miny) miny = oldy1;
                //更新上一点的最值
            q1:

                
                if (oldy2 > maxy) maxy = oldy2;
                if (oldy2 < miny) miny = oldy2;

                while ((int)x2 == (int)startx + i)//当前列仍有点
                {
                    oldx2 = x2;
                    oldy2 = y2;
                    if (oldy2 > maxy) maxy = oldy2;
                    if (oldy2 < miny) miny = oldy2;////更新上一点的最值

                    if (++p2 == vertexs2.Count) goto q2;//遇到终点                    
                    x2 = vertexs2[p2].X / gridLength2;
                    y2 = vertexs2[p2].Y / gridLength2;//遍历下一点
                }

                tempx = (int)oldx2 + 1;
                oldy2 = (tempx - oldx2) * (y2 - oldy2) / (x2 - oldx2) + oldy2;
                oldx2 = tempx;
                //计算边界点

                if (oldy2 > maxy) maxy = oldy2;
                if (oldy2 < miny) miny = oldy2;
            //更新上一点的最值
            q2:
                
                for (int j = (int)miny; j <= (int)maxy; j++)
                    point3DList.Add(new Position3D(((int)startx + i + 0.5) * gridLength, (j + 0.5) * gridLength, (z + 0.5) * gridLength));
            }
        }
        
    }
}
