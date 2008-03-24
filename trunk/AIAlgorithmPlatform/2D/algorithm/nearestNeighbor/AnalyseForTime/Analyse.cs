using System;
using System.Collections.Generic;
using RandomMakerAlgorithm;
using M2M;
using KDT;
using Position_Interface;
using CountTimeTool;
using CurvePlotDrawer;
using PositionSetViewer;
using Position_Implement;

namespace AnalyseForTime
{
    class Analyse
    {
        M2M_NN             m2m_nn;
        KDT_NN             kdt_nn;
        KD2DPoint[]        askPoint;
        IPosition[]         m2mNearestPoint, kdtNearestPoint;
        int                 mapWidth = 100, mapHeight = 100;
        //List<IPosition>     posList;

        //TimeCounter           stopwatch;
        //Random              random;
        //double[] nnX, nnLogX, m2mnnY, kdtnnY, m2mnnppY, kdtnnppY;

        /**************************************************************************************
         * 功能：对算法进行分析
         * 参数：width          - 地图宽
         *       height         - 地图高
         *       start          - 初始的点集数目
         *       multiple       - 每一轮分析后点集数目的增大倍数
         *       round          - 已不同点集数目分析的次数
         *       avergae        - 对同一点集数目分析的次数（然后取结果平均值）
         **************************************************************************************/
        public void analyse(int askCount)
        {
            //生成源点集
            askPoint = new KD2DPoint[askCount];
            for (int i = 0; i < askCount; i++)
                askPoint[i] = getRandomPoint(mapWidth, mapHeight);
            m2mNearestPoint = new IPosition[askCount];
            kdtNearestPoint = new IPosition[askCount];

            double[] dataSequence = SequenceMaker.GenerateData_Multiple();

            CurvePlot_Function_Time curvePlot1 = new CurvePlot_Function_Time("Comparsion of the Preprocesses of KDTree and M2M", "PointNumber", "time", false, createPointSet);
            init();
            curvePlot1.addProc("M2MNNPP", runM2MNNPP);
            curvePlot1.addProc("KDTNNPP", runKDTNNPP);
            curvePlot1.draw(dataSequence);

            CurvePlot_Function_Time curvePlot2 = new CurvePlot_Function_Time("Comparsion of the Nearest Neighbor Algorithm base on KDTree and M2M", "PointNumber", "time", false, createPointSetWithPrepare);
            init();
            curvePlot2.addProc("M2MN", runM2MNN);
            curvePlot2.addProc("KDTNN", runKDTNN);
            curvePlot2.setAskCount(askCount);
            curvePlot2.draw(dataSequence);
            compareResult();
        }


        //初始化
        private void init()
        {
            m2m_nn = new M2M_NN();
            kdt_nn = new KDT_NN();
        }


        //运行M2MNN算法测试
        private void runM2MNN(double n, Object o)
        {
            for (int i = 0; i < askPoint.Length; i++)
                m2mNearestPoint[i] = m2m_nn.NearestNeighbor(askPoint[i]);
        }
        

        //运行KDTNN算法测试
        private void runKDTNN(double n, Object o)
        {
            for (int i = 0; i < askPoint.Length; i++)
                kdtNearestPoint[i] = kdt_nn.NearestNeighbor(askPoint[i]);
        }

        //运行M2MNN算法预处理过程
        private void runM2MNNPP(double n, Object o)
        {
            m2m_nn.PreProcess((List<IPosition>)o);
        }

        //运行KDTNN算法预处理过程
        private void runKDTNNPP(double n, Object o)
        {
            kdt_nn.PreProcess((List<IPosition>)o);
        }

