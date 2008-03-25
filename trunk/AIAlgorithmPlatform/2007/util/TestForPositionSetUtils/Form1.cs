using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Position_Interface;
using Position_Implement;
using Troschuetz.Random;
using RandomMakerAlgorithm;
using QuickHullAlgorithm;
using PositionSetViewer;
using Configuration;

namespace PositionSetUtils
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PainterDialog painterDialog = new PainterDialog();

            RandomPositionSet_InFixedDistribution randomPositionSet = new RandomPositionSet_InFixedDistribution();

            new ConfiguratedByForm(randomPositionSet);
            randomPositionSet.Produce();

            QuickHull quickHull = new QuickHull();
            IPositionSet convexHull = quickHull.ConvexHull(randomPositionSet);

            painterDialog.Clear();
            painterDialog.DrawConvexHull(convexHull);

            IPosition p = PositionSetAttribute.GetGravityCenter(convexHull);
            List<IPosition> pl = new List<IPosition>();
            pl.Add(p);
            painterDialog.DrawPath(new PositionSet_ImplementByIEnumerableTemplate(pl));
        }
    }
}