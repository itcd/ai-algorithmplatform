using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Implement;
using Position_Interface;
using PositionSetViewer;
using PositionSetEditer;
using Position_Connected_Interface;
using DataStructure;
using Configuration;
using M2M;
using Position_Connected_Implement;

namespace TestForM2MPF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void TestBotton_Click(object sender, EventArgs e)
        {
            PainterDialog painterDialog = new PainterDialog();

            RandomPositionSet_Connected_Config config = new RandomPositionSet_Connected_Config();
            new ConfiguratedByForm(config);
            IPositionSet_ConnectedEdit pSet = config.Produce();

            painterDialog.DrawPositionSet_Connected(pSet);

            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();

            m2m_Creater_ForGeneralM2MStruture.PartType = typeof(Part_Multi);
            m2m_Creater_ForGeneralM2MStruture.SetPointInPartFactor(10);
            m2m_Creater_ForGeneralM2MStruture.SetUnitNumInGridLength(4);

            IM2MStructure m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(pSet);
            m2mStructure.Preprocessing(pSet);
            painterDialog.DrawM2MStructure(m2mStructure);

            BuildPartSetConnectionForM2MStructure buildPartSetConnectionForM2MStructure = new BuildPartSetConnectionForM2MStructure();

            buildPartSetConnectionForM2MStructure.GetPartSetInSpecificLevel += delegate(ILevel level, int levelSequence, IPositionSet positionSet) {
                List<IPosition_Connected> position_ConnectedList = new List<IPosition_Connected>();

                positionSet.InitToTraverseSet();
                while(positionSet.NextPosition())
                {
                    IPart_Multi partMulti = (IPart_Multi)positionSet.GetPosition();
                    IEnumerable<IPart_Connected> part_ConnectedEnumerable = partMulti.GetSubPartSet();

                    foreach (IPart_Connected part in part_ConnectedEnumerable)
                    {
                        position_ConnectedList.Add((IPosition_Connected)part);
                    }
                }

                PositionSet_Connected positionSet_Connected = new PositionSet_Connected(position_ConnectedList);
                Layer_PositionSet_Connected layer_PositionSet_Connected = new Layer_PositionSet_Connected(positionSet_Connected);
                layer_PositionSet_Connected.SetPositionSetTransformByM2MLevel(level);
                painterDialog.Layers.Add(layer_PositionSet_Connected);

                painterDialog.Show();
            };

            buildPartSetConnectionForM2MStructure.TraversalEveryLevelAndBuild(m2mStructure);

            for (int levelSequence = 1; levelSequence < m2mStructure.GetLevelNum(); levelSequence++)
            {
                ILevel level = m2mStructure.GetLevel(levelSequence);
                IPart rootPart = m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0);
                IPositionSet positionSet = m2mStructure.GetDescendentPositionSetByAncestorPart(levelSequence, rootPart, 0);

                List<IPosition_Connected> position_ConnectedList = new List<IPosition_Connected>();
                positionSet.InitToTraverseSet();
                while (positionSet.NextPosition())
                {
                    if (positionSet.GetPosition() is IPart_Multi)
                    {
                        IPart_Multi partMulti = (IPart_Multi)positionSet.GetPosition();
                        IEnumerable<IPart_Connected> part_ConnectedEnumerable = partMulti.GetSubPartSet();

                        foreach (IPart_Connected part in part_ConnectedEnumerable)
                        {
                            position_ConnectedList.Add((IPosition_Connected)part);
                        }
                    }
                    else
                    {
                        position_ConnectedList.Add((IPosition_Connected)positionSet.GetPosition());
                    }
                }
                PositionSet_Connected positionSet_Connected = new PositionSet_Connected(position_ConnectedList);

                Layer_PositionSet_Connected layer_PartSet_Connected = new Layer_PositionSet_Connected(positionSet_Connected);
                //layer_PartSet_Connected.MainColor
                //layer_PartSet_Connected.Active = true;
                layer_PartSet_Connected.SetPositionSetTransformByM2MLevel(level);

                painterDialog.Layers.Add(layer_PartSet_Connected);
            }

            painterDialog.Show();
            //painterDialog.Show();
        }
    }
}