using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;
using Troschuetz.Random;
using PositionSetViewer;
using RandomMakerAlgorithm;
using Position_Implement;

namespace TestForRandomPositionSet
{
    public partial class LaunchTest : Form
    {
        public LaunchTest()
        {
            InitializeComponent();
        }

        private void LaunchTest_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PainterDialog painterDialog = new PainterDialog();

            distributionStyle dStyle = distributionStyle.Uniform;
            int pointNum = 10000;

            RandomPositionSet_InFixedDistribution randomPositionSet = new RandomPositionSet_InFixedDistribution(pointNum, dStyle);

            //new ConfiguratedByForm(randomPositionSet);
            randomPositionSet.ConfiguratedByGUI();
            randomPositionSet.Produce();

            painterDialog.Clear();
            painterDialog.DrawPositionSet(randomPositionSet);
        }
    }
}