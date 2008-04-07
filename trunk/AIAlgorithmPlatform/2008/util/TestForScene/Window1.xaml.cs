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
            scene.AddElement(new GridSetElement(point3DList));

            scene.ShowScene();
        }

        private void Test2()
        {
            List<IPosition3D> point3DList = new List<IPosition3D>();

            double range = 100;

            for (int i = 0; i < 8000; i++)
            {
                point3DList.Add(new Position3D(
                    GetRandomInRange(range), GetRandomInRange(range), GetRandomInRange(range)));
            }

            Scene scene = new Scene();
            scene.AddElement(new PositionSetElement(point3DList));

            scene.ShowScene();
        }

        Random random = new Random();
        private double GetRandomInRange(double range)
        {
            return ((random.NextDouble() - 0.5) * 2 * range);
        }
    }
}
