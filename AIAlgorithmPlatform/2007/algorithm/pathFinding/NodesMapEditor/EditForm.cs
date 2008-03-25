using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SearchEngineLibrary;
using DataStructure;
using DrawingLib;
using CountTimeTool;
using Position_Connected_Interface;
using Position_Connected_Implement;

namespace NodesMapEditor
{
    public partial class EditForm : Form
    {
        int x0 = -1, y0 = -1, x1 = -1, y1 = -1;//x0,y0为上一次经过的点，x1,y1为被选择的点
        int blockSize = 4;
        int mapWidth = 240, mapHeight = 160;
        PositionMap map;
        Bitmap bmp;
        Graphics buffer;
        bool connecting = false;
        ISearchPathEngine dijkstra = new Dijkstra();
        ISearchPathEngine astar = new AStar();
        List<IPosition_Connected> path = null;
        PositionInfoForm infoForm = new PositionInfoForm();
        AboutForm aboutForm = new AboutForm();
        InputForm inputForm = new InputForm();
        LineWidthForm lineWidthForm = new LineWidthForm();

        ColorPenSet penSet = new ColorPenSet();
        Pen linePen = new Pen(Color.White);
        Pen nodePen = new Pen(Color.SkyBlue);
        Pen selectedNodePen = new Pen(Color.Yellow);
        Pen pathNodePen = new Pen(Color.Coral);
        Pen selectBoxPen = new Pen(Color.Yellow);

        TimeCounter timeCounter = new TimeCounter();

        //计算算法运行时间用
        IPosition_Connected start;
        IPosition_Connected end;
        ISearchPathEngine searchPathEngine;

        public EditForm()
        {
            InitializeComponent();
            ChangeBlockSize(blockSize);
            ChangeMapSize(mapWidth, mapHeight);
            linePen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            penSet.setEndCap(System.Drawing.Drawing2D.LineCap.ArrowAnchor);
            penSet.setWidth(3);

            timeCounter.setPrecision(1);
            timeCounter.setMinRound(10);
        }

        protected void SearchForPath_countTime()
        {
            searchPathEngine.SearchPath(start, end);
        }

        protected string SearchForPath(ISearchPathEngine searchEngine, bool countTimeEnable)
        {
            start = map.GetStartPosition();
            end = map.GetEndPosition();
            searchPathEngine = searchEngine;
            if (start != null && end != null)
            {
                searchEngine.InitEngineForMap(map.GetPositionSet_Connected());
                path = searchEngine.SearchPath(start, end);
                if (path != null)
                {
                    double length = 0, dx, dy;
                    for (int i = 1; i < path.Count; i++)
                    {
                        dx = path[i].GetX() - path[i - 1].GetX();
                        dy = path[i].GetY() - path[i - 1].GetY();
                        length += Math.Sqrt(dx * dx + dy * dy);
                    }
                    if (countTimeEnable)
                        return string.Format("Time:{0} milliseconds.\nPath length:{1}", timeCounter.CountTimeForRepeatableMethod(SearchForPath_countTime), length);
                    else
                        return string.Format("Path length:{1}", length);
                }
            }
            return "There is not any path between the start point and the end point!";
        }

        protected void ChangeFormSize()
        {
            int width = panel1.Width + 10, height = menuStrip1.Height + toolStrip1.Height + panel1.Height + 36;
            Rectangle r = Screen.GetWorkingArea(this);
            if (width < r.Width && height < r.Height)
            {
                this.Width = width;
                this.Height = height;
            }
            else
                this.WindowState = FormWindowState.Maximized;
        }

        protected void ChangePanelSize(int width, int height)
        {
            panel1.Width = width * blockSize + 1;
            panel1.Height = height * blockSize + 1;
        }

        protected void ChangeBufferSize()
        {
            bmp = new Bitmap(panel1.Width, panel1.Height);
            buffer = Graphics.FromImage(bmp);
            buffer.Clear(panel1.BackColor);
        }

        protected void ChangeMapSize(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
            mapWidthToolStripTextBox.Text = Convert.ToString(mapWidth);
            mapHeightoolStripTextBox.Text = Convert.ToString(mapHeight);
            ChangePanelSize(mapWidth, mapHeight);
            map = new PositionMap(mapWidth, mapHeight, map);
            ChangeBufferSize();
            ChangeFormSize();
        }

