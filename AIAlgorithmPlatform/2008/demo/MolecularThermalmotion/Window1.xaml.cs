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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            this.Closed += delegate { if (timerPump != null) { timerPump.AbortPump(); } };
        }

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
            int MoleculeNum = 1000;
            double positionRange = 100;
            double velocityRange = 10;
            double radius = 2;

            MeshGeometry3D sphere = new SphereMesh().Geometry;

            //用于保存所有小球的Model
            Model3DGroup moleculeModel3DGroup = new Model3DGroup();

            for (int i = 0; i < MoleculeNum; i++)
            {
                //创建并初始化molecule的属性
                Molecule molecule = new Molecule()
                {
                    Position = new Point3D(GetRandomInRange(positionRange), GetRandomInRange(positionRange), GetRandomInRange(positionRange)),
                    CurrentVelocity = new Vector3D(GetRandomInRange(velocityRange), GetRandomInRange(velocityRange), GetRandomInRange(velocityRange)),
                    Radius = radius
                };                

                //创建每个球体的GeometryModel并添加进显示窗口
                Material material = new SpecularMaterial(new SolidColorBrush(Color.FromRgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))), 3);
                molecule.MoleculeGeometryModel = new GeometryModel3D(sphere, material);
                molecule.MoleculeGeometryModel.Transform = new TranslateTransform3D(molecule.Position.X, molecule.Position.Y, molecule.Position.Z);

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

            //用速度来update小球的位置
            UpdateMoleculeSetPositionByVelocity(t);
        }

        private void UpdateMoleculeSetPositionByVelocity(double t)
        {
            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];
                if (m.CurrentVelocity.X == 0 && m.CurrentVelocity.Y == 0 && m.CurrentVelocity.Z == 0)
                {
                    continue;
                }
                else
                {
                    //更新小球位置
                    m.position.X += m.CurrentVelocity.X * t;
                    m.position.Y += m.CurrentVelocity.Y * t;
                    m.position.Z += m.CurrentVelocity.Z * t;

                    //更新小球显示的位置
                    TranslateTransform3D temp = (TranslateTransform3D)(m.MoleculeGeometryModel.Transform);
                    temp.OffsetX = m.Position.X;
                    temp.OffsetY = m.Position.Y;
                    temp.OffsetZ = m.Position.Z;
                }
            }
        }

        private void StartBotton_Click(object sender, RoutedEventArgs e)
        {
            gameWindows.Show();
            Initializtion();
            timerPump = new TimerPump() { InvokedMethod = PerformOneFrame, Dispatcher = this.Dispatcher };            
            timerPump.BeginPumpWithTimeInterval(40);
        }

        private void StopBotton_Click(object sender, RoutedEventArgs e)
        {
            timerPump.StopPump();
        }
    }

    class Molecule
    {
        double radius;
        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Point3D position;
        public Point3D Position
        {
            get { return position; }
            set { position = value; }
        }

        Vector3D currentVelocity;
        public Vector3D CurrentVelocity
        {
            get { return currentVelocity; }
            set { currentVelocity = value; }
        }

        double mass;
        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        GeometryModel3D moleculeGeometryModel;
        public GeometryModel3D MoleculeGeometryModel
        {
            get { return moleculeGeometryModel; }
            set { moleculeGeometryModel = value; }
        }
    }
    
    class TimerPump
    {
        public delegate void InvokeByTimerDelegate(double time);

        InvokeByTimerDelegate invokeByTimerMethod = null;
        public InvokeByTimerDelegate InvokedMethod
        {
            set { invokeByTimerMethod = value; }
        }

        Dispatcher dispatcher = null;
        public Dispatcher Dispatcher
        {
            set { dispatcher = value; }
        }

        Thread thread = null;
        double timeInterval = 0;

        ~TimerPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();                
            }

            //dispatcher.DisableProcessing();
        }

        void Run()
        {
            while (true)
            {
                TimeSpan sw = new TimeSpan();

                DateTime start = DateTime.Now;
                dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, invokeByTimerMethod, timeInterval);
                sw = DateTime.Now - start;

                double elapsedTime = sw.TotalMilliseconds;

                if (elapsedTime < timeInterval)
                {
                    Thread.Sleep((int)(timeInterval - elapsedTime));
                }
            }
        }

        /// <summary>
        /// BeginPumpWithTimeInterval
        /// </summary>
        /// <param name="intervalTime">Millisecond</param>
        public void BeginPumpWithTimeInterval(double intervalTime)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                while (thread.IsAlive)
                { }
            }

            this.timeInterval = intervalTime;
            thread = new Thread(new ThreadStart(Run));
            thread.IsBackground = true;
            thread.Start();
        }

        public void StopPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Suspend();
            }
        }

        public void AbortPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }
    }
}
