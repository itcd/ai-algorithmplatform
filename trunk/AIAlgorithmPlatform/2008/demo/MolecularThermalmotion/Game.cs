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

        CollisionDetectionEngine CDE = new CollisionDetectionEngine();

        int MoleculeNum = 7;
        double positionRange = 50;
        double velocityRange = 60;
        double radius = 6;

        double length = 100;
        double width = 100;
        double height = 100;

        Random random = new Random();
        private double GetRandomInRange(double range)
        {
            return ((random.NextDouble() - 0.5) * 2 * range);
        }

        double TransformToRealX(double x)
        { return x - length / 2; }

        double TransformToRealY(double y)
        { return y - width / 2; }

        double TransformToRealZ(double z)
        { return z - height / 2; }

        double TransformToModelX(double x)
        { return x + length / 2; }

        double TransformToModelY(double y)
        { return y + width / 2; }

        double TransformToModelZ(double z)
        { return z + height / 2; }


        void Initializtion()
        {
            //MeshGeometry3D sphere = new SphereMesh().Geometry;

            SphereMesh sphereMesh = new SphereMesh();
            sphereMesh.Slices = 72;
            sphereMesh.Stacks = 36;
            sphereMesh.Radius = radius+1;
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
                    position = new Point3D(TransformToModelX(GetRandomInRange(positionRange)), TransformToModelY(GetRandomInRange(positionRange)), TransformToModelZ(GetRandomInRange(positionRange))),

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

                molecule.MoleculeGeometryModel.Transform = new TranslateTransform3D(TransformToRealX(molecule.position.X), TransformToRealY(molecule.position.Y), TransformToRealZ(molecule.position.Z));

                moleculeModel3DGroup.Children.Add(molecule.MoleculeGeometryModel);

                MoleculeSet.Add(molecule);
            }

            ModelVisual3D moleculeSetModel = new ModelVisual3D();
            moleculeSetModel.Content = moleculeModel3DGroup;
            gameWindows.model.Children.Add(moleculeSetModel);

            //初始化物理引擎
            CDE.InitCollisionDetectionEngine(MoleculeSet, (int)length, (int)width, (int)height, (int)radius * 2);

            CDE.CollisionResponse += delegate(int index1, int index2)
            {
                //Vector3D v1 = MoleculeSet[index1].currentVelocity;
                //MoleculeSet[index1].currentVelocity = MoleculeSet[index2].currentVelocity;
                //MoleculeSet[index2].currentVelocity = v1;

                PhysicEngine.UpdateVelocityByCollide(MoleculeSet[index1].position, MoleculeSet[index2].position, ref MoleculeSet[index1].currentVelocity, ref MoleculeSet[index2].currentVelocity,
                    MoleculeSet[index1].mass, MoleculeSet[index2].mass, MoleculeSet[index1].radius, MoleculeSet[index2].radius);
            };

            CDE.CollideWithWall += delegate(int index)
            {
                Molecule m = MoleculeSet[index];

                if (m.position.X < m.radius)
                {
                    m.position.X = m.radius;
                    m.currentVelocity.X = -m.currentVelocity.X;
                }
                if (m.position.X > length - m.radius)
                {
                    m.position.X = length - m.radius;
                    m.currentVelocity.X = -m.currentVelocity.X;
                }

                if (m.position.Y < m.radius)
                {
                    m.position.Y = m.radius;
                    m.currentVelocity.Y = -m.currentVelocity.Y;
                }
                if (m.position.Y > width - m.radius)
                {
                    m.position.Y = width - m.radius;
                    m.currentVelocity.Y = -m.currentVelocity.Y;
                }

                if (m.position.Z < m.radius)
                {
                    m.position.Z = m.radius;
                    m.currentVelocity.Z = -m.currentVelocity.Z;
                }
                if (m.position.Z > height - m.radius)
                {
                    m.position.Z = height - m.radius;
                    m.currentVelocity.Z = -m.currentVelocity.Z;
                }
            };

        }

        void PerformOneFrame(double timeInterval)
        {
            //把单位转换为秒
            double t = timeInterval / 1000;

            //碰撞检测（如果检测到碰撞在这里改变碰撞后小球的速度矢量）
            CDE.CollisionDetection();

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
            //for (int i = 0; i < MoleculeSet.Count; i++)
            //{
            //    Molecule m = MoleculeSet[i];

            //    m.currentVelocity.Y -= 0.1;

            //    //if (m.currentVelocity.X == 0 && m.currentVelocity.Y == 0 && m.currentVelocity.Z == 0)
            //    //{
            //    //    continue;
            //    //}
            //    //else
            //    //{
            //    //    //通过小球的速度来更新小球位置
            //    //    PhysicEngine.UpdatePositionByVelocity(ref m.position, ref m.currentVelocity, t);

            //    //    //更新小球显示的位置
            //    //    TranslateTransform3D temp = (TranslateTransform3D)(m.MoleculeGeometryModel.Transform);
            //    //    temp.OffsetX = m.position.X;
            //    //    temp.OffsetY = m.position.Y;
            //    //    temp.OffsetZ = m.position.Z;
            //    //}
            //}
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
                    PhysicEngine.UpdatePositionByVelocity(ref m.position, ref m.oldPosition, ref m.currentVelocity, t);

                    CDE.UpdateToGridmap(i);

                    //更新小球显示的位置
                    TranslateTransform3D temp = (TranslateTransform3D)(m.MoleculeGeometryModel.Transform);
                    temp.OffsetX = TransformToRealX(m.position.X);
                    temp.OffsetY = TransformToRealY(m.position.Y);
                    temp.OffsetZ = TransformToRealZ(m.position.Z);
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