        protected void ChangeBlockSize(int size)
        {
            if (size != blockSize)
            {
                blockSize = size;
                blockSizeToolStripTextBox.Text = Convert.ToString(blockSize);
                ChangePanelSize(mapWidth, mapHeight);
                if (panel1.Width > bmp.Width || panel1.Height > bmp.Height)
                {
                    ChangeBufferSize();
                }
                ChangeFormSize();
            }
        }

        protected void AdapteBlockSizeAccordingToMapSize()
        {
            Rectangle r = Screen.GetWorkingArea(this);
            int newBlockSize = blockSize;
            while (newBlockSize > 1 && (map.GetWidth() * newBlockSize > r.Width || map.GetHeight() * newBlockSize > r.Height))
            {
                newBlockSize = newBlockSize / 2;
            }
            if (newBlockSize != blockSize)
                ChangeBlockSize(newBlockSize);
        }

        protected void DrawSelectedNode()
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                if (connecting && x1 >= 0 && y1 >= 0)
                    g.DrawEllipse(selectedNodePen, x1 * blockSize, y1 * blockSize, blockSize, blockSize);
            }
        }

        protected void DrawPathNode(IPosition_Connected p)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                g.DrawEllipse(pathNodePen, (int)p.GetX() * blockSize, (int)p.GetY() * blockSize, blockSize, blockSize);
            }
        }

        protected void drawNode(float x, float y)
        {
            buffer.DrawEllipse(nodePen, x * blockSize, y * blockSize, blockSize, blockSize);
        }

        protected void drawConnection(Pen pen, float x1, float y1, float x2, float y2)
        {
            int halfBolckSize = blockSize / 2;
            buffer.DrawLine(pen, x1 * blockSize + halfBolckSize, y1 * blockSize + halfBolckSize, x2 * blockSize + halfBolckSize, y2 * blockSize + halfBolckSize);
        }

        protected void drawConnection(Pen pen, IPosition_Connected p1, IPosition_Connected p2)
        {
            drawConnection(pen, p1.GetX(), p1.GetY(), p2.GetX(), p2.GetY());
        }

        protected void drawNode_refreshTags(int x, int y)
        {
            drawNode(x, y);
            refreshTags();
        }

        protected void DrawPath()
        {
            if (path != null)
            {
                penSet.setMaxValue(path.Count);
                for (int i = 0; i < path.Count; i++)
                {
                    DrawPathNode(path[i]);
                    if (i > 0 && colorThePathToolStripMenuItem.Checked)
                        drawConnection(penSet.getPen(i), path[i - 1], path[i]);
                }
            }
        }

        protected void DrawStartEndNode()
        {
            IPosition_Connected_Edit p;
            p = map.GetStartPosition();
            if (p != null)
                DrawPathNode(p);
            p = map.GetEndPosition();
            if (p != null)
                DrawPathNode(p);
        }

        protected void refreshTags()
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                g.DrawImage(bmp, 0, 0);
                DrawPath();
                DrawStartEndNode();
                DrawSelectedNode();
            }
        }

        protected void drawMap()
        {
            buffer.Clear(panel1.BackColor);

            IPosition_Connected_Edit adj;
            //先画节点间连接，再画节点，让节点覆盖在连接上，看起来比较清晰
            foreach (IPosition_Connected_Edit p in map.GetPositionList())
            {
                IPositionSet_Connected_AdjacencyEdit adjSet = p.GetAdjacencyPositionSetEdit();
                adjSet.InitToTraverseSet();
                while (adjSet.NextPosition())
                {
                    adj = adjSet.GetPosition_Connected_Edit();
                    drawConnection(linePen, p.GetX(), p.GetY(), adj.GetX(), adj.GetY());
                }
            }
            foreach (IPosition_Connected_Edit p in map.GetPositionList())
            {
                drawNode(p.GetX(), p.GetY());
            }

            refreshTags();
        }

        protected void refreshBlock(int x, int y)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                Rectangle r = new Rectangle(x * blockSize, y * blockSize, blockSize + 1, blockSize + 1);
                if (!pictureBox1.Visible)
                    g.DrawImage(bmp, r, r, GraphicsUnit.Pixel);
                else
                    g.DrawImage(pictureBox1.Image, r, r, GraphicsUnit.Pixel);
            }
        }

        protected void ClearPath()
        {
            if (path != null)
                path = null;
        }

        protected void ClearTags()
        {
            ClearPath();
            x0 = y0 = x1 = y1 = -1;
        }

        protected void ClearMap()
        {
            map.Clear();
            ClearTags();
        }

        public void SaveMapToFile(string filename)
        {
            map.ClearAttachment();
            //Create the file.
            using (FileStream fs = File.Create(filename))
            {
                map.ClearArray();//清空数组内容再存储，以节省存储空间
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, map);
                fs.Close();
                map.RestoreArrayFromList();//从List恢复内容到数组
            }
        }

        public void LoadMapFromFile(string filename)
        {
            //Open the stream and read it back.
            using (FileStream fs = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                PositionMap newMap = (PositionMap)formatter.Deserialize(fs);
                if (newMap != null)
                {
                    map = newMap;
                    map.RestoreArrayFromList();//从List恢复内容到数组
                    mapWidth = map.GetWidth();
                    mapHeight = map.GetHeight();
                    ChangeMapSize(mapWidth, mapHeight);
                    AdapteBlockSizeAccordingToMapSize();
                    ClearTags();
                    drawMap();
                }
                fs.Close();
            }
        }

        public void SavePathToFile(string filename)
        {
            if (path != null)
            {
                foreach (IPosition_Connected p in path)
                {
                    p.SetAttachment(null);
                }
                //Create the file.
                using (FileStream fs = File.Create(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, path);
                    fs.Close();
                }
            }
        }

        public void LoadPathFromFile(string filename)
        {
            //Open the stream and read it back.
            using (FileStream fs = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                List<IPosition_Connected> newPath = (List<IPosition_Connected>)formatter.Deserialize(fs);
                if (newPath != null)
                {
                    path = newPath;
                    refreshTags();
                }
                fs.Close();
            }
        }

        public PositionMap GetPositionMap()
        {
            return map;
        }

        public List<IPosition_Connected> GetPath()
        {
            return path;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / blockSize, y = e.Y / blockSize;
            toolStripLabel1.Text = "(" + Convert.ToString(x) + "," + Convert.ToString(y) + ") (" + Convert.ToString(e.X) + "," + Convert.ToString(e.Y) + ")";
            if (x0 != x || y0 != y)
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    if (x0 >= 0 && y0 >= 0)
                    {
                        refreshBlock(x0, y0);
                        if (Math.Abs(x1 - x0) <= 1 && Math.Abs(y1 - y0) <= 1)
                            DrawSelectedNode();
                        IPosition_Connected_Edit p1 = map.GetStartPosition(), p2 = map.GetEndPosition();
                        if ((p1 != null && Math.Abs(p1.GetX() - x0) <= 1 && Math.Abs(p1.GetY() - y0) <= 1) || (p2 != null && Math.Abs(p2.GetX() - x0) <= 1 && Math.Abs(p2.GetY() - y0) <= 1))
                            DrawStartEndNode();
                        if (path != null)
                            foreach (IPosition_Connected p in path)
                            {
                                if (Math.Abs(p.GetX() - x0) <= 1 && Math.Abs(p.GetY() - y0) <= 1)
                                {
                                    DrawPathNode(p);
                                }
                            }
                    }
                    g.DrawRectangle(selectBoxPen, x * blockSize, y * blockSize, blockSize, blockSize);
                }
                x0 = x;
                y0 = y;
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / blockSize, y = e.Y / blockSize;
            if (nodeToolStripButton.Checked)
            {
                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    if (!map.Exists(x, y))
                    {
                        map.AddPosition(x, y);
                        drawNode_refreshTags(x, y);
                    }
                }
                return;
            }
            if (connectionToolStripButton.Checked)
            {
                if (connecting)
                {
                    connecting = false;
                    if (map.Exists(x, y) && map.Exists(x1, y1) && (x != x1 || y != y1))
                    {
                        map.AddConnection(x1, y1, x, y, (float)Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1)));
                        ClearPath();
                        drawMap();
                    }
                    else
                        refreshBlock(x1, y1);
                }
                else
                {
                    if (map.Exists(x, y))
                    {
                        x1 = x;
                        y1 = y;
                        connecting = true;
                    }
                }
                return;
            }
            if (doubleConnectionToolStripButton.Checked)
            {
                if (connecting)
                {
                    connecting = false;
                    if (map.Exists(x, y) && map.Exists(x1, y1) && (x != x1 || y != y1))
                    {
                        map.AddDoubleConnection(x1, y1, x, y, (float)Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1)));
                        ClearPath();
                        drawMap();
                    }
                    else
                        refreshBlock(x1, y1);
                }
                else
                {
                    if (map.Exists(x, y))
                    {
                        x1 = x;
                        y1 = y;
                        connecting = true;
                    }
                }
                return;
            }
            if (eraserToolStripButton.Checked)
            {
                if (map.Exists(x, y))
                {
                    IPosition_Connected p = map.GetPosition(x, y);
                    if (p == map.GetStartPosition())
                        map.ClearStartPosition();
                    if (p == map.GetEndPosition())
                        map.ClearEndPosition();
                    map.RemovePosition(x, y);
                    ClearPath();
                    drawMap();
                }
                return;
            }
            if (startPositionToolStripButton.Checked)
            {
                if (map.Exists(x, y))
                {
                    map.SetStartPosition(x, y);
                    ClearPath();
                    refreshTags();
                }
                return;
            }
            if (endPositionToolStripButton.Checked)
            {
                if (map.Exists(x, y))
                {
                    map.SetEndPosition(x, y);
                    ClearPath();
                    refreshTags();
                }
                return;
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int size = blockSize * 2;
            if (size < short.MaxValue)
            {
                ChangeBlockSize(size);
                blockSizeToolStripTextBox.Text = Convert.ToString(blockSize);
                drawMap();
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            int size = blockSize / 2;
            if (size > 0)
            {
                ChangeBlockSize(size);
                blockSizeToolStripTextBox.Text = Convert.ToString(blockSize);
                drawMap();
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int size = Convert.ToInt32(blockSizeToolStripTextBox.Text);
                if (size > 0 && size < short.MaxValue)
                {
                    ChangeBlockSize(size);
                    drawMap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                blockSizeToolStripTextBox.Text = Convert.ToString(blockSize);
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                int width = Convert.ToInt32(mapWidthToolStripTextBox.Text);
                int height = Convert.ToInt32(mapHeightoolStripTextBox.Text);
                if ((width != mapWidth || height != mapHeight) && width > 0 && width < short.MaxValue && height > 0 && height < short.MaxValue)
                {
                    ChangeMapSize(width, height);
                    drawMap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mapWidthToolStripTextBox.Text = Convert.ToString(mapWidth);
                mapHeightoolStripTextBox.Text = Convert.ToString(mapHeight);
            }
        }

        private void doubleSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = Convert.ToInt32(mapWidthToolStripTextBox.Text) * 2;
            int height = Convert.ToInt32(mapHeightoolStripTextBox.Text) * 2;
            if (width < short.MaxValue && height < short.MaxValue)
            {
                mapWidthToolStripTextBox.Text = width.ToString();
                mapHeightoolStripTextBox.Text = height.ToString();
                toolStripSplitButton1_ButtonClick(sender, e);
            }
        }

        private void halfSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = Convert.ToInt32(mapWidthToolStripTextBox.Text) / 2;
            int height = Convert.ToInt32(mapHeightoolStripTextBox.Text) / 2;
            if (width > 0 && height > 0)
            {
                mapWidthToolStripTextBox.Text = width.ToString();
                mapHeightoolStripTextBox.Text = height.ToString();
                toolStripSplitButton1_ButtonClick(sender, e);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutForm.ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            IPosition_Connected_Edit p, adj;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Position:\tAdjacency:");
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    if (map.Exists(i, j))
                    {
                        p = map.GetPosition(i, j);
                        sb.Append(p.ToString());
                        sb.Append("\t");
                        IPositionSet_Connected_AdjacencyEdit adjSet = p.GetAdjacencyPositionSetEdit();
                        adjSet.InitToTraverseSet();
                        while (adjSet.NextPosition())
                        {
                            adj = adjSet.GetPosition_Connected_Edit();
                            sb.Append(adj.ToString() + ":" + adjSet.GetDistanceToAdjacency().ToString() + "\t");
                        }
                        sb.AppendLine();
                    }
                }
            }
            IPosition_Connected start = map.GetStartPosition();
            IPosition_Connected end = map.GetEndPosition();
            if (start != null)
                sb.Append("Start position:" + start.ToString());
            if (end != null)
                sb.Append("End position:" + end.ToString());
            sb.AppendLine(SearchForPath(astar, false));
            if (path != null)
            {
                sb.AppendLine();
                sb.AppendLine("Path length:" + path.Count);
                foreach (IPosition_Connected iter in path)
                {
                    sb.Append(iter.ToString() + iter.GetAttachment().ToString() + "\t");
                }
            }
            infoForm.SetText(sb.ToString());
            infoForm.Show();
            infoForm.Activate();
            if (colorThePathToolStripMenuItem.Checked)
                drawMap();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string result = SearchForPath(dijkstra, true);
            if (colorThePathToolStripMenuItem.Checked)
                drawMap();
            MessageBox.Show(result);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string result = SearchForPath(astar, true);
            if (colorThePathToolStripMenuItem.Checked)
                drawMap();
            MessageBox.Show(result);
        }

        private void pointerToolStripButton_Click(object sender, EventArgs e)
        {
            pointerToolStripButton.Checked = false;
            nodeToolStripButton.Checked = false;
            connectionToolStripButton.Checked = false;
            doubleConnectionToolStripButton.Checked = false;
            eraserToolStripButton.Checked = false;
            startPositionToolStripButton.Checked = false;
            endPositionToolStripButton.Checked = false;
            ((ToolStripButton)sender).Checked = true;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveMapDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveMapDialog.FileName.CompareTo("") != 0)
                    SaveMapToFile(saveMapDialog.FileName);
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (loadMapDialog.ShowDialog() == DialogResult.OK)
            {
                if (loadMapDialog.FileName.CompareTo("") != 0)
                    LoadMapFromFile(loadMapDialog.FileName);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            refreshTags();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripButton_Click(object sender, EventArgs e)
        {
            ClearMap();
            drawMap();
        }

        private void pictureBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = pictureBoxToolStripMenuItem.Checked;
        }

        private void toolStripSeparator2_Click(object sender, EventArgs e)
        {
            pictureBoxToolStripMenuItem.Visible = !pictureBoxToolStripMenuItem.Visible;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                if (savePathDialog.ShowDialog() == DialogResult.OK)
                {
                    if (savePathDialog.FileName.CompareTo("") != 0)
                        SavePathToFile(savePathDialog.FileName);
                }
            }
            else
                MessageBox.Show("Please search for a path first.");
        }

        private void generateFullMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTags();
            map.GenerateFullMap();
            drawMap();
        }

        private void generateMazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearTags();
            map.GenerateRandomMaze();
            drawMap();
        }

        private void randomizeMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                ClearTags();
                map.GenerateRandomMap(inputForm.TotalAmount, inputForm.Amount1, inputForm.Amount2, inputForm.Amount3, inputForm.Probability1, inputForm.Probability2, inputForm.Probability3);
                drawMap();
            }
        }

        private void randomizePositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                ClearTags();
                map.GenerateRandomPositions(inputForm.TotalAmount, inputForm.Amount1, inputForm.Amount2, inputForm.Amount3, inputForm.Probability1, inputForm.Probability2, inputForm.Probability3);
                drawMap();
            }
        }

        private void colorThePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMap();
            if (colorThePathToolStripMenuItem.Checked)
                refreshTags();
        }

        private void refreshRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawMap();
            if (colorThePathToolStripMenuItem.Checked)
                refreshTags();
        }

        private void lineColorLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = linePen.Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                linePen.Color = colorDialog1.Color;
        }

        private void nodeColorNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = nodePen.Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                nodePen.Color = colorDialog1.Color;
        }

        private void pathNodeColorPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = pathNodePen.Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pathNodePen.Color = colorDialog1.Color;
        }

        private void selectedNodeColorSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = selectedNodePen.Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                selectedNodePen.Color = colorDialog1.Color;
        }

        private void selectBoxColorBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = selectBoxPen.Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                selectBoxPen.Color = colorDialog1.Color;
        }

        private void lineWidthSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineWidthForm.NodeLineWidth = nodePen.Width;
            lineWidthForm.PathNodeLineWidth = pathNodePen.Width;
            lineWidthForm.SelectedNodeLineWidth = selectedNodePen.Width;
            lineWidthForm.ConnectionLineWidth = linePen.Width;
            lineWidthForm.PathConnectionLineWidth = penSet.getWidth();

            if (lineWidthForm.ShowDialog() == DialogResult.OK)
            {
                nodePen.Width = lineWidthForm.NodeLineWidth;
                pathNodePen.Width = lineWidthForm.PathNodeLineWidth;
                selectedNodePen.Width = lineWidthForm.SelectedNodeLineWidth;
                linePen.Width = lineWidthForm.ConnectionLineWidth;
                penSet.setWidth(lineWidthForm.PathConnectionLineWidth);
            }
        }
    }
}
