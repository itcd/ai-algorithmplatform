using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PositionSetEditer;
using PositionSetViewer;
using Position_Interface;
using Position_Implement;
using Configuration;

namespace TestForPositionSetEditer
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestBotton_Click(object sender, EventArgs e)
        {
            LayersExOptDlg layers = new LayersExOptDlg();
            RandomPositionSet_InFixedDistribution randomPositionSet1 = new RandomPositionSet_InFixedDistribution
                (10000, distributionStyle.GaussianDistribution);
            new ConfiguratedByForm(randomPositionSet1);
            randomPositionSet1.Produce();

            layers.Add(new Layer_PositionSet_Point(randomPositionSet1));

            LayersEditerForm positionSetEditerForm = new LayersEditerForm(layers);
            positionSetEditerForm.Show();
        }
    }
}