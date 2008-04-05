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
using Petzold.Media3D;
using M2M.Position.Implement;
using M2M.Position.Interface;

namespace M2M.Util
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

            Test1();
            Test2();
        }

        private void Test1()
        {
            List<IPosition3D> point3DList = new List<IPosition3D>();

            double range = 5;

            for (int i = 0; i < 500; i++)
            {
                point3DList.Add(new Position3D(
                    (int)GetRandomInRange(range), (int)GetRandomInRange(range), (int)GetRandomInRange(range)));
            }

            Scene scene = new Scene();
            //scene.AddElement(new PositionSetElement(point3DList));
            scene.AddElement(new GridSetElement(point3DList));

            scene.ShowScene();
        }

        private void Test2()
        {
            List<IPosition3D> point3DList = new List<IPosition3D>();

            double range = 100;

            for (int i = 0; i < 1000; i++)
            {
                point3DList.Add(new Position3D(
                    GetRandomInRange(range), GetRandomInRange(range), GetRandomInRange(range)));
            }

            Scene scene = new Scene();
            //scene.AddElement(new PositionSetElement(point3DList));
            scene.AddElement(new PositionSetElement(point3DList));

            scene.ShowScene();
        }

        Random random = new Random();
        private double GetRandomInRange(double range)
        {
            return ((random.NextDouble() - 0.5) * 2 * range);
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
