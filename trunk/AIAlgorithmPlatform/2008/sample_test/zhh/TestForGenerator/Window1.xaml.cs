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
        }

        public void Test1(){
            List<IPosition3D> point3DList = new RandomPositionSet_Square3D(100, 0, 300);

            Scene scene = new Scene();
            scene.AddElement(new GridSetElement(point3DList));

            scene.ShowScene();
        }

        public void Test2(){
        }
    }
}
