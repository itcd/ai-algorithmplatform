using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Configuration;
using PositionSetViewer;
using Position_Interface;
using BlockLineAlgorithm;
using WriteLineInGridEngine;
using PositionSetDrawer;
using Position_Implement;

namespace TestForWriteLineInGridAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class ConfigurationForWriteLineInGridAlgorithm
        {
            float gridWidth = 249.918076f;
            public float GridWidth
            {
                get { return gridWidth; }
                set { gridWidth = value; }
            }

            float gridHeight = 249.918076f;
            public float GridHeight
            {
                get { return gridHeight; }
                set { gridHeight = value; }
            }

            float startPositionX = 920.552856f;
            public float StartPositionX
            {
                get { return startPositionX; }
                set { startPositionX = value; }
            }

            float startPositionY = 839.262146f;
            public float StartPositionY
            {
                get { return startPositionY; }
                set { startPositionY = value; }
            }

            float endPositionX = 879.879f;
            public float EndPositionX
            {
                get { return endPositionX; }
                set { endPositionX = value; }
            }

            float endPositionY = 55.6689f;
            public float EndPositionY
            {
                get { return endPositionY; }
                set { endPositionY = value; }
            }

            [BrowsableAttribute(false)]
            public IPosition StartPosition
            {
                get { return new Position_Point(startPositionX, startPositionY); }
            }

            [BrowsableAttribute(false)]
            public IPosition EndPosition
            {
                get { return new Position_Point(endPositionX, endPositionY); }
            }
        }

        ConfigurationForWriteLineInGridAlgorithm configurationForWriteLineInGridAlgorithm;
        PainterDialog painterDialog = new PainterDialog();

        private void TestBotton_Click(object sender, EventArgs e)
        {
            configurationForWriteLineInGridAlgorithm = new ConfigurationForWriteLineInGridAlgorithm();
            new ConfiguratedByForm(configurationForWriteLineInGridAlgorithm, ShowLine);
            //new ConfiguratedByForm(configurationForWriteLineInGridAlgorithm);
        }

        private void ShowLine()
        {
            IWriteLineInGridEngine writeLineInGridEngine = new Line2Block();
            //IWriteLineInGridEngine writeLineInGridEngine = new Line2Block();

            IPositionSet line = writeLineInGridEngine.WriteLineInGrid(configurationForWriteLineInGridAlgorithm.GridWidth,
                 configurationForWriteLineInGridAlgorithm.GridHeight,
                 configurationForWriteLineInGridAlgorithm.StartPosition,
                 configurationForWriteLineInGridAlgorithm.EndPosition);

            painterDialog.Clear();
            painterDialog.Layers.Add(new Layer_Grid(10, 10, configurationForWriteLineInGridAlgorithm.GridWidth,
                configurationForWriteLineInGridAlgorithm.GridHeight, 0, 0));

            IPositionSetEdit startAndEnd = new PositionSetEdit_ImplementByICollectionTemplate();

            startAndEnd.AddPosition(configurationForWriteLineInGridAlgorithm.StartPosition);
            startAndEnd.AddPosition(configurationForWriteLineInGridAlgorithm.EndPosition);

            painterDialog.HoldOnMode();
            painterDialog.SetShowModeToForm();

            if (line != null)
            {
                Layer_PositionSet_Point layer = new Layer_PositionSet_Point(line);
                layer.Point.PointRadius = 2;
                layer.Point.PointColor = Color.Blue;
                painterDialog.Layers.Add(layer);
            }

            Layer_PositionSet_Path layer2 = new Layer_PositionSet_Path((IPositionSet)startAndEnd);
            layer2.PathLine.LineWidth = 2;
            layer2.PathLine.LineColor = Color.Red;
            painterDialog.Layers.Add(layer2);

            painterDialog.Show();
        }
    }
}