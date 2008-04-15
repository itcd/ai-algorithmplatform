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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using Petzold.Media3D;

namespace MolecularThermalmotion
{
    class Game
    {
        GameWindows gameWindows = new GameWindows();

        TimerPump timerPump;

        List<Molecule> MoleculeSet = new List<Molecule>();

        Random random = new Random();
        private double GetRandomInRange(double range)
        {
            return ((random.NextDouble() - 0.5) * 2 * range);
        }

        void Initializtion()
        {
            int MoleculeNum = 100;
            double positionRange = 10;
            double velocityRange = 2;
            double radius = 2;

            //MeshGeometry3D sphere = new SphereMesh().Geometry;

            SphereMesh sphereMesh = new SphereMesh();
            sphereMesh.Slices = 72;
            sphereMesh.Stacks = 36;
            MeshGeometry3D sphere = sphereMesh.Geometry;

            //用于保存所有小球的Model
            Model3DGroup moleculeModel3DGroup = new Model3DGroup();

            Material material = (Material)gameWindows.viewport.Resources["ER_Vector___Glossy_Yellow___MediumMR2"];
            Material specularMaterial = new SpecularMaterial(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 1024);

            for (int i = 0; i < MoleculeNum; i++)
            {
                //创建并初始化molecule的属性
                Molecule molecule = new Molecule()
                {
                    position = new Point3D(GetRandomInRange(positionRange), GetRandomInRange(positionRange), GetRandomInRange(positionRange)),
                    currentVelocity = new Vector3D(GetRandomInRange(velocityRange), GetRandomInRange(velocityRange), GetRandomInRange(velocityRange)),
                    radius = radius
                };

                //创建每个球体的GeometryModel并添加进显示窗口
                MaterialGroup materialGroup = new MaterialGroup();
                DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))));
                materialGroup.Children.Add(diffuseMaterial);
                materialGroup.Children.Add(specularMaterial);
                molecule.MoleculeGeometryModel = new GeometryModel3D(sphere, materialGroup);
                //molecule.MoleculeGeometryModel.BackMaterial = materialGroup;

                molecule.MoleculeGeometryModel.Transform = new TranslateTransform3D(molecule.position.X, molecule.position.Y, molecule.position.Z);

                moleculeModel3DGroup.Children.Add(molecule.MoleculeGeometryModel);

                MoleculeSet.Add(molecule);
            }

            ModelVisual3D moleculeSetModel = new ModelVisual3D();
            moleculeSetModel.Content = moleculeModel3DGroup;
            gameWindows.model.Children.Add(moleculeSetModel);
        }

        void PerformOneFrame(double timeInterval)
        {
            //把单位转换为秒
            double t = timeInterval / 1000;

            //碰撞检测（如果检测到碰撞在这里改变碰撞后小球的速度矢量）

            //通过小球的受力来更新小球的速度（重力，摩擦力，碰撞后瞬间的冲力等）
            UpdateMoleculeSetVelocityByForce(t);

            //用速度来update小球的位置
            UpdateMoleculeSetPositionByVelocity(t);
        }

        /// <summary>
        /// 通过小球的受力来更新小球的速度（重力，摩擦力，碰撞后瞬间的冲力等）
        /// </summary>
        /// <param name="t"></param>
        private void UpdateMoleculeSetVelocityByForce(double t)
        {
            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                m.currentVelocity.Y -= 0.1;

                //if (m.currentVelocity.X == 0 && m.currentVelocity.Y == 0 && m.currentVelocity.Z == 0)
                //{
                //    continue;
                //}
                //else
                //{
                //    //通过小球的速度来更新小球位置
                //    PhysicEngine.UpdatePositionByVelocity(ref m.position, ref m.currentVelocity, t);

                //    //更新小球显示的位置
                //    TranslateTransform3D temp = (TranslateTransform3D)(m.MoleculeGeometryModel.Transform);
                //    temp.OffsetX = m.position.X;
                //    temp.OffsetY = m.position.Y;
                //    temp.OffsetZ = m.position.Z;
                //}
            }
        }

        private void UpdateMoleculeSetPositionByVelocity(double t)
        {
            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];
                if (m.currentVelocity.X == 0 && m.currentVelocity.Y == 0 && m.currentVelocity.Z == 0)
                {
                    continue;
                }
                else
                {
                    //通过小球的速度来更新小球位置
                    PhysicEngine.UpdatePositionByVelocity(ref m.position, ref m.currentVelocity, t);

                    //更新小球显示的位置
                    TranslateTransform3D temp = (TranslateTransform3D)(m.MoleculeGeometryModel.Transform);
                    temp.OffsetX = m.position.X;
                    temp.OffsetY = m.position.Y;
                    temp.OffsetZ = m.position.Z;
                }
            }
        }

        public void StartGame()
        {
            gameWindows.Show();
            Initializtion();
            timerPump = new TimerPump() { InvokedMethod = PerformOneFrame };
            timerPump.BeginPumpWithTimeInterval(40);
        }

        public void StopGame()
        {
            timerPump.StopPump();
        }
    }
}
