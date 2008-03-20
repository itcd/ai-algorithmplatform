using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using M2M;
using NearestNeighbor;
using Configuration;
using PositionSetViewer;
using Position_Interface;
using DrawingLib;
using PositionSetEditer;
using AlgorithmDemo;
using Position_Implement;


namespace TestForAlgorithmDemo
{
    public partial class TestForAlgorithmDemoForm : Form
    {
        public TestForAlgorithmDemoForm()
        {
            InitializeComponent();
        }

        delegate void dDemoProcess();
        private void TestForAlgorithmDemo(object sender, EventArgs e)
        {
            //以下代码必须在UI线程中调用，即不能在另开的线程中调用
            LayersExOptDlg layers = new LayersExOptDlg();
            LayersPainterForm layersPainterForm = new LayersPainterForm(layers);
            LayersPaintedControl layersPaintedControl = layersPainterForm.LayersPaintedControl;
            LayersEditedControl layersEditedControl = new LayersEditedControl();
            layersEditedControl.Dock = DockStyle.Top;
            layersEditedControl.LayersPaintedControl = layersPaintedControl;
            layersPainterForm.Controls.Add(layersEditedControl);
            FlowControlerForm flowControlerForm = new FlowControlerForm();
            layersPainterForm.Show();
            flowControlerForm.Show(layersPainterForm);

            //新建或外部传入待演示对象
            M2M_NN m2m_NN = new M2M_NN();
            M2M_CH m2m_CH = new M2M_CH();

            //打开一个Worker线程来进行算法流程的演示（否则会阻塞UI线程以至于演示不能进行）
            IAsyncResult result = new dDemoProcess(delegate
            {
                //产生随机点集：
                RandomPositionSet_InFixedDistribution randomPositionSet_InFixedDistribution = new RandomPositionSet_InFixedDistribution();
                randomPositionSet_InFixedDistribution.PointNum = 100000;
                randomPositionSet_InFixedDistribution.DistributionStyle = distributionStyle.ClusterGaussianDistribution;
                new ConfiguratedByForm(randomPositionSet_InFixedDistribution);
                randomPositionSet_InFixedDistribution.Produce();

                //编辑点集
                Layer_PositionSet_Point layer = new Layer_PositionSet_Point(randomPositionSet_InFixedDistribution);
                layer.EditAble = true;
                layer.Point.PointColor = Color.Yellow;
                layer.Point.PointRadius = 2;
                layer.Point.IsDrawPointBorder = true;
                layers.Add(layer);
                layersPainterForm.Invalidate();
                flowControlerForm.SuspendAndRecordWorkerThread();
                layers.Remove(layer);

                ///////////////////////////////////////
                //GetRandomPositionFromPositionSetRectangle getRandomPositionFromPositionSetRectangle
                //= new GetRandomPositionFromPositionSetRectangle(randomPositionSet_InFixedDistribution);
                                
                ////对m2m_NN算法进行演示：
                ////为对象的事件添加事件响应，以进行算法演示
                //AlgorithmDemo_M2M_NN algorithmDemo_M2M_NN = new AlgorithmDemo_M2M_NN(m2m_NN, layers, flowControlerForm, layersPaintedControl.Invalidate);

                ////以下代码与非演示状态一样。
                //m2m_NN.PreProcess(randomPositionSet_InFixedDistribution);
                //m2m_NN.NearestNeighbor(layersPaintedControl.GetMouseDoubleChickedRealPosition());
                //while (true)
                //{
                //    for (int i = layers.Count - 1; i >= 0; i--)
                //    {
                //        if ((layers[i].Name != "M2MStructure") && (layers[i].Name != "PositionSetOfComparedPoint"))
                //        {
                //            layers.Remove(layers[i]);
                //        }
                //    }

                //    m2m_NN.NearestNeighbor(getRandomPositionFromPositionSetRectangle.Get());
                //}

                ////结束演示（解除事件响应的绑定）
                //algorithmDemo_M2M_NN.EndDemo();
                ///////////////////////////////////////

                //对m2m_CH算法进行演示：
                //为对象的事件添加事件响应，以进行算法演示
                AlgorithmDemo_M2M_CH algorithmDemo_M2M_CH = new AlgorithmDemo_M2M_CH(m2m_CH, layers, flowControlerForm, layersPaintedControl.Invalidate);
                while (true)
                {
                    layers.Clear();
                    //以下代码与非演示状态一样。
                    m2m_CH.ConvexHull(randomPositionSet_InFixedDistribution.Produce());
                }

                //结束演示（解除事件响应的绑定）
                algorithmDemo_M2M_CH.EndDemo();

            }).BeginInvoke(null, null);
        }
    }
}