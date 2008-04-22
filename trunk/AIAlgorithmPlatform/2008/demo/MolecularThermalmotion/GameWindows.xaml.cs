﻿using System;
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

            this.game = game;

            model.Transform = new Transform3DGroup();
        }

        public void SetShootDirectionAndForceFactor()
        {
            //这时候使到那些control有效
        }

        private bool mDown;
        private Point mLastPos;

        private GameWindows()
        {
            InitializeComponent();

            model.Transform = new Transform3DGroup();

            this.PreviewKeyDown += new KeyEventHandler(GameWindows_KeyDown);

            this.viewport.PreviewKeyDown += new KeyEventHandler(GameWindows_KeyDown);
        }

        void GameWindows_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) { sliderShootDIrectionX.Value += 0.1; }

            if (e.Key == Key.W) { sliderShootDIrectionX.Value -= 0.1; }
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

                double rotation = 0.002 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

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

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    game.Height = Convert.ToDouble(this.textBox1.Text);
        //}

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