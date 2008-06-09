using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Real = System.Double;
using IPositionSet = System.Collections.Generic.ICollection<M2M.Position.IPosition>;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.IPosition_Connected>;
using PositionSet = System.Collections.Generic.List<M2M.Position.IPosition>;
using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.IPosition_Connected>;
using IPosition3DSet = System.Collections.Generic.ICollection<M2M.Position.IPosition3D>;
using Position3DSet = System.Collections.Generic.List<M2M.Position.IPosition3D>;

using M2M.Position;

namespace M2M.Media3D
{
    public class MapModel
    {
        public static readonly int PATH_ID = -10;
        Material material_ball;
        Material material_ball_path;
        Material material_stick;

        // Geometry creation
        Rect3D b;
        MeshGeometry3D sphere;
        Real x_rate;
        Real y_rate;
        Real z_rate;
        Real stick_rate;
        MeshGeometry3D cylinder;
        Vector3D v_up;
        Vector3D v_down;
        Vector3D v_out;

        public MapModel()
        {
            material_ball = new DiffuseMaterial(Brushes.AliceBlue);
            material_ball_path = new DiffuseMaterial(Brushes.Lime);
            material_stick = new DiffuseMaterial(Brushes.BurlyWood);

            // Geometry creation
            sphere = new Petzold.Media3D.SphereMesh().Geometry;
            b = sphere.Bounds;
            x_rate = b.SizeX * 2;
            y_rate = b.SizeY * 2;
            z_rate = b.SizeZ * 2;
            stick_rate = 0.3;
            cylinder = new Petzold.Media3D.CylinderMesh().Geometry;
            v_up = new Vector3D(0, 1, 0);
            v_down = new Vector3D(0, -1, 0);
            v_out = new Vector3D(0, 0, 1);
        }

        public void generateMap(Model3DGroup group, int[,] map, int width, int height)
        {
            if (group == null || map == null)
                return;

            GeometryModel3D mGeometry;
            Transform3DGroup tg;
            TranslateTransform3D tt;

            // Add model to the model group
            int i, j;
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    if (map[i, j] > 0)
                    {
                        mGeometry = new GeometryModel3D(sphere, material_ball);
                        tg = new Transform3DGroup();
                        tt = new TranslateTransform3D(i * b.SizeX, j * b.SizeY, 0);
                        tg.Children.Add(tt);                          
                        mGeometry.Transform = tg;
                        group.Children.Add(mGeometry);
                    }
                }
        }

        public void generateMap3D(Model3DGroup group, int[, ,] map, int width, int height, int depth)
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
                            mGeometry = new GeometryModel3D(sphere, material_ball);
                            tg = new Transform3DGroup();
                            tt = new TranslateTransform3D(i * b.SizeX, j * b.SizeY, k * b.SizeZ);
                            tg.Children.Add(tt);
                            mGeometry.Transform = tg;
                            group.Children.Add(mGeometry);
                        }
                    }
        }

        public void generateMap_from_IPosition_ConnectedSet(Model3DGroup group, IPosition_ConnectedSet map)
        {
            if (group == null || map == null)
                return;

            GeometryModel3D mGeometry;
            Transform3DGroup tg;
            Real x, y, z;
            IEvaluator e = new EuclidDistanceEvaluator();
            Vector3D v_axis;

            // Add model to the model groups
            foreach (IPosition_Connected p in map)
            {
                if (p.GetTagIndex() != PATH_ID)
                    mGeometry = new GeometryModel3D(sphere, material_ball);
                else
                    mGeometry = new GeometryModel3D(sphere, material_ball_path);
                tg = new Transform3DGroup();
                x = p.GetValue(0) * x_rate;
                y = p.GetValue(1) * y_rate;
                if (p.GetDimension() > 2)
                    z = p.GetValue(2) * z_rate;
                else
                    z = 0;
                tg.Children.Add(new TranslateTransform3D(x, y, z));
                mGeometry.Transform = tg;
                group.Children.Add(mGeometry);

                foreach (IAdjacency a in p.GetAdjacencyOut())
                {
                    IPosition_Connected p2 = a.GetPosition_Connected();
                    Vector3D v_diff = Vector3D_IPosition.Subtract(p2, p);
                    mGeometry = new GeometryModel3D(cylinder, material_stick);
                    tg = new Transform3DGroup();
                    tg.Children.Add(new ScaleTransform3D(stick_rate, e.GetDistance(p, p2) * y_rate, stick_rate));
                    if (!Vector3D.Equals(v_diff, v_down))
                        v_axis = Vector3D.CrossProduct(v_up, v_diff);
                    else
                        v_axis = v_out;
                    tg.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(v_axis, Vector3D.AngleBetween(v_up, v_diff))));
                    tg.Children.Add(new TranslateTransform3D(x, y, z));
                    mGeometry.Transform = tg;
                    group.Children.Add(mGeometry);
                }
            }
        }
    }
}
