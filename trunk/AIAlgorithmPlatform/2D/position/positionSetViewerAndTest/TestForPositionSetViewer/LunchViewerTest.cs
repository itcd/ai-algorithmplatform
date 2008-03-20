using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;
using Position_Implement;
using PositionSetViewer;
using Configuration;
using M2M;

namespace TestForPositionSetViewer
{
    public partial class LunchViewerTest : Form
    {
        public LunchViewerTest()
        {
            InitializeComponent();
        }

        public IPositionSet GetRamSet(int wRank,int hRank)
        {
            List<IPosition> lst = new List<IPosition>();
            Position_Point ipos = null;
            for (int i = 0; i < 100; i++)
            {
                ipos = new Position_Point();
                ipos.SetX(RandomMakerAlgorithm.RandomMaker.RapidBetween(0, wRank));
                ipos.SetY(RandomMakerAlgorithm.RandomMaker.RapidBetween(0, hRank));
                lst.Add(ipos);
            }
            PositionSetEdit_ImplementByICollectionTemplate pSet = new PositionSetEdit_ImplementByICollectionTemplate(lst);
            return pSet;
        }

        public IPositionSet GetRamSet2()
        {
            List<IPosition> lst = new List<IPosition>();
            Position_Point ipos = null;
            ipos = new Position_Point();
            ipos.SetX(10);
            ipos.SetY(20);
            lst.Add(ipos);
            ipos = new Position_Point();
            ipos.SetX(50);
            ipos.SetY(70);
            lst.Add(ipos);
            ipos = new Position_Point();
            ipos.SetX(50);
            ipos.SetY(90);
            lst.Add(ipos);
            ipos = new Position_Point();
            ipos.SetX(25);
            ipos.SetY(30);
            lst.Add(ipos);
            PositionSetEdit_ImplementByICollectionTemplate pSet = new PositionSetEdit_ImplementByICollectionTemplate(lst);
            return pSet;
        }

        PainterDialog painterDialog = new PainterDialog();

        private void TestForViewer(object sender, EventArgs e)
        {
            painterDialog.Show();

            painterDialog.Reset();
            painterDialog.HoldOnMode();

            RandomPositionSet_InFixedDistribution randomPositionSet1 = new RandomPositionSet_InFixedDistribution
                (10000, distributionStyle.GaussianDistribution);
            new ConfiguratedByForm(randomPositionSet1);
            randomPositionSet1.Produce();

            RandomPositionSet_InFixedDistribution randomPositionSet2 = new RandomPositionSet_InFixedDistribution
                (10000, distributionStyle.LaplaceDistribution);
            new ConfiguratedByForm(randomPositionSet2);
            randomPositionSet2.Produce();

            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();
            IM2MStructure m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(randomPositionSet2);
            m2mStructure.Preprocessing(randomPositionSet2);

            painterDialog.Clear();
            painterDialog.DrawPositionSet(randomPositionSet1);
            painterDialog.DrawPositionSet(randomPositionSet2);
            painterDialog.DrawM2MLevel(m2mStructure.GetLevel(m2mStructure.GetLevelNum() - 1));
            painterDialog.DrawM2MStructure(m2mStructure);
            Layer_PositionSet layer = new Layer_PositionSet_Point(GetRamSet2());
            layer.SetPositionSetTransformByM2MLevel(m2mStructure.GetLevel(m2mStructure.GetLevelNum() - 1));
            //painterDialog.Painter.Layers.Add(layer);
            painterDialog.Show();

            painterDialog.Clear();
            painterDialog.DrawPath(GetRamSet(1000, 800));
            painterDialog.DrawPositionSet(GetRamSet(500, 400));
            painterDialog.DrawConvexHull(GetRamSet2());
            painterDialog.DrawSquareFrame(GetRamSet(500, 400));
            painterDialog.Show();

            painterDialog.Reset();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            painterDialog.Clear();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            painterDialog.Hide();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            painterDialog.Show();
        }

        private void justShowAndFocusItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            painterDialog.Show();
        }
    }
}