using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M2M.Position;
using M2M.Util;
using Position_Implement;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;
using Position3DSet = System.Collections.Generic.List<M2M.Position.IPosition3D>;


namespace TestForGenerator
{
    public partial class LaunchTest : Form
    {
        public LaunchTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RandomPositionSet_InFixedDistribution3D randomPositonSet = new RandomPositionSet_InFixedDistribution3D();
            randomPositonSet.ConfiguratedByGUI();
            PositionSet3D pointSet3D = randomPositonSet.Produce();
            Position3DSet point3DList = new List<IPosition3D>();
            for (int i = 0; i < pointSet3D.Count; i++) {
                point3DList.Add(pointSet3D.ElementAt(i));
            }

            Scene scene = new Scene();
            scene.AddElement(new GridSetElement(point3DList));

            scene.ShowScene();
        }
    }
}