        //比较两种算法得出的结果
        private void compareResult()
        {
            int n = askPoint.Length;
            for (int i = 0; i < n; i++)
            {
                double d1 = (m2mNearestPoint[i].GetX() - askPoint[i].GetX()) * (m2mNearestPoint[i].GetX() - askPoint[i].GetX()) + (m2mNearestPoint[i].GetY() - askPoint[i].GetY()) * (m2mNearestPoint[i].GetY() - askPoint[i].GetY());
                double d2 = (kdtNearestPoint[i].GetX() - askPoint[i].GetX()) * (kdtNearestPoint[i].GetX() - askPoint[i].GetX()) + (kdtNearestPoint[i].GetY() - askPoint[i].GetY()) * (kdtNearestPoint[i].GetY() - askPoint[i].GetY());
                if (d1 != d2)
                    Console.WriteLine("结果不匹配：" + d1 + " != " + d2);
            }
        }

        //产生一个随机点
        KD2DPoint getRandomPoint(int mapWidth, int mapHeight)
        {
            //return new KD2DPoint(random.Next(mapWidth - 1), random.Next(mapHeight - 1));
            return new KD2DPoint(RandomMaker.RapidBetween(0.0f, mapWidth - 1.0f), RandomMaker.RapidBetween(0.0f, mapHeight - 1.0f));
        }

        //生成随机点集
        List<IPosition> createPointSet(double pointCount)
        {
            List<IPosition> posList = new List<IPosition>();
            for (int i = 0; i < pointCount; i++)
                posList.Add(getRandomPoint(mapWidth, mapHeight));

            return posList;
        }
        List<IPosition> createPointSetWithPrepare(double pointCount)
        {
            List<IPosition> posList = new List<IPosition>();
            for (int i = 0; i < pointCount; i++)
                posList.Add(getRandomPoint(mapWidth, mapHeight));
            runM2MNNPP(0, posList);
            runKDTNNPP(0, posList);
            return posList;

        }
    }

    class Analyse2
    {
        M2M_NN m2m_nn;
        KDT_NN kdt_nn;
        KD2DPoint[] askPoint;
        IPosition[] m2mNearestPoint, kdtNearestPoint;
        //int mapWidth = 400, mapHeight = 400;
       
        int askCount = 100;

        public int AskCount
        {
            get { return askCount; }
            set { askCount = value; }
        }

        distributionStyle dStyle;
        public distributionStyle DistributionStyle
        {
            get { return dStyle; }
            set { dStyle = value; }
        }

        double theDensityOfBottomLevel = 33;

        public double TheDensityOfBottomLevel
        {
            get { return theDensityOfBottomLevel; }
            set { theDensityOfBottomLevel = value; }
        }

        
        /**************************************************************************************
         * 功能：对算法进行分析
         * 参数：width          - 地图宽
         *       height         - 地图高
         *       start          - 初始的点集数目
         *       multiple       - 每一轮分析后点集数目的增大倍数
         *       round          - 已不同点集数目分析的次数
         *       avergae        - 对同一点集数目分析的次数（然后取结果平均值）
         **************************************************************************************/
        public void analyse()
        {
            //生成源点集
            askPoint = new KD2DPoint[askCount];
            for (int i = 0; i < askCount; i++)
                askPoint[i] = getRandomPoint();
            m2mNearestPoint = new IPosition[askCount];
            kdtNearestPoint = new IPosition[askCount];

            double[] dataSequence = SequenceMaker.GenerateData_Multiple();

            CurvePlot_Function_Time curvePlot1 = new CurvePlot_Function_Time("Comparsion of the Preprocesses of KDTree and M2M", "PointNumber", "time", false, createPointSet);
            init();
            curvePlot1.addProc("M2MNNPP", runM2MNNPP);
            curvePlot1.addProc("KDTNNPP", runKDTNNPP);
            curvePlot1.draw(dataSequence);

            CurvePlot_Function_Time curvePlot2 = new CurvePlot_Function_Time("Comparsion of the Nearest Neighbor Algorithm base on KDTree and M2M", "PointNumber", "time", false, createPointSetWithPrepare);
            init();
            curvePlot2.addProc("M2MN", runM2MNN);
            curvePlot2.addProc("KDTNN", runKDTNN);
            curvePlot2.setAskCount(askCount);
            curvePlot2.draw(dataSequence);
            compareResult();
        }
        
