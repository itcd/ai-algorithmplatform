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
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using M2M.Util;

namespace MolecularThermalmotion
{
    /// <summary>
    /// Interaction logic for GameWindows.xaml
    /// </summary>
    public partial class GameWindows : Window
    {
        private Game game;
        internal Game CurrentGame
        {
            get { return game; }
            set { game = value; }
        }

        internal GameWindows(Game game)
        {
            InitializeComponent();

            model.Transform = new Transform3DGroup();

            this.game = game;
            game.ShootForceFactor = progressBar1.Value;
        }

        public void SetShootDirectionAndForceFactor()
        {
            //这时候使到那些control有效
        }

        // variables for mouse controlling
        private bool leftDown = false;
        private Point leftLastPos;
        private bool middleDown = false;
        private Point middleStartPos;
        private bool rightDown = false;
        private Point rightLastPos;

        private GameWindows()
        {
            InitializeComponent();

            model.Transform = new Transform3DGroup();

            game.ShootForceFactor = progressBar1.Value;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 25D);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftDown)
            {
                Point pos = Mouse.GetPosition(viewport);
                Point actualPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - leftLastPos.X, dy = actualPos.Y - leftLastPos.Y;

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
                double rotation = 0.002 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                Transform3DGroup tg = model.Transform as Transform3DGroup;
                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                tg.Children.Add(new RotateTransform3D(r));

                leftLastPos = actualPos;
            }

            if(middleDown)
            {
                double value = progressBar1.Value + middleStartPos.Y - Mouse.GetPosition(viewport).Y;
                if (value > progressBar1.Maximum)
                    value = progressBar1.Maximum;
                if (value < progressBar1.Minimum)
                    value = progressBar1.Minimum;
                if (value != progressBar1.Value)
                {
                    progressBar1.Value = value;
                    game.ShootForceFactor = value;
                }
            }

            if(rightDown)
            {
                Point pos = Mouse.GetPosition(viewport);
                Point actualPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);

                //game.ShotDirection = Trackball.RotateTheVector3D(game.ShotDirection, rightLastPos.X, rightLastPos.Y, actualPos.X, actualPos.Y);

                {
                    //Vector3D v = game.ShotDirection;
                    //double x1 = rightLastPos.X;
                    //double y1 = rightLastPos.Y;
                    //double x2 = actualPos.X;
                    //double y2 = actualPos.Y;

                    //double d1 = Math.Sqrt(x1 * x1 + y1 * y1);
                    //double d2 = Math.Sqrt(x2 * x2 + y2 * y2);
                    //double radius = d1 > d2 ? d1 : d2;
                    //double z1 = Trackball.projectToSphere(radius, x1, y1);
                    //double z2 = Trackball.projectToSphere(radius, x2, y2);

                    //Vector3D v1 = new Vector3D(x1, y1, z1);
                    //Vector3D v2 = new Vector3D(x2, y2, z2);
                    //Vector3D n = Vector3D.CrossProduct(v1, v2) / 10;
                    //double angle = Math.Asin(n.Length / v1.Length / v2.Length);

                    //Quaternion q = new Quaternion(n, angle * 180 / Math.PI);
                    //Quaternion qv = new Quaternion(v.X, v.Y, v.Z, 1);
                    //Quaternion result = q * qv;
                    //q.Conjugate();
                    //result = result * q;

                    //game.ShotDirection = new Vector3D(result.X, result.Y, result.Z) * model.Transform.Value;

                    double dx = actualPos.X - rightLastPos.X, dy = actualPos.Y - rightLastPos.Y;

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
                    double rotation = 0.002 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                    var tempMatrix = model.Transform.Value;
                    tempMatrix.Invert();

                    axis = axis * tempMatrix;

                    //Transform3DGroup tg = model.Transform as Transform3DGroup;
                    QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                    //tg.Children.Add(new RotateTransform3D(r));

                    game.ShotDirection = game.ShotDirection * (new RotateTransform3D(r).Value);


                }

                rightLastPos = actualPos;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    leftDown = true;
                    Point pos = Mouse.GetPosition(viewport);
                    leftLastPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
                    break;

                case MouseButton.Middle:
                    middleDown = true;
                    middleStartPos = Mouse.GetPosition(viewport);
                    break;

                case MouseButton.Right:
                    rightDown = true;
                    Point p = Mouse.GetPosition(viewport);
                    rightLastPos = new Point(p.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - p.Y);
                    break;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    leftDown = false;
                    break;

                case MouseButton.Middle:
                    middleDown = false;
                    game.ShootWhiteBallAndContinueGameLoop();
                    break;

                case MouseButton.Right:
                    rightDown = false;
                    break;
            }
        }

        private void sliderShootDIrectionX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            game.ShotDirection = new Vector3D(sliderShootDIrectionX.Value,
                sliderShootDIrectionY.Value,
                sliderShootDIrectionZ.Value);
        }

        private void sliderShootDIrectionY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            game.ShotDirection = new Vector3D(sliderShootDIrectionX.Value,
                sliderShootDIrectionY.Value,
                sliderShootDIrectionZ.Value);
        }

        private void sliderShootDIrectionZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            game.ShotDirection = new Vector3D(sliderShootDIrectionX.Value,
                sliderShootDIrectionY.Value,
                sliderShootDIrectionZ.Value);
        }

        private void sliderShootForeceFactor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            game.ShootForceFactor = sliderShootForeceFactor.Value;
        }

        private void ShootButton_Click(object sender, RoutedEventArgs e)
        {
            game.ShootWhiteBallAndContinueGameLoop();
        }
    }
}
