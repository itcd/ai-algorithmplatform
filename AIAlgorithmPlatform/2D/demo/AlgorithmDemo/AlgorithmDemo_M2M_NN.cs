using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using M2M;
using NearestNeighbor;
using PositionSetViewer;
using PositionSetEditer;
using PositionSetDrawer;
using Configuration;
using Position_Interface;
using Position_Implement;

namespace AlgorithmDemo
{
    public class AlgorithmDemo_M2M_NN
    {
        M2M_NN m2m_NN;
        Layers layers;
        FlowControlerForm flowControlerForm;
        dUpdate Update;

        public AlgorithmDemo_M2M_NN(M2M_NN m2m_NN, Layers layers, FlowControlerForm flowControlerForm, dUpdate update)
        {
            this.m2m_NN = m2m_NN;
            this.layers = layers;
            this.flowControlerForm = flowControlerForm;
            this.Update = update;

            m2m_NN.GetQueryPosition += delegate
            {
                SearchPartSetLayer = null;
                ComparedPointLayer = null;
                shrinkBoundPointSetLayer = null;
                searchBoundLayer = null;
            };

            m2m_NN.GetPositionSetOfComparedPoint += OnGetPositionSetOfComparedPoint;
            m2m_NN.GetM2MStructure += OnGetM2MStructure;
            m2m_NN.GetQueryPosition += OnGetQueryPosition;
            m2m_NN.GetQueryPart += OnGetQueryPart;
            m2m_NN.GetSearchPart += OnGetSearchPart;
            m2m_NN.GetComparedPoint += OnGetComparedPoint;
            m2m_NN.SearchBoundChanged += OnSearchBoundChanged;
            m2m_NN.CurrentNearestPointChanged += OnCurrentNearestPointChanged;

            flowControlerForm.SelectConfiguratoinObject(this);
        }

        public void EndDemo()
        {
            IsGetPositionSetOfComparedPoint = false;
            IsGetM2MStructure = false;
            IsGetComparedPoint = false;

            m2m_NN.GetQueryPosition -= OnGetQueryPosition;
            m2m_NN.GetQueryPart -= OnGetQueryPart;
            m2m_NN.GetSearchPart -= OnGetSearchPart;
            m2m_NN.SearchBoundChanged -= OnSearchBoundChanged;
            m2m_NN.CurrentNearestPointChanged -= OnCurrentNearestPointChanged;

            flowControlerForm.SelectConfiguratoinObject(null);
            flowControlerForm.Reset();
        }

        bool isGetPositionSetOfComparedPoint = true;
        public bool IsGetPositionSetOfComparedPoint
        {
            get { return isGetPositionSetOfComparedPoint; }
            set
            {
                if (isGetPositionSetOfComparedPoint != value)
                {
                    isGetPositionSetOfComparedPoint = value;
                    if (value)
                    {
                        m2m_NN.GetPositionSetOfComparedPoint += OnGetPositionSetOfComparedPoint;
                    }
                    else
                    {
                        m2m_NN.GetPositionSetOfComparedPoint -= OnGetPositionSetOfComparedPoint;
                    }
                }
            }
        }

        bool isGetM2MStructure = true;
        public bool IsGetM2MStructure
        {
            get { return isGetM2MStructure; }
            set
            {
                if (isGetM2MStructure != value)
                {
                    isGetM2MStructure = value;
                    if (value)
                    {
                        m2m_NN.GetM2MStructure += OnGetM2MStructure;
                    }
                    else
                    {
                        m2m_NN.GetM2MStructure -= OnGetM2MStructure;
                    }
                }
            }
        }

        bool isGetComparedPoint = true;
        public bool IsGetComparedPoint
        {
            get { return isGetComparedPoint; }
            set
            {
                if (isGetComparedPoint != value)
                {
                    isGetComparedPoint = value;
                    if (value)
                    {
                        m2m_NN.GetComparedPoint += OnGetComparedPoint;
                    }
                    else
                    {
                        m2m_NN.GetComparedPoint -= OnGetComparedPoint;
                    }
                }
            }
        }


