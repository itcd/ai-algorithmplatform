using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BFS_AStarforGrid;
using DrawingLib;

namespace GridMap_Demo
{
    public partial class GridMapForm : Form
    {
        Map map = new ArrayMap(250, 150);
        BFS bfs = new BFS();
        AStar astar = new AStar();
        MapDrawer mapDrawer = new MapDrawer();
        Bitmap bufferBitmap = null;
        Graphics bufferGraphics = null;
        Canvas canvas = null;
        List<MyPoint> path = null;
        MapMark mapMark = null;

        public GridMapForm()
        {
            InitializeComponent();
            //初始化           
            bufferBitmap = new Bitmap(panel1.Width, panel1.Height);
            bufferGraphics = Graphics.FromImage(bufferBitmap);
            canvas = new Canvas(bufferGraphics);
        }

        protected void drawMap()
        {
            mapDrawer.drawMap(map, canvas);
            refresh();
        }

        protected void drawMapMark()
        {
            mapDrawer.drawMap(mapMark, canvas);
            if (ShowPathToolStripMenuItem.Checked)
                mapDrawer.drawPath(path, mapMark, canvas);
            refresh();
        }

        protected void refresh()
        {
            Graphics g = null;
            try
            {
                g = panel1.CreateGraphics();
                g.DrawImage(bufferBitmap, new Point(0, 0));
            }
            finally
            {
                g.Dispose();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            refresh();
        }

        private void BFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message;
            bfs.init(map);
            if (bfs.search(map))
                message = "结果:找到路径。";
            else
                message = "结果:没有找到路径。";
            mapMark = bfs.getMapMark();
            path = bfs.getPath();
            drawMapMark();
            if (TimeToolStripMenuItem.Checked)
            {
                TickTimer timer = new TickTimer();
                int total = 0;
                for (int i = 0; i < 20; i++)
                {
                    bfs.init(map);
                    timer.start();
                    bfs.search(map);
                    total += timer.getElapsedTime();
                }
                MessageBox.Show("广度优先搜索\n用时:" + Convert.ToString(total / 20.0) + "ms.\n" + message);
            }
        }

        private void AStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message;
            astar.init(map);
            if (astar.search(map))
                message = "结果:找到路径。";
            else
                message = "结果:没有找到路径。";
            mapMark = astar.getMapMark();
            path = astar.getPath();
            drawMapMark();
            if (TimeToolStripMenuItem.Checked)
            {
                TickTimer timer = new TickTimer();
                int total = 0;
                for (int i = 0; i < 20; i++)
                {
                    astar.init(map);
                    timer.start();
                    astar.search(map);
                    total += timer.getElapsedTime();
                }
                MessageBox.Show("A*搜索\n用时:" + Convert.ToString(total / 20.0) + "ms.\n" + message);
            }
        }

        private void RandomMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path != null)
                path.Clear();
            map.clear();
            map.setObstacle();
            drawMap();
        }

        private void RenewMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMap();
        }

        private void ClearMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.clear();
            drawMap();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("算法演示平台。\nCopyright (c) 2006. All rights reserved.");
        }

        private void randomMap2ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (path != null)
                path.Clear();
            map.clear();
            map.setObstacle();
            drawMap();
        }
    }
}