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

        private void StartBotton_Click(object sender, RoutedEventArgs e)
        {
            gameWindows.Show();
            Initializtion();
            timerPump = new TimerPump() { InvokedMethod = PerformOneFrame};            
            timerPump.BeginPumpWithTimeInterval(40);
        }

        private void StopBotton_Click(object sender, RoutedEventArgs e)
        {
            timerPump.StopPump();
        }
    }

    public class PhysicEngine
    {
        public static void UpdatePositionByVelocity(ref Point3D position, ref Vector3D velocity, double t)
        {
            position.X += velocity.X * t;
            position.Y += velocity.Y * t;
            position.Z += velocity.Z * t;
        }

        public static void UpdateVelocityByForce(ref Vector3D velocity, ref Vector3D force, double mass, double t)
        {
 
        }
    }

    public class CollisionDetectionEngine
    {
        static Dictionary<int, int>[, ,] gridMap;
        static List<Molecule> MoleculeSet;
        static int length, width, height, gridLength;

        public static void InitCollisionDetectionEngine(List<Molecule> mset, int l, int w, int h, int gl)
        {
            length = l;
            width = w;
            height = h;

            MoleculeSet = mset;
            gridMap = new Dictionary<int, int>[length, width, height];

            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                Dictionary<int, int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
                if (grid == null) grid = new Dictionary<int, int>();
                grid.Add(i, i);


            }
        }

        public static void UpdateGridmap()
        {
            for (int i = 0; i < MoleculeSet.Count; i++)
            {
                Molecule m = MoleculeSet[i];

                if ((int)(m.oldPosition.X / gridLength) == (int)(m.oldPosition.X / gridLength) &&
                    (int)(m.oldPosition.Y / gridLength) == (int)(m.oldPosition.Y / gridLength) &&
                    (int)(m.oldPosition.Z / gridLength) == (int)(m.oldPosition.Z / gridLength))
                    continue;


                Dictionary<int, int> oldGrid = gridMap[(int)(m.oldPosition.X / gridLength), (int)(m.oldPosition.Y / gridLength), (int)(m.oldPosition.Z / gridLength)];
                oldGrid.Remove(i);

                Dictionary<int, int> grid = gridMap[(int)(m.position.X / gridLength), (int)(m.position.Y / gridLength), (int)(m.position.Z / gridLength)];
                if (grid == null) grid = new Dictionary<int, int>();
                grid.Add(i, i);
            }
        }

        public static void CollisionDetection()
        {
            for (int index = 0; index < MoleculeSet.Count; index++)
            {
                Molecule m = MoleculeSet[index];

                int x = (int)m.position.X / gridLength;
                int y = (int)m.position.Y / gridLength;
                int z = (int)m.position.Z / gridLength;

                Dictionary<int, int> grid = gridMap[x, y, z];

                for (int i = (x - 1) > 0 ? (x - 1) : 0; i <= ((x + 1) < (length - 1) ? (x + 1) : (length - 1)); i++)
                    for (int j = (y - 1) > 0 ? (y - 1) : 0; i <= ((y + 1) < (width - 1) ? (y + 1) : (width - 1)); j++)
                        for (int k = (z - 1) > 0 ? (z - 1) : 0; i <= ((z + 1) < (height - 1) ? (z + 1) : (height - 1)); k++)
                        {
                            Dictionary<int, int> neighborGrid = gridMap[i, j, k];
                            if (neighborGrid != null)
                            {
                                for (int l = 0; l < neighborGrid.Count; l++)
                                {
                                    int neighborIndex = neighborGrid.ElementAt(l).Key;
                                    if (neighborIndex <= index) continue;
                                    //Collide(index,neighborIndex);
                                }
                            }

                        }
            }
        }
    }

    public class Molecule
    {
        public double radius;

        public Point3D position;

        public Point3D oldPosition;

        public Vector3D currentVelocity;

        double mass;

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

        Thread thread = null;
        double timeInterval = 0;

        ~TimerPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();                
            }
        }

        void Run()
        {
            while (true)
            {
                TimeSpan sw = new TimeSpan();

                DateTime start = DateTime.Now;
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, invokeByTimerMethod, timeInterval);
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
