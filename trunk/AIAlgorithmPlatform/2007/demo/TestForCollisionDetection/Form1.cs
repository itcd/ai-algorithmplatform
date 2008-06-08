using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

namespace TestForCollisionDetection
{
    [Obsolete("ssss")]
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        delegate void dDemoProcess();
        
        private void button1_Click(object sender, EventArgs e)
        {
            M2M_CD M2M_CD = new M2M_CD();
            new ConfiguratedByForm(M2M_CD);
            
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
            
            

            //打开一个Worker线程来进行算法流程的演示（否则会阻塞UI线程以至于演示不能进行）
            IAsyncResult result = new dDemoProcess(delegate
            {
                //产生随机点集：
                /*
                RandomPositionSet_InFixedDistribution randomPositionSet_InFixedDistribution = new RandomPositionSet_InFixedDistribution();
                randomPositionSet_InFixedDistribution.PointNum = 1000;
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
                 * */

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

                //对M2M_CD算法进行演示：
                //为对象的事件添加事件响应，以进行算法演示
                PositionSetEditSet polygonSet = new PositionSetEditSet();
                IPositionSetEdit polygon = new PositionSetEdit_ImplementByICollectionTemplate();
                polygon.AddPosition(new Position_Point(40, 40));
                polygon.AddPosition(new Position_Point(50, 10));
                polygon.AddPosition(new Position_Point(100, 70));
                IPositionSetEdit polygon2 = new PositionSetEdit_ImplementByICollectionTemplate();
                polygon2.AddPosition(new Position_Point(0, 0));
                polygon2.AddPosition(new Position_Point(20, 20));
                polygon2.AddPosition(new Position_Point(21, 12));
                IPositionSetEdit polygon3 = new PositionSetEdit_ImplementByICollectionTemplate();
                polygon3.AddPosition(new Position_Point(60, 60));
                polygon3.AddPosition(new Position_Point(62, 60));
                polygon3.AddPosition(new Position_Point(62, 56));
                polygon3.AddPosition(new Position_Point(64, 56));
                polygon3.AddPosition(new Position_Point(61, 52));
                polygon3.AddPosition(new Position_Point(58, 56));
                polygon3.AddPosition(new Position_Point(60, 56));
                polygonSet.AddPositionSet(polygon);
                polygonSet.AddPositionSet(polygon2);
                //polygonSet.AddPositionSet(polygon3);

                
                

                AlgorithmDemo_M2M_CD algorithmDemo_M2M_CD = new AlgorithmDemo_M2M_CD(M2M_CD , layers, flowControlerForm, layersPaintedControl.Invalidate);
                while (true)
                {
                    layers.Clear();
                    //以下代码与非演示状态一样。
                    //M2M_CD.ConvexHull(randomPositionSet_InFixedDistribution.Produce());
                    Layer_PositionSet_Polygon layer = new Layer_PositionSet_Polygon(polygon);
                    Layer_PositionSet_Polygon layer2 = new Layer_PositionSet_Polygon(polygon2);
                    layers.Add(layer);
                    layers.Add(layer2);
                    M2M_CD.CollisionDetection(polygonSet,polygon3);
                }

                //结束演示（解除事件响应的绑定）
                algorithmDemo_M2M_CD.EndDemo();

            }).BeginInvoke(null, null);
        }

        
    }
}