        private void OnGetPositionSetOfComparedPoint(IPositionSet positionSet)
        {
            lock (layers)
            {
                Layer_PositionSet_Point layer = new Layer_PositionSet_Point(new PositionSet_Cloned(positionSet));
                layer.Point.PointRadius = 2;
                layer.Point.IsDrawPointBorder = true;
                layer.MainColor = Color.FromArgb(0, 255, 0);
                layer.Name = "PositionSetOfComparedPoint";
                layers.Add(layer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnGetM2MStructure(IM2MStructure m2mStructure)
        {
            lock (layers)
            {
                Layer_M2MStructure layer = new Layer_M2MStructure(m2mStructure);
                layer.Name = "M2MStructure";
                layers.Add(layer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnGetQueryPosition(IPosition position)
        {
            lock (layers)
            {
                IPositionSetEdit temp = new PositionSetEdit_ImplementByICollectionTemplate();
                temp.AddPosition(position);

                Layer_PositionSet_Point queryPositionLayer = new Layer_PositionSet_Point(temp);
                queryPositionLayer.Point.PointRadius = 3;
                queryPositionLayer.Point.PointColor = Color.IndianRed;
                queryPositionLayer.Point.IsDrawPointBorder = true;
                queryPositionLayer.Active = true;
                layers.Add(queryPositionLayer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnGetQueryPart(ILevel level, int levelSequence, IPart part)
        {
            lock (layers)
            {
                PositionSetEdit_ImplementByICollectionTemplate partSet = new PositionSetEdit_ImplementByICollectionTemplate();
                partSet.AddPosition(part);
                Layer_M2MPartSetInSpecificLevel layer = new Layer_M2MPartSetInSpecificLevel(level, partSet);
                layer.MainColor = Color.FromArgb(255, 0, 0);
                layer.Active = true;
                layers.Add(layer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_M2MPartSetInSpecificLevel SearchPartSetLayer = null;
        IPositionSetEdit SearchPartSet = null;
        private void OnGetSearchPart(ILevel level, int levelSequence, IPart part)
        {
            lock (layers)
            {
                if (SearchPartSetLayer == null)
                {
                    SearchPartSet = new PositionSetEdit_ImplementByICollectionTemplate();
                    SearchPartSetLayer = new Layer_M2MPartSetInSpecificLevel(level, SearchPartSet);
                    SearchPartSetLayer.MainColor = Color.Blue;
                    SearchPartSetLayer.Active = true;
                    layers.Add(SearchPartSetLayer);
                }

                SearchPartSet.AddPosition(part);
                SearchPartSetLayer.PositionSet = SearchPartSet;
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_PositionSet_Point ComparedPointLayer = null;
        IPositionSetEdit ComparedPointSet = null;
        private void OnGetComparedPoint(IPosition point)
        {
            lock (layers)
            {
                if (ComparedPointLayer == null)
                {
                    ComparedPointSet = new PositionSetEdit_ImplementByICollectionTemplate();
                    ComparedPointLayer = new Layer_PositionSet_Point(ComparedPointSet);
                    ComparedPointLayer.Active = true;
                    ComparedPointLayer.Point.PointRadius = 2;
                    ComparedPointLayer.Point.IsDrawPointBorder = true;
                    ComparedPointLayer.Point.PointColor = Color.Goldenrod;
                    layers.Add(ComparedPointLayer);
                }

                ComparedPointSet.AddPosition(point);
                ComparedPointLayer.PositionSet = ComparedPointSet;
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_PositionSet_Point shrinkBoundPointSetLayer = null;
        IPositionSetEdit shrinkBoundPointSet = null;
        private void OnCurrentNearestPointChanged(IPosition point)
        {
            lock (layers)
            {
                if (shrinkBoundPointSetLayer == null)
                {
                    shrinkBoundPointSet = new PositionSetEdit_ImplementByICollectionTemplate();
                    shrinkBoundPointSetLayer = new Layer_PositionSet_Point(shrinkBoundPointSet);
                    shrinkBoundPointSetLayer.Active = true;
                    shrinkBoundPointSetLayer.Point.PointRadius = 3;
                    shrinkBoundPointSetLayer.Point.IsDrawPointBorder = true;
                    shrinkBoundPointSetLayer.Point.PointColor = Color.Red;
                    layers.Add(shrinkBoundPointSetLayer);
                }

                shrinkBoundPointSet.AddPosition(point);
                shrinkBoundPointSetLayer.PositionSet = shrinkBoundPointSet;
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_Rectangle searchBoundLayer = null;
        private void OnSearchBoundChanged(float upperBound, float lowerBound, float leftBound, float rightBound)
        {
            lock (layers)
            {
                if (searchBoundLayer == null)
                {
                    searchBoundLayer = new Layer_Rectangle(upperBound, lowerBound, leftBound, rightBound);
                    searchBoundLayer.Active = true;
                    searchBoundLayer.PenWidth = 2;
                    searchBoundLayer.PenStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    searchBoundLayer.MainColor = Color.Orange;
                    layers.Add(searchBoundLayer);
                }

                searchBoundLayer.UpperBound = upperBound;
                searchBoundLayer.LowerBound = lowerBound;
                searchBoundLayer.LeftBound = leftBound;
                searchBoundLayer.RightBound = rightBound;
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }
    }
}
