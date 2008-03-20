using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;
using Configuration;
using PositionSetEditer;
using AlgorithmDemo;
using PositionSetViewer;
using M2M;
using NearestNeighbor;
using PositionSetDrawer;
using Position_Implement;
using Position_Connected_Interface;
using Position_Connected_Implement;
using DataStructure;
using System.Runtime.InteropServices;
using SearchEngineLibrary;

namespace FunctionDemo
{
    public partial class FunctionDemoForm : Form
    {
        //[DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess")]

        //[DllImport("GISMapInterface.DLL")]
        //public static extern int OpenNodeDBF(); 
        //[DllImport("GISMapInterface.DLL")]
        //public static extern double ReadNodeX_COORD(int iRecord);
        //[DllImport("GISMapInterface.DLL")]
        //public static extern double ReadNodeY_COORD(int iRecord);
        //[DllImport("GISMapInterface.DLL")]
        //public static extern int GetNodeRecordCount();

        [DllImport("GISMapInterface.DLL")]
        public static extern int Create(StringBuilder pszDBFFile);
        [DllImport("GISMapInterface.DLL")]
        public static extern void SetCurrentIndex(int index);
        [DllImport("GISMapInterface.DLL")]
        public static extern int GetCurrentIndex(int index);

        ////////////////////////////////Node/////////////////////////////////////
        [DllImport("GISMapInterface.DLL")]
        public static extern int OpenNodeDBF();
        [DllImport("GISMapInterface.DLL")]
        public static extern double ReadNodeX_COORD(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern double ReadNodeY_COORD(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadNodeBEGS_ID(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadNodeENDS_ID(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadNodeBOUNDARY(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int GetNodeRecordCount();
        [DllImport("GISMapInterface.DLL")]
        public static extern void CloseNodeDBF();

        /////////////////////Node Adjacent Table////////////////////////
        [DllImport("GISMapInterface.DLL")]
        public static extern int OpenS_ATTDBF();
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadAdjacentRoadID(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int GetS_ATTRecordCount();
        [DllImport("GISMapInterface.DLL")]
        public static extern void CloseS_ATTDBF();

        ///////////////////////Road////////////////////////////
        [DllImport("GISMapInterface.DLL")]
        public static extern int OpenRoadDBF();
        [DllImport("GISMapInterface.DLL")]
        public static extern double ReadRoadX_COORD(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern double ReadRoadY_COORD(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadRoadFNODE(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadRoadTNODE(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern double ReadRoadLENGTH(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int ReadRoadDIRECTION(int iRecord);
        [DllImport("GISMapInterface.DLL")]
        public static extern int GetRoadRecordCount();
        [DllImport("GISMapInterface.DLL")]
        public static extern void CloseRoadDBF();


        [StructLayout(LayoutKind.Sequential)]
        public class LOGFONT
        {
            public const int LF_FACESIZE = 32;
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LF_FACESIZE)]
            public string lfFaceName;
        }
        
        PositionSetContainer positionSetContainer = null;        

        public FunctionDemoForm()
        {
            InitializeComponent();
            positionSetContainer = new PositionSetContainer(this);
        }

        private void FunctionDemoForm_Load(object sender, EventArgs e)
        { 
        }

        private void createRandomSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //产生随机点集：
            RandomPositionSet_InFixedDistribution randomPositionSet_InFixedDistribution = new RandomPositionSet_InFixedDistribution();
            randomPositionSet_InFixedDistribution.PointNum = 100000;
            randomPositionSet_InFixedDistribution.DistributionStyle = distributionStyle.Uniform;
            new ConfiguratedByForm(randomPositionSet_InFixedDistribution);
            randomPositionSet_InFixedDistribution.Produce();

            positionSetContainer.AddPositionSet(randomPositionSet_InFixedDistribution);
        }

        private void emptyPositionSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PositionSetEdit_ImplementByICollectionTemplate positionSetEdit_ImplementByICollectionTemplate = new PositionSetEdit_ImplementByICollectionTemplate();
            positionSetContainer.AddPositionSet(positionSetEdit_ImplementByICollectionTemplate);
        }

        private void currentPositionSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            positionSetContainer.ShowCurrentPositionSetDlg();
        }

        delegate void dDemoProcess();
        private void m2MCHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //以下代码必须在UI线程中调用，即不能在另开的线程中调用
            LayersExOptDlg layers = new LayersExOptDlg();
            LayersPainterForm layersPainterForm = new LayersPainterForm(layers);
            LayersPaintedControl layersPaintedControl = layersPainterForm.LayersPaintedControl;
            LayersEditedControl layersEditedControl = new LayersEditedControl();
            layersEditedControl.LayersPaintedControl = layersPaintedControl;
            layersPainterForm.Controls.Add(layersEditedControl);
            FlowControlerForm flowControlerForm = new FlowControlerForm();
            layersPainterForm.MdiParent = this;
            layersPainterForm.WindowState = FormWindowState.Maximized;
            layersPainterForm.FormClosing += delegate { flowControlerForm.Close(); };
            layersPainterForm.Show();
            flowControlerForm.Show(layersPainterForm);
           
            //打开一个Worker线程来进行算法流程的演示（否则会阻塞UI线程以至于演示不能进行）
            IAsyncResult result = new dDemoProcess(delegate
            {
                M2M_CH m2m_CH = new M2M_CH();

                //为对象的事件添加事件响应，以进行算法演示
                AlgorithmDemo_M2M_CH algorithmDemo_M2M_CH = new AlgorithmDemo_M2M_CH(m2m_CH, layers, flowControlerForm, layersPaintedControl.Invalidate);

                m2m_CH.ConvexHull(positionSetContainer.GetPositionSet());

                //结束演示（解除事件响应的绑定）
                algorithmDemo_M2M_CH.EndDemo();
            }).BeginInvoke(null, null);
        }

        private void m2MNNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //以下代码必须在UI线程中调用，即不能在另开的线程中调用
            LayersExOptDlg layers = new LayersExOptDlg();
            LayersPainterForm layersPainterForm = new LayersPainterForm(layers);
            LayersPaintedControl layersPaintedControl = layersPainterForm.LayersPaintedControl;
            LayersEditedControl layersEditedControl = new LayersEditedControl();
            layersEditedControl.LayersPaintedControl = layersPaintedControl;
            layersPainterForm.Controls.Add(layersEditedControl);
            FlowControlerForm flowControlerForm = new FlowControlerForm();
            layersPainterForm.MdiParent = this;
            layersPainterForm.WindowState = FormWindowState.Maximized;
            layersPainterForm.FormClosing += delegate { flowControlerForm.Close(); };
            layersPainterForm.Show();
            flowControlerForm.Show(layersPainterForm);

            //打开一个Worker线程来进行算法流程的演示（否则会阻塞UI线程以至于演示不能进行）
            IAsyncResult result = new dDemoProcess(delegate
            {
                M2M_NN m2m_NN = new M2M_NN();

                //为对象的事件添加事件响应，以进行算法演示
                AlgorithmDemo_M2M_NN algorithmDemo_M2M_NN = new AlgorithmDemo_M2M_NN(m2m_NN, layers, flowControlerForm, layersPaintedControl.Invalidate);

                m2m_NN.PreProcess(positionSetContainer.GetPositionSet());
                m2m_NN.NearestNeighbor(layersPaintedControl.GetMouseDoubleChickedRealPosition());

                //结束演示（解除事件响应的绑定）
                algorithmDemo_M2M_NN.EndDemo();
            }).BeginInvoke(null, null);
        }

        private void getPositionSetFormDBFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPositionSet_Connected positionSet_Connected = ConvertDBFToPositionSet_Connected("");

            positionSetContainer.AddPositionSet(positionSet_Connected);
            Layer_PositionSet_Connected layer_PositionSet_Connected = new Layer_PositionSet_Connected(positionSet_Connected);
            PainterDialog painterDialog = new PainterDialog();
            painterDialog.Layers.Add(layer_PositionSet_Connected);
            painterDialog.Show();
        }

        private static IPositionSet_Connected ConvertDBFToPositionSet_Connected(string fileFolder)
        {
            Dictionary<string, Position_Connected_Edit> boundaryPointDictioinary = new Dictionary<string, Position_Connected_Edit>();

            List<IPosition_Connected> position_ConnectedList = new List<IPosition_Connected>();

            int a1 = Create(new StringBuilder("J50F001019\\"));
            int a2 = Create(new StringBuilder("J50F002020\\"));
            int a3 = Create(new StringBuilder("J50F001020\\"));
            int a4 = Create(new StringBuilder("J50F002019\\"));
            

            for (int currentDBFIndex = a1; currentDBFIndex <= a4; currentDBFIndex++)
            {
                SetCurrentIndex(currentDBFIndex);

                if ((OpenNodeDBF() == 1) && (OpenRoadDBF() == 1) && (OpenS_ATTDBF() == 1))
                {
                    int aTTRecordCount = GetS_ATTRecordCount();
                    int roadRecordCount = GetRoadRecordCount();
                    int nodeRecordCount = GetNodeRecordCount();

                    Position_Connected_Edit[] aryPosition_Connected_Edit = new Position_Connected_Edit[nodeRecordCount];

                    for (int i = 0; i < nodeRecordCount; i++)
                    {
                        Position_Connected_Edit point = aryPosition_Connected_Edit[i];

                        //如果之前没有被创建
                        if (point == null)
                        {
                            //如果是边界点
                            if (ReadNodeBOUNDARY(i) != 0)
                            {
                                string str = ReadNodeX_COORD(i).ToString() + ReadNodeY_COORD(i).ToString();
                                //如果没添加进hash表
                                if (!boundaryPointDictioinary.TryGetValue(str, out point))
                                {
                                    point = new Position_Connected_Edit((float)ReadNodeX_COORD(i), (float)ReadNodeY_COORD(i));
                                    aryPosition_Connected_Edit[i] = point;
                                    boundaryPointDictioinary.Add(str, point);
                                }
                            }
                            else
                            {
                                double x = ReadNodeX_COORD(i);
                                double y = ReadNodeY_COORD(i);
                                point = new Position_Connected_Edit((float)x, (float)y);
                                aryPosition_Connected_Edit[i] = point;
                            }
                        }

                        int beginId = ReadNodeBEGS_ID(i);
                        int endId = ReadNodeENDS_ID(i);

                        //if (endId < GetS_ATTRecordCount())
                        {
                            for (int j = beginId; j < endId; j++)
                            {
                                int roadId = ReadAdjacentRoadID(j) - 1;
                                int FNodeId = ReadRoadFNODE(roadId) - 1;
                                int TNodeId = ReadRoadTNODE(roadId) - 1;

                                int aNodeId = (FNodeId == i) ? TNodeId : FNodeId;

                                bool isConnectToAdjacent = false;
                                bool isAdjacentConnectToCurrentPoint = false;

                                if (FNodeId == i)
                                {
                                    if (ReadRoadDIRECTION(roadId) == 1)
                                    {
                                        isConnectToAdjacent = true;
                                        isAdjacentConnectToCurrentPoint = true;
                                    }
                                    else if (ReadRoadDIRECTION(roadId) == 2)
                                    {
                                        isConnectToAdjacent = true;
                                    }
                                    else if (ReadRoadDIRECTION(roadId) == 3)
                                    {
                                        isAdjacentConnectToCurrentPoint = true;
                                    }
                                }
                                else if (TNodeId == i)
                                {
                                    if (ReadRoadDIRECTION(roadId) == 1)
                                    {
                                        isConnectToAdjacent = true;
                                        isAdjacentConnectToCurrentPoint = true;
                                    }
                                    else if (ReadRoadDIRECTION(roadId) == 2)
                                    {
                                        isAdjacentConnectToCurrentPoint = true;
                                    }
                                    else if (ReadRoadDIRECTION(roadId) == 3)
                                    {
                                        isConnectToAdjacent = true;
                                    }
                                }

                                //如果存在当前点到邻接点的路径
                                if (isConnectToAdjacent || isAdjacentConnectToCurrentPoint)
                                {
                                    Position_Connected_Edit aNode = aryPosition_Connected_Edit[aNodeId];
                                    //如果该节点未被创建
                                    if (aNode == null)
                                    {
                                        //如果是边界点
                                        if (ReadNodeBOUNDARY(aNodeId) != 0)
                                        {
                                            string str = ReadNodeX_COORD(aNodeId).ToString() + ReadNodeY_COORD(aNodeId).ToString();
                                            //如果没添加进hash表
                                            if (!boundaryPointDictioinary.TryGetValue(str, out aNode))
                                            {
                                                aNode = new Position_Connected_Edit((float)ReadNodeX_COORD(aNodeId), (float)ReadNodeY_COORD(aNodeId));
                                                aryPosition_Connected_Edit[aNodeId] = aNode;
                                                boundaryPointDictioinary.Add(str, aNode);
                                            }
                                        }
                                        else
                                        {
                                            aNode = new Position_Connected_Edit((float)ReadNodeX_COORD(aNodeId), (float)ReadNodeY_COORD(aNodeId));
                                            aryPosition_Connected_Edit[aNodeId] = aNode;
                                        }
                                    }

                                    if (isConnectToAdjacent)
                                    {
                                        point.GetAdjacencyPositionSetEdit().AddAdjacency(aNode, (float)ReadRoadLENGTH(roadId));
                                    }
                                    if (isAdjacentConnectToCurrentPoint)
                                    {
                                        aNode.GetAdjacencyPositionSetEdit().AddAdjacency(point, (float)ReadRoadLENGTH(roadId));
                                    }
                                }
                            }
                        }

                        position_ConnectedList.Add(point);
                    }

                    CloseNodeDBF();
                    CloseRoadDBF();
                    CloseS_ATTDBF();
                }
            }

            IPositionSet_Connected positionSet_Connected = new PositionSet_Connected(position_ConnectedList);
            return positionSet_Connected;
        }

        delegate DialogResult dShow(string text);
        private void pathFindingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPositionSet_Connected positionSet_Connected = (IPositionSet_Connected)positionSetContainer.GetPositionSet();

            Dijkstra dijkstra = new Dijkstra();
            AStar astar = new AStar();
            M2M_PF m2m_PF = new M2M_PF();

            IM2MStructure m2mStructure = null;            
            
            List<IPosition_Connected> m2mPath = null;
            List<IPosition_Connected> path = null;

            //计算算法运行时间用
            //IPosition_Connected start = null;
            //IPosition_Connected end = null;
            ISearchPathEngine searchPathEngine1 = m2m_PF;
            ISearchPathEngine searchPathEngine2 = dijkstra;
            
            m2m_PF.GetM2MStructure += delegate(IM2MStructure m2mS) { m2mStructure = m2mS; };
            m2m_PF.GetM2MStructureInPreprocess += delegate(IM2MStructure m2mS) { m2mStructure = m2mS; };
            List<ushort> timeStampList = new List<ushort>();
            m2m_PF.GetTimeStamp += delegate(ushort timeStamp) { timeStampList.Add(timeStamp); };

            searchPathEngine1.InitEngineForMap(positionSet_Connected);
            searchPathEngine2.InitEngineForMap(positionSet_Connected);

            //以下代码必须在UI线程中调用，即不能在另开的线程中调用
            LayersExOptDlg layers = new LayersExOptDlg();
            LayersPainterForm layersPainterForm = new LayersPainterForm(layers);
            LayersPaintedControl layersPaintedControl = layersPainterForm.LayersPaintedControl;
            LayersEditedControl layersEditedControl = new LayersEditedControl();
            layersEditedControl.Dock = DockStyle.Top;
            layersEditedControl.LayersPaintedControl = layersPaintedControl;
            layersPainterForm.Controls.Add(layersEditedControl);
            layersPainterForm.Show();

            //打开一个Worker线程来进行算法流程的演示（否则会阻塞UI线程以至于演示不能进行）
            IAsyncResult result = new dDemoProcess(delegate
            {
                Layer_PositionSet_Connected layer_PositionSet_Connected = new Layer_PositionSet_Connected(positionSet_Connected);
                layer_PositionSet_Connected.Point.PointColor = Color.Yellow;
                layer_PositionSet_Connected.Connection.LineColor = Color.Blue;
                layer_PositionSet_Connected.Connection.LineWidth = 0.6f;
                layer_PositionSet_Connected.Point.PointRadius = 1.2f;
                layers.Add(layer_PositionSet_Connected);
                layer_PositionSet_Connected.SpringLayerRepresentationChangedEvent(layer_PositionSet_Connected);

                for (int levelSequence = 1; levelSequence < m2mStructure.GetLevelNum(); levelSequence++)
                {
                    Layer_PartSet_Connected layer_PartSet_Connected = new Layer_PartSet_Connected(m2mStructure, levelSequence);
                    layer_PartSet_Connected.Visible = false;
                    layers.Add(layer_PartSet_Connected);
                }

                while (true)
                {
                    IPosition_Connected start = (IPosition_Connected)layersEditedControl.GetMouseDoubleChickedNearestPositionInCurrentPositionSet(positionSet_Connected);
                    PositionSet_ConnectedEdit startPointSet = new PositionSet_ConnectedEdit();
                    startPointSet.AddPosition_Connected(start);
                    Layer_PositionSet_Point layerStartPoint = new Layer_PositionSet_Point(startPointSet);
                    layerStartPoint.Active = true;
                    layerStartPoint.Point.IsDrawPointBorder = true;
                    layerStartPoint.Point.PointRadius = 5;
                    layerStartPoint.Point.PointColor = Color.PaleGreen;
                    layers.Add(layerStartPoint);
                    layerStartPoint.SpringLayerRepresentationChangedEvent(layerStartPoint);

                    IPosition_Connected end = (IPosition_Connected)layersEditedControl.GetMouseDoubleChickedNearestPositionInCurrentPositionSet(positionSet_Connected);
                    PositionSet_ConnectedEdit endPointSet = new PositionSet_ConnectedEdit();
                    endPointSet.AddPosition_Connected(end);
                    Layer_PositionSet_Point layerEndPoint = new Layer_PositionSet_Point(endPointSet);
                    layerEndPoint.Active = true;
                    layerEndPoint.Point.IsDrawPointBorder = true;
                    layerEndPoint.Point.PointRadius = 5;
                    layerEndPoint.Point.PointColor = Color.Cyan;
                    layers.Add(layerEndPoint);
                    layerEndPoint.SpringLayerRepresentationChangedEvent(layerEndPoint);

                    CountTimeTool.TimeCounter timeCounter = new CountTimeTool.TimeCounter();

                    searchPathEngine2.InitEngineForMap(positionSet_Connected);
                    double time2 = timeCounter.CountTimeForRepeatableMethod(delegate { searchPathEngine2.SearchPath(start, end); });
                    path = searchPathEngine2.SearchPath(start, end);
                    float pathLength2 = ((Dijkstra)searchPathEngine2).GetPathLength();
                    Console.WriteLine("Dijkstra consume time: " + time2 + " path Length = " + pathLength2);

                    searchPathEngine1.InitEngineForMap(positionSet_Connected);

                    //清空timeStampList以记录寻径过程各层的timeStamp
                    double time1 = timeCounter.CountTimeForRepeatableMethod(delegate { searchPathEngine1.SearchPath(start, end); });

                    timeStampList.Clear();
                    m2mPath = searchPathEngine1.SearchPath(start, end);
                    float pathLength1 = ((M2M_PF)searchPathEngine1).GetPathLength();

                    Console.WriteLine("M2M_PF   consume time: " + time1 + " path Length = " + pathLength1);
                    //searchPathEngine1.SearchPath(start, end);

                    if (searchPathEngine1 is M2M_PF && m2mStructure != null)
                    {                        
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
                                        IPosition_Connected tempPartConnected = (IPosition_Connected)part;
                                        if (((Tag_M2M_Part)tempPartConnected.GetAttachment()).isNeedToSearch == true)
                                        {
                                            if (timeStampList[levelSequence - 1] == ((Tag_M2M_Part)tempPartConnected.GetAttachment()).timeStamp)
                                            {
                                                position_ConnectedList.Add(tempPartConnected);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    IPosition_Connected tempPartConnected = (IPosition_Connected)positionSet.GetPosition();
                                    if (((Tag_M2M_Part)tempPartConnected.GetAttachment()).isNeedToSearch == true)
                                    {
                                        if (timeStampList[levelSequence - 1] == ((Tag_M2M_Part)tempPartConnected.GetAttachment()).timeStamp)
                                        {
                                            position_ConnectedList.Add(tempPartConnected);
                                        }
                                    }
                                }
                            }

                            IPositionSet partSet = new PositionSet_Connected(position_ConnectedList);

                            Layer_M2MPartSetInSpecificLevel layer_M2MPartSetInSpecificLevel = new Layer_M2MPartSetInSpecificLevel(m2mStructure.GetLevel(levelSequence), partSet);
                            layer_M2MPartSetInSpecificLevel.Active = true;
                            layer_M2MPartSetInSpecificLevel.Alpha = 100;
                            //layer_M2MPartSetInSpecificLevel.

                            layers.Add(layer_M2MPartSetInSpecificLevel);

                            //PositionSet_Connected partSet_Connected = new PositionSet_Connected(position_ConnectedList);

                            //Layer_PositionSet_Connected layer_PartSet_Connected = new Layer_PositionSet_Connected(partSet_Connected);
                            ////layer_PartSet_Connected.MainColor
                            ////layer_PartSet_Connected.Active = true;
                            //layer_PartSet_Connected.SetPositionSetTransformByM2MLevel(level);

                            //layers.Add(layer_PartSet_Connected);
                        }
                    }

                    if (path != null)
                    {
                        Layer_PositionSet_Path layer = new Layer_PositionSet_Path(new PositionSet_Connected(path));
                        layer.Active = true;
                        layer.PathLine.LineColor = Color.Black;
                        layer.PathLine.LineWidth = 2;
                        layer.PathPoint.PointRadius = 2;
                        layer.PathPoint.IsDrawPointBorder = true;

                        //layer.EditAble = true;
                        //layer.Point.PointColor = Color.Yellow;
                        //layer.Point.PointRadius = 2;
                        //layer.Point.IsDrawPointBorder = true;
                        layers.Add(layer);
                        layer.SpringLayerRepresentationChangedEvent(layer);
                    }
                    else
                    {
                        this.BeginInvoke(new dShow(MessageBox.Show), new object[] { "There is no path between two node" });
                    }

                    if (m2mPath != null)
                    {
                        Layer_PositionSet_Path layer = new Layer_PositionSet_Path(new PositionSet_Connected(m2mPath));
                        layer.Active = true;
                        layer.PathLine.LineColor = Color.Red;
                        layer.PathLine.LineWidth = 2;
                        layer.PathPoint.PointRadius = 2;
                        layer.PathPoint.IsDrawPointBorder = true;

                        //layer.EditAble = true;
                        //layer.Point.PointColor = Color.Yellow;
                        //layer.Point.PointRadius = 2;
                        //layer.Point.IsDrawPointBorder = true;
                        layers.Add(layer);
                        layer.SpringLayerRepresentationChangedEvent(layer);
                    }
                    else
                    {
                        this.BeginInvoke(new dShow(MessageBox.Show), new object[] { "There is no path between two node by M2M_PF" });
                    }
                }
            }).BeginInvoke(null, null);
        }

        private void preProcessForPositionSetConnectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IPositionSet_Connected pSet = (IPositionSet_Connected)positionSetContainer.GetPositionSet();

            LayersExOptDlg layers = new LayersExOptDlg();
            LayersPainterForm layersPainterForm = new LayersPainterForm(layers);
            LayersPaintedControl layersPaintedControl = layersPainterForm.LayersPaintedControl;
            LayersEditedControl layersEditedControl = new LayersEditedControl();
            layersEditedControl.Dock = DockStyle.Top;
            layersEditedControl.LayersPaintedControl = layersPaintedControl;
            layersPainterForm.Controls.Add(layersEditedControl);

            Layer_PositionSet_Connected layer_PositionSet_Connected = new Layer_PositionSet_Connected(pSet);
            layer_PositionSet_Connected.Point.PointColor = Color.Yellow;
            layer_PositionSet_Connected.Connection.LineColor = Color.Blue;
            layer_PositionSet_Connected.Connection.LineWidth = 0.6f;
            layer_PositionSet_Connected.Point.PointRadius = 1.2f;
            layers.Add(layer_PositionSet_Connected);
            layer_PositionSet_Connected.SpringLayerRepresentationChangedEvent(layer_PositionSet_Connected);


            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();

            m2m_Creater_ForGeneralM2MStruture.PartType = typeof(Part_Multi);
            m2m_Creater_ForGeneralM2MStruture.SetPointInPartFactor(50);
            m2m_Creater_ForGeneralM2MStruture.SetUnitNumInGridLength(3);
            
            IM2MStructure m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(pSet);
            m2mStructure.Preprocessing(pSet);

            BuildPartSetConnectionForM2MStructure buildPartSetConnectionForM2MStructure = new BuildPartSetConnectionForM2MStructure();
            
            buildPartSetConnectionForM2MStructure.TraversalEveryLevelAndBuild(m2mStructure);

            layers.Add(new Layer_M2MStructure(m2mStructure));

            for (int levelSequence = 1; levelSequence < m2mStructure.GetLevelNum(); levelSequence++)
            {
                //ILevel level = m2mStructure.GetLevel(levelSequence);
                //IPart rootPart = m2mStructure.GetLevel(0).GetPartRefByPartIndex(0, 0);
                //IPositionSet positionSet = m2mStructure.GetDescendentPositionSetByAncestorPart(levelSequence, rootPart, 0);

                //List<IPosition_Connected> position_ConnectedList = new List<IPosition_Connected>();
                //positionSet.InitToTraverseSet();
                //while (positionSet.NextPosition())
                //{
                //    position_ConnectedList.Add((IPosition_Connected)positionSet.GetPosition());
                //}
                   
                //PositionSet_Connected positionSet_Connected = new PositionSet_Connected(position_ConnectedList);

                Layer_PartSet_Connected layer_PartSet_Connected = new Layer_PartSet_Connected(m2mStructure, levelSequence);
                //layer_PartSet_Connected.MainColor
                //layer_PartSet_Connected.Active = true;
                //layer_PartSet_Connected.SetPositionSetTransformByM2MLevel(level);

                layers.Add(layer_PartSet_Connected);
            }

            layersPainterForm.ShowDialog();
        }

        private void createRandomSetConnectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //产生随机点集：
            RandomPositionSet_Connected_Config config = new RandomPositionSet_Connected_Config();
            new ConfiguratedByForm(config);
            IPositionSet_ConnectedEdit pSet = config.Produce();

            positionSetContainer.AddPositionSet(pSet);
        }
    }
}