        //初始化
        private void init()
        {
            m2m_nn = new M2M_NN();
            ((M2M_NN)m2m_nn).TheDensityOfBottomLevel = theDensityOfBottomLevel;
            kdt_nn = new KDT_NN();
        }
        
        //运行M2MNN算法测试
        private void runM2MNN(double n, Object o)
        {
            for (int i = 0; i < askPoint.Length; i++)
                m2mNearestPoint[i] = m2m_nn.NearestNeighbor(askPoint[i]);
        }


        //运行KDTNN算法测试
        private void runKDTNN(double n, Object o)
        {
            for (int i = 0; i < askPoint.Length; i++)
                kdtNearestPoint[i] = kdt_nn.NearestNeighbor(askPoint[i]);
        }

        //运行M2MNN算法预处理过程
        private void runM2MNNPP(double n, Object o)
        {
            m2m_nn.PreProcess((List<IPosition>)o);
        }

        //运行KDTNN算法预处理过程
        private void runKDTNNPP(double n, Object o)
        {         
            kdt_nn.PreProcess((List<IPosition>)o);
        }

        //比较两种算法得出的结果
        private void compareResult()
        {
            int n = askPoint.Length;
            for (int i = 0; i < n; i++)
            {
                double d1 = (m2mNearestPoint[i].GetX() - askPoint[i].GetX()) * (m2mNearestPoint[i].GetX() - askPoint[i].GetX()) + (m2mNearestPoint[i].GetY() - askPoint[i].GetY()) * (m2mNearestPoint[i].GetY() - askPoint[i].GetY());
                double d2 = (kdtNearestPoint[i].GetX() - askPoint[i].GetX()) * (kdtNearestPoint[i].GetX() - askPoint[i].GetX()) + (kdtNearestPoint[i].GetY() - askPoint[i].GetY()) * (kdtNearestPoint[i].GetY() - askPoint[i].GetY());
                if (d1 != d2)
                    Console.WriteLine("结果不匹配：" + d1 + " != " + d2);
            }
        }

        //产生一个随机点
        KD2DPoint getRandomPoint()
        {
            //return new KD2DPoint(random.Next(mapWidth - 1), random.Next(mapHeight - 1));
            return new KD2DPoint(RandomMaker.RapidBetween(minX, maxX), RandomMaker.RapidBetween(minY, maxY));
        }

        //生成随机点集
        List<IPosition> createPointSet(double pointCount)
        {
            RandomPositionSet_InFixedDistribution positionSet = new RandomPositionSet_InFixedDistribution((int)pointCount, dStyle);

            List<IPosition> posList = new List<IPosition>();

            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                KD2DPoint point = new KD2DPoint(positionSet.GetPosition());
                posList.Add(point);
            }

            return posList;
        }

        float minX = 0;
        float minY = 0;
        float maxX = 0;
        float maxY = 0;
        List<IPosition> createPointSetWithPrepare(double pointCount)
        {
            RandomPositionSet_InFixedDistribution positionSet = new RandomPositionSet_InFixedDistribution((int)pointCount, dStyle);

            List<IPosition> posList = new List<IPosition>();

            minX = float.MaxValue;
            minY = float.MaxValue;
            maxX = float.MinValue;
            maxY = float.MinValue;

            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                KD2DPoint point = new KD2DPoint(positionSet.GetPosition());
                posList.Add(point);

                if (minX > point.GetX())
                {
                    minX = point.GetX();
                }
                else if (maxX < point.GetX())
                {
                    maxX = point.GetX();
                }

                if (minY > point.GetY())
                {
                    minY = point.GetY();
                }
                else if (maxY < point.GetY())
                {
                    maxY = point.GetY();
                }
            }

            runM2MNNPP(0, posList);
            runKDTNNPP(0, posList);
            return posList;
        }
    }
}
