using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

//**************************************************************
//  Time:April 1,2008
//  Version:0.0.1
//  Author:李秉熙(lic3h6@gmail.com)
//  Descripe:This class contains some mesh transform function,
//           which include translate,scale and combine.
//**************************************************************

namespace MeshGeometry3DTransform
{
    class MeshGeometry3DTransformer
    {
        /// <summary>
        /// Translate mesh
        /// </summary>
        /// <param name="oldMesh3D">The mesh which need to be translated</param>
        /// <param name="vector">The translate vector</param>
        /// <returns>The mesh after being translated</returns>
        public static MeshGeometry3D translate(MeshGeometry3D oldMesh3D, Vector3D vector)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            Point3D point;

            for (int i = 0; i < oldMesh3D.Positions.Count; i++)
            {
                point = oldMesh3D.Positions.ElementAt(i);
                result.Positions.Add(new Point3D(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z));
            }

            for (int i = 0; i < oldMesh3D.TriangleIndices.Count; i++)
            {
                result.TriangleIndices.Add(oldMesh3D.TriangleIndices.ElementAt(i));
            }
            
            return result;
        }

        /// <summary>
        /// Scale mesh
        /// </summary>
        /// <param name="oldMesh3D">The mesh which need to be scaled</param>
        /// <param name="vector">The scale vector</param>
        /// <returns>The mesh after being scaled</returns>
        public static MeshGeometry3D scale(MeshGeometry3D oldMesh3D, Vector3D vector)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            Point3D point;

            for (int i = 0; i < oldMesh3D.Positions.Count; i++)
            {
                point = oldMesh3D.Positions.ElementAt(i);
                result.Positions.Add(new Point3D(point.X * vector.X, point.Y * vector.Y, point.Z * vector.Z));
            }

            for (int i = 0; i < oldMesh3D.TriangleIndices.Count; i++)
            {
                result.TriangleIndices.Add(oldMesh3D.TriangleIndices.ElementAt(i));
            }

            return result;
        }

        /// <summary>
        /// Scale mesh
        /// </summary>
        /// <param name="oldMesh3D">The mesh which need to be scaled</param>
        /// <param name="vector">The scale vector</param>
        /// <param name="center">The scale center</param>
        /// <returns>The mesh after being scaled</returns>
        public static MeshGeometry3D scale(MeshGeometry3D oldMesh3D, Vector3D vector, Point3D center)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            Point3D point;

            for (int i = 0; i < oldMesh3D.Positions.Count; i++)
            {
                point = oldMesh3D.Positions.ElementAt(i);
                result.Positions.Add(new Point3D((point.X - center.X) * vector.X + center.X, (point.Y - center.Y) * vector.Y + center.Y, (point.Z - center.Z) * vector.Z + center.Z));
            }

            for (int i = 0; i < oldMesh3D.TriangleIndices.Count; i++)
            {
                result.TriangleIndices.Add(oldMesh3D.TriangleIndices.ElementAt(i));
            }

            return result;
        }

        /// <summary>
        /// Combine two meshes
        /// </summary>
        /// <param name="oldMesh3DOne">The first mesh which need to be combined</param>
        /// <param name="oldMesh3DTwo">The second mesh which need to be combined</param>
        /// <returns>The mesh after being combined</returns>
        public static MeshGeometry3D combine(MeshGeometry3D oldMesh3DOne, MeshGeometry3D oldMesh3DTwo)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            Point3D point;

            for (int i = 0; i < oldMesh3DOne.Positions.Count; i++)
            {
                point = oldMesh3DOne.Positions.ElementAt(i);
                result.Positions.Add(new Point3D(point.X, point.Y, point.Z));
            }

            for (int i = 0; i < oldMesh3DOne.TriangleIndices.Count; i++)
            {
                result.TriangleIndices.Add(oldMesh3DOne.TriangleIndices.ElementAt(i));
            }

            for (int i = 0; i < oldMesh3DTwo.Positions.Count; i++)
            {
                point = oldMesh3DTwo.Positions.ElementAt(i);
                result.Positions.Add(new Point3D(point.X, point.Y, point.Z));
            }

            for (int i = 0; i < oldMesh3DTwo.TriangleIndices.Count; i++)
            {
                result.TriangleIndices.Add(oldMesh3DTwo.TriangleIndices.ElementAt(i) + oldMesh3DOne.Positions.Count);
            }
            
            return result;
        }

        /// <summary>
        /// Combine numbers of meshes
        /// </summary>
        /// <param name="mesh3DList">A list of the mesh which need to be combined</param>
        /// <returns>The mesh after being combined</returns>
        public static MeshGeometry3D combine(List<MeshGeometry3D> mesh3DList)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            MeshGeometry3D current;
            Point3D point;
            int count = 0;

            for (int i = 0; i < mesh3DList.Count; i++)
            {
                current = mesh3DList.ElementAt(i);

                for (int j = 0; j < current.Positions.Count; j++)
                {
                    point = current.Positions.ElementAt(i);
                    result.Positions.Add(new Point3D(point.X,point.Y,point.Z));
                }

                count += current.Positions.Count;

                for (int j = 0; j < current.TriangleIndices.Count; j++)
                {
                    result.TriangleIndices.Add(current.TriangleIndices.ElementAt(i) + count);
                }
            }

            return result;
        }
    }
}
