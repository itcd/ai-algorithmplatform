using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using Petzold.Media3D;
using M2M.Util.Scene;

using M2M.Position.Implement;

using Real = System.Double;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;
using PositionSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition>;
using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition_Connected>;


namespace Wpf_RandomMap_Demo
{
    public class MapModel
    {
        public static Material material = new DiffuseMaterial(Brushes.AliceBlue);

        // Geometry creation
        static MeshGeneratorBase m = new Petzold.Media3D.SphereMesh();
        static MeshGeometry3D sphere = m.Geometry;
        static Rect3D b = sphere.Bounds;

        public static void generateMap(Model3DGroup group, int[,] map, int width, int height)
        {
            if (group == null || map == null)
                return;

            GeometryModel3D mGeometry;
            Transform3DGroup tg;
            TranslateTransform3D tt;

            var positionSet = new PositionSet();

            // Add model to the model group
            int i, j;
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    if (map[i, j] > 0)
                    {
                        mGeometry = new GeometryModel3D(sphere, material);
                        tg = new Transform3DGroup();
                        tt = new TranslateTransform3D(i * b.SizeX, j * b.SizeY, 0);
                        tg.Children.Add(tt);
                        positionSet.Add(new Position3D(i * b.SizeX, j * b.SizeY, 0));

                        
                        mGeometry.Transform = tg;
                        group.Children.Add(mGeometry);
                    }
                }

            var scene = new Scene();
            scene.AddElement(new PositionSetElement(positionSet));
            scene.ShowScene();
        }

        public static void generateMap3D(Model3DGroup group, int[, ,] map, int width, int height, int depth)
        {
            if (group == null || map == null)
                return;

            GeometryModel3D mGeometry;
            Transform3DGroup tg;
            TranslateTransform3D tt;

            // Add model to the model group
            int i, j, k;
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                    for (k = 0; k < depth; k++)
                    {
                        if (map[i, j, k] > 0)
                        {
                            mGeometry = new GeometryModel3D(sphere, material);
                            tg = new Transform3DGroup();
                            tt = new TranslateTransform3D(i * b.SizeX, j * b.SizeY, k * b.SizeZ);
                            tg.Children.Add(tt);
                            mGeometry.Transform = tg;
                            group.Children.Add(mGeometry);
                        }
                    }
        }
    }
}
