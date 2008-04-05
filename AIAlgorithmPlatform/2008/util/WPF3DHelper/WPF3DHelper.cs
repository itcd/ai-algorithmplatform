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
//
//Update record:
//
//**************************************************************
//  Time:April 5,2008
//  Version:0.0.2
//  Author:李秉熙(lic3h6@gmail.com)
//  Descripe:This class contains some mesh transform function,
//           which include translate,scale and combine.
//  Update:Write function CombineTo, update Combine and its
//         overloads as a function which run CombineTo.
//**************************************************************

namespace M2M.Util
{
    public class WPF3DHelper
    {
        /// <summary>
        /// Translate mesh
        /// </summary>
        /// <param name="oldMesh3D">The mesh which need to be translated</param>
        /// <param name="vector">The translate vector</param>
        /// <returns>The mesh after being translated</returns>
        public static MeshGeometry3D Translate(MeshGeometry3D oldMesh3D, Vector3D vector)
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
        public static MeshGeometry3D Scale(MeshGeometry3D oldMesh3D, Vector3D vector)
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
        public static MeshGeometry3D Scale(MeshGeometry3D oldMesh3D, Vector3D vector, Point3D center)
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
        /// Combine a mesh to another mesh
        /// </summary>
        /// <param name="oldMesh3DOne">The mesh which a mesh will be combine to</param>
        /// <param name="oldMesh3DTwo">The mesh which need to be combined</param>
        public static void CombineTo(MeshGeometry3D oldMesh3DOne, MeshGeometry3D oldMesh3DTwo)
        {
            Point3D point;

            int count = oldMesh3DOne.Positions.Count;

            for (int i = 0; i < oldMesh3DTwo.Positions.Count; i++)
            {
                point = oldMesh3DTwo.Positions.ElementAt(i);
                oldMesh3DOne.Positions.Add(new Point3D(point.X, point.Y, point.Z));
            }

            for (int i = 0; i < oldMesh3DTwo.TriangleIndices.Count; i++)
            {
                oldMesh3DOne.TriangleIndices.Add(oldMesh3DTwo.TriangleIndices.ElementAt(i) + count);
            }
        }

        /// <summary>
        /// Combine two meshes
        /// </summary>
        /// <param name="oldMesh3DOne">The first mesh which need to be combined</param>
        /// <param name="oldMesh3DTwo">The second mesh which need to be combined</param>
        /// <returns>The mesh after being combined</returns>
        public static MeshGeometry3D Combine(MeshGeometry3D oldMesh3DOne, MeshGeometry3D oldMesh3DTwo)
        {
            MeshGeometry3D result = new MeshGeometry3D();

            CombineTo(result, oldMesh3DOne);
            CombineTo(result, oldMesh3DTwo);

            return result;
        }

        /// <summary>
        /// Combine numbers of meshes
        /// </summary>
        /// <param name="mesh3DList">A list of the mesh which need to be combined</param>
        /// <returns>The mesh after being combined</returns>
        public static MeshGeometry3D Combine(List<MeshGeometry3D> mesh3DList)
        {
            MeshGeometry3D result = new MeshGeometry3D();
            MeshGeometry3D current;

            for (int i = 0; i < mesh3DList.Count; i++)
            {
                current = mesh3DList.ElementAt(i);
                CombineTo(result, current);
            }

            return result;
        }
    }
}
