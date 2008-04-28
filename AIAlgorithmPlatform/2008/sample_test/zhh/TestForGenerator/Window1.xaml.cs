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
using M2M.Position;
using Position_Implement;
using M2M.Util;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;
using Position3DSet = System.Collections.Generic.List<M2M.Position.IPosition3D>;

namespace TestForGenerator
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
        }

        public void Test1(){
            PositionSet3D pointSet3D = new RandomPositionSet_Square3D(100, 0, 100);
            Position3DSet point3DSet = new Position3DSet();
            for(int i = 0; i < pointSet3D.Count; i++){
                point3DSet.Add(pointSet3D.ElementAt(i));
            }

            Scene scene = new Scene();
            scene.AddElement(new GridSetElement(point3DSet));

            scene.ShowScene();
        }

        public void Test2(){
        }
    }
}
