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

using Configuration;
using M2M.Media3D;
using M2M.Position;
using M2M.Position.RandomMap;
using M2M.Algorithm.Pathfinding;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.IPosition_Connected>;

namespace Map_Path_3D
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private GeometryModel3D mGeometry;
        private bool mDown;
        private Point mLastPos;
        MapModel mapModel = new MapModel();
        IPosition_ConnectedSet set = null;

        public Window1()
        {
            InitializeComponent();

            group.Transform = new Transform3DGroup();
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

                Transform3DGroup tg = group.Transform as Transform3DGroup;
                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                tg.Children.Add(new RotateTransform3D(r));

                mLastPos = actualPos;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            mDown = true;
            Point pos = Mouse.GetPosition(viewport);
            mLastPos = new Point(pos.X - viewport.ActualWidth / 2, viewport.ActualHeight / 2 - pos.Y);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, 5);
            group.Transform = new Transform3DGroup();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ////产生随机迷宫(数组)：
            //RandomMaze_Config config = new RandomMaze_Config();
            //new ConfiguratedByForm(config);
            //config.Produce();
            //if (config.RandomMapStyle == RandomMapStyle.RandomMaze)
            //    mapModel.generateMap(group, config.Map, config.Width, config.Height);
            //else
            //    mapModel.generateMap3D(group, config.Map3D, config.Width, config.Height, config.Depth);

            //产生随机迷宫(IPosition_Connected)：
            RandomMaze_IPosition_Connected_Config config = new RandomMaze_IPosition_Connected_Config();
            new ConfiguratedByForm(config);
            set = config.Produce();

            group.Children.Clear();
            mapModel.generateMap_from_IPosition_ConnectedSet(group, set);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            group.Children.Clear();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (set == null)
                return;

            IPosition_Connected a = null, b = null;
            foreach (IPosition_Connected p in set)
            {
                if (a == null)
                    a = p;
                b = p;
            }

            SearchPathEngine search = new Dijkstra();
            search.InitEngineForMap(set);
            IPosition_ConnectedSet path = search.SearchPath(a, b);

            if (path != null)
                foreach (IPosition_Connected p in path)
                    p.SetTagIndex(MapModel.PATH_ID);

            group.Children.Clear();
            mapModel.generateMap_from_IPosition_ConnectedSet(group, set);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (set == null)
                return;

            IPosition_Connected a = null, b = null;
            foreach (IPosition_Connected p in set)
            {
                if (a == null)
                    a = p;
                b = p;
            }

            SearchPathEngine search = new AStar();
            search.InitEngineForMap(set);
            IPosition_ConnectedSet path = search.SearchPath(a, b);

            if (path != null)
                foreach (IPosition_Connected p in path)
                    p.SetTagIndex(MapModel.PATH_ID);

            group.Children.Clear();
            mapModel.generateMap_from_IPosition_ConnectedSet(group, set);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (set == null)
                return;

            IPosition_Connected a = null, b = null;
            foreach (IPosition_Connected p in set)
            {
                if (a == null)
                    a = p;
                b = p;
            }

            SearchPathEngine search = new AStar(new ManhattanDistanceEvaluator());
            search.InitEngineForMap(set);
            IPosition_ConnectedSet path = search.SearchPath(a, b);

            if (path != null)
                foreach (IPosition_Connected p in path)
                    p.SetTagIndex(MapModel.PATH_ID);

            group.Children.Clear();
            mapModel.generateMap_from_IPosition_ConnectedSet(group, set);
        }

    }
}
