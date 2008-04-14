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

namespace TestFor3D
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool mDown;
        private Point mLastPos;

        public Window1()
        {
            InitializeComponent();
            
            //group.Transform = new Transform3DGroup();
            model.Transform = new Transform3DGroup();

            Material material = new DiffuseMaterial(Brushes.AliceBlue);
            MeshGeneratorBase MeshGeneratorBase = new Petzold.Media3D.SphereMesh();
            MeshGeometry3D sphere = MeshGeneratorBase.Geometry;

            //StarPoint
            Vector3D v1 = new Vector3D(30, 30, 0);
            GeometryModel3D StartPoint = new GeometryModel3D(sphere, material);
            TranslateTransform3D translateTransform3D1 = new TranslateTransform3D(v1);
            Transform3DGroup transform3DGroup1 = new Transform3DGroup();
            transform3DGroup1.Children.Add(new ScaleTransform3D(2, 2, 2));
            transform3DGroup1.Children.Add(translateTransform3D1);
            StartPoint.Transform = transform3DGroup1;

            //EndPoint
            Vector3D v2 = new Vector3D(-30, -30, 0);
            GeometryModel3D EndPoint = new GeometryModel3D(sphere, new DiffuseMaterial(Brushes.BurlyWood));
            EndPoint.Transform = new TranslateTransform3D(v2);

            //ModelVisual3D modelVisual3D = new ModelVisual3D();
            //modelVisual3D.Content = StartPoint;
            //model.Children.Add(modelVisual3D);

            Model3DGroup model3DGroup = new Model3DGroup();
            model3DGroup.Children.Add(StartPoint);
            model3DGroup.Children.Add(EndPoint);
            model.Content = model3DGroup;
            //model.Content = StartPoint;
            //model.Content = EndPoint;

            Cylinder cylinder = new Cylinder();
            cylinder.Fold1 = 0.25;
            cylinder.Fold2 = 0.75;
            cylinder.Radius1 = 0.3;
            cylinder.Radius2 = 1.0;
            cylinder.Material = material;
            cylinder.Point1 = new Point3D(v1.X,v1.Y,v1.Z);
            cylinder.Point2 = new Point3D(v2.X,v2.Y,v2.Z);
            model.Children.Add(cylinder);

            cylinder.Material = new DiffuseMaterial(Brushes.BurlyWood);
                        
            //ScreenSpaceLines3D ssl3D = new ScreenSpaceLines3D();
            //ssl3D.Color = Colors.White;
            //Point3DCollection gridLines = new Point3DCollection();

            //gridLines.Add(new Point3D(100, 0, 0));
            //gridLines.Add(new Point3D(-100, 0, 0));

            //int size = 100;
            //int interval = 10;
            //int len = 1000;

            //for (int i = -size; i <= size; i++)
            //{
            //    for (int j = -size; j <= size; j++)
            //    {
            //        // convert the loop iterators into the required grid cell size interval, relative to len
            //        double a = i * interval;
            //        double b = j * interval;

            //        // x line
            //        gridLines.Add(new Point3D(0, a, b));
            //        gridLines.Add(new Point3D(len, a, b));

            //        // y line
            //        gridLines.Add(new Point3D(a, 0, b));
            //        gridLines.Add(new Point3D(a, len, b));

            //        // z line
            //        gridLines.Add(new Point3D(a, b, 0));
            //        gridLines.Add(new Point3D(a, b, len));
            //    }
            //}
            //ssl3D.Points = gridLines;
            //ssl3D.Color = Colors.Red;
            //ssl3D.Thickness = 5;
            ////this.mainViewport.Children.Add(ssl3D);
            
            //viewport.Children.Add(ssl3D);
        }

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
