using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using _3DTools;
using Petzold.Media3D;
using WPF3DHelper;

namespace TestForWPF3DHelper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            model.Transform = new Transform3DGroup();

            MeshGeometry3D mesh1 = new CubeMesh().Geometry;
            MeshGeometry3D mesh2 = new MeshGeometry3D();
            MeshGeometry3D mesh3;

            mesh2.Positions.Add(new Point3D(20, 20, 0));
            mesh2.Positions.Add(new Point3D(0, 20, 20));
            mesh2.Positions.Add(new Point3D(20, 0, 20));

            mesh2.TriangleIndices.Add(0);
            mesh2.TriangleIndices.Add(1);
            mesh2.TriangleIndices.Add(2);

            //List<MeshGeometry3D> list = new List<MeshGeometry3D>();
            //list.Add(mesh1);
            //list.Add(mesh2);
            mesh3 = WPF3DHelper.WPF3DHelper.Combine(mesh1, mesh2);
            //mesh3 = WPF3DHelper.WPF3DHelper.Combine(list);

            mesh1 = WPF3DHelper.WPF3DHelper.Translate(mesh1, new Vector3D(20, 20, 20));
            mesh2 = WPF3DHelper.WPF3DHelper.Scale(mesh2, new Vector3D(-3, -3, -3), new Point3D(0, 20, 20));

            GeometryModel3D mGeometry1 = new GeometryModel3D(mesh1, new DiffuseMaterial(Brushes.Red));
            GeometryModel3D mGeometry2 = new GeometryModel3D(mesh2, new DiffuseMaterial(Brushes.Green));
            GeometryModel3D mGeometry3 = new GeometryModel3D(mesh3, new DiffuseMaterial(Brushes.Blue));

            //model.Children.Add(mGeometry);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(mGeometry1);
            group.Children.Add(mGeometry2);
            group.Children.Add(mGeometry3);

            model.Content = group;
        }

        private bool mDown;
        private Point mLastPos;

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 25D);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (mDown)
            {
                Point pos = Mouse.GetPosition(viewport);
                Point actualPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                double mouseAngle = 0;
                if (dx != 0 && dy != 0)
                {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                }
                else if (dx == 0 && dy != 0) mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                else if (dx != 0 && dy == 0) mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;

                double axisAngle = mouseAngle + Math.PI / 2;

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                //QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));

                //tg.Children.Add(new RotateTransform3D(r));

                Transform3DGroup tg = model.Transform as Transform3DGroup;
                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                tg.Children.Add(new RotateTransform3D(r));

                mLastPos = actualPos;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mDown = true;
                Point pos = Mouse.GetPosition(viewport);
                mLastPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;
        }
    }
}
