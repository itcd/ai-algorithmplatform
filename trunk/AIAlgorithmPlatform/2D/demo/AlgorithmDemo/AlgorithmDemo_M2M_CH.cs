using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using M2M;
using NearestNeighbor;
using ConvexHullEngine;
using PositionSetViewer;
using PositionSetEditer;
using Configuration;
using Position_Interface;
using Position_Implement;
using AlgorithmDemo.Properties;

namespace AlgorithmDemo
{
    public class AlgorithmDemo_M2M_CH
    {
        M2M_CH m2m_CH;
        Layers layers;
        FlowControlerForm flowControlerForm;
        dUpdate Update;
        IM2MStructure m2mStructure;
        Color[] aryMainColor = { Color.FromArgb(30, 249, 35), Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 235), Color.FromArgb(236, 19, 225) };

        public AlgorithmDemo_M2M_CH(M2M_CH m2m_CH, Layers layers, FlowControlerForm flowControlerForm, dUpdate update)
        {
            this.m2m_CH = m2m_CH;
            this.layers = layers;
            this.flowControlerForm = flowControlerForm;
            this.Update = update;

            m2m_CH.GetM2MStructure += delegate(IM2MStructure m2mStructure)
            {
                this.m2mStructure = m2mStructure;
            };

            m2m_CH.GetChildPositionSetInSpecificLevelOfConvexHull += delegate
            {
                if (representativeHullLayer != null)
                {
                    representativeHullLayer.Visible = false;
                }
                if (convexHullLayer != null)
                {
                    lock (layers)
                    {
                        convexHullLayer.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate
            {
                if (linePartSetLayer != null)
                {
                    lock (layers)
                    {
                        linePartSetLayer.Visible = false;
                        BottonLevelPositionSetLayer.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            m2m_CH.GetRealConvexHull += delegate
            {
                if (representativeHullLayer != null)
                {
                    lock (layers)
                    {
                        representativeHullLayer.Visible = false;
                    }
                }
                if (convexHullLayer != null)
                {
                    lock (layers)
                    {
                        convexHullLayer.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            IsGetM2MStructure = true;
            IsGetPositionSetToGetConvexHull = true;
            IsGetConvexHullPositionSetInSpecificLevel = true;
            IsGetRepresentativeHullInSpecificLevel = true;
            IsGetLinePositionSetInSpecificLevel = true;
            IsGetChildPositionSetInSpecificLevelOfConvexHull = true;
            IsGetRealConvexHull = true;

            m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate
            {
                if (childPositionSetOfConvexHullLayer != null)
                {
                    lock (layers)
                    {
                        childPositionSetOfConvexHullLayer.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate
            {
                if (convexHullLayer != null)
                {
                    lock (layers)
                    {
                        convexHullLayer.ConvexHull.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            m2m_CH.GetRealConvexHull += delegate
            {
                if (linePartSetLayer != null)
                {
                    lock (layers)
                    {
                        linePartSetLayer.Visible = false;
                        BottonLevelPositionSetLayer.Visible = false;
                    }
                    flowControlerForm.BeginInvoke(Update);
                    flowControlerForm.SuspendAndRecordWorkerThread();
                }
            };

            flowControlerForm.SelectConfiguratoinObject(this);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        public void EndDemo()
        {
            IsGetM2MStructure = false;
            IsGetPositionSetToGetConvexHull = false;
            IsGetConvexHullPositionSetInSpecificLevel = false;
            IsGetRepresentativeHullInSpecificLevel = false;
            IsGetLinePositionSetInSpecificLevel = false;
            IsGetChildPositionSetInSpecificLevelOfConvexHull = false;
            IsGetRealConvexHull = false;

            flowControlerForm.SelectConfiguratoinObject(null);
            flowControlerForm.Reset();
        }

        bool isGetPositionSetToGetConvexHull = false;
        public bool IsGetPositionSetToGetConvexHull
        {
            get { return isGetPositionSetToGetConvexHull; }
            set
            {
                if (isGetPositionSetToGetConvexHull != value)
                {
                    isGetPositionSetToGetConvexHull = value;
                    if (value)
                    {
                        m2m_CH.GetPositionSetToGetConvexHull += OnGetPositionSetToGetConvexHull;
                    }
                    else
                    {
                        m2m_CH.GetPositionSetToGetConvexHull -= OnGetPositionSetToGetConvexHull;
                    }
                }
            }
        }

        bool isGetM2MStructure = false;
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
                        m2m_CH.GetM2MStructure += OnGetM2MStructure;
                    }
                    else
                    {
                        m2m_CH.GetM2MStructure -= OnGetM2MStructure;
                    }
                }
            }
        }

        bool isGetConvexHullPositionSetInSpecificLevel = false;
        public bool IsGetConvexHullPositionSetInSpecificLevel
        {
            get { return isGetConvexHullPositionSetInSpecificLevel; }
            set
            {
                if (isGetConvexHullPositionSetInSpecificLevel != value)
                {
                    isGetConvexHullPositionSetInSpecificLevel = value;
                    if (value)
                    {
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel += OnGetConvexHullPositionSetInSpecificLevel;
                    }
                    else
                    {
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel -= OnGetConvexHullPositionSetInSpecificLevel;
                    }
                }
            }
        }

        bool isGetRepresentativeHull = false;
        public bool IsGetRepresentativeHullInSpecificLevel
        {
            get { return isGetRepresentativeHull; }
            set
            {
                if (isGetRepresentativeHull != value)
                {
                    isGetRepresentativeHull = value;
                    if (value)
                    {
                        m2m_CH.GetRepresentativeHullInSpecificLevel += OnGetRepresentativeHullInSpecificLevel;
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate { representativeHullLayer = null; };
                    }
                    else
                    {
                        m2m_CH.GetRepresentativeHullInSpecificLevel -= OnGetRepresentativeHullInSpecificLevel;
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate { representativeHullLayer = null; };
                    }
                }
            }
        }

        bool isGetLinePositionSetInSpecificLevel = false;
        public bool IsGetLinePositionSetInSpecificLevel
        {
            get { return isGetLinePositionSetInSpecificLevel; }
            set
            {
                if (isGetLinePositionSetInSpecificLevel != value)
                {
                    isGetLinePositionSetInSpecificLevel = value;
                    if (value)
                    {
                        m2m_CH.GetLinePositionSetInSpecificLevel += OnGetLinePositionSetInSpecificLevel;
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel += delegate { OnTheEndOfGetLinePositionSetInSpecificLevel(); };
                    }
                    else
                    {
                        m2m_CH.GetLinePositionSetInSpecificLevel -= OnGetLinePositionSetInSpecificLevel;
                        m2m_CH.GetConvexHullPositionSetInSpecificLevel -= delegate { OnTheEndOfGetLinePositionSetInSpecificLevel(); };
                    }
                }
            }
        }

        bool isGetChildPositionSetInSpecificLevelOfConvexHull = false;
        public bool IsGetChildPositionSetInSpecificLevelOfConvexHull
        {
            get { return isGetChildPositionSetInSpecificLevelOfConvexHull; }
            set
            {
                if (isGetChildPositionSetInSpecificLevelOfConvexHull != value)
                {
                    isGetChildPositionSetInSpecificLevelOfConvexHull = value;
                    if (value)
                    {
                        m2m_CH.GetChildPositionSetInSpecificLevelOfConvexHull += OnGetChildPositionSetInSpecificLevelOfConvexHull;
                    }
                    else
                    {
                        m2m_CH.GetChildPositionSetInSpecificLevelOfConvexHull -= OnGetChildPositionSetInSpecificLevelOfConvexHull;
                    }
                }
            }
        }

        bool isGetRealConvexHull = false;
        public bool IsGetRealConvexHull
        {
            get { return isGetRealConvexHull; }
            set
            {
                if (isGetRealConvexHull != value)
                {
                    isGetRealConvexHull = value;
                    if (value)
                    {
                        m2m_CH.GetRealConvexHull += OnGetRealConvexHull;
                    }
                    else
                    {
                        m2m_CH.GetRealConvexHull -= OnGetRealConvexHull;
                    }
                }
            }
        }

        private void OnGetPositionSetToGetConvexHull(IPositionSet positionSet)
        {
            lock (layers)
            {
                Layer_PositionSet_Point layer = new Layer_PositionSet_Point(new PositionSet_Cloned(positionSet));
                layer.MainColor = Settings.Default.PositionSetToGetConvexHullColor;
                layer.Point.PointRadius = 1;
                layers.Add(layer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnGetM2MStructure(IM2MStructure m2mStructure)
        {
            lock (layers)
            {
                layers.Add(new Layer_M2MStructure(m2mStructure));
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_PositionSet_ConvexHull convexHullLayer;
        private void OnGetConvexHullPositionSetInSpecificLevel(ILevel level, int levelSequence, IPositionSet positionSet)
        {
            lock (layers)
            {
                convexHullLayer = new Layer_PositionSet_ConvexHull(new PositionSet_Cloned(positionSet));
                convexHullLayer.MainColor = Settings.Default.ConvexHullPositionSetInSpecificLevelColor;
                convexHullLayer.ConvexHull.LineWidth = 2;
                convexHullLayer.ConvexHull.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                convexHullLayer.HullPoint.IsDrawPointBorder = true;
                convexHullLayer.HullPoint.PointRadius = 3;
                convexHullLayer.HullPoint.PointColor = Color.Red;
                convexHullLayer.SetPositionSetTransformByM2MLevel(level);
                convexHullLayer.Active = true;
                layers.Add(convexHullLayer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_PositionSet_Path representativeHullLayer;
        private void OnGetRepresentativeHullInSpecificLevel(ILevel level, int levelSequence, IPositionSet positionSet)
        {
            lock (layers)
            {
                if (representativeHullLayer == null)
                {
                    representativeHullLayer = new Layer_PositionSet_Path(positionSet);
                    representativeHullLayer.PositionSetTranslationX = -(level.ConvertRealValueToRelativeValueX(0));
                    representativeHullLayer.PositionSetTranslationY = -(level.ConvertRealValueToRelativeValueY(0));
                    representativeHullLayer.MainColor = Color.FromArgb(255, 128, 0);
                    representativeHullLayer.PathLine.LineWidth = 2;
                    representativeHullLayer.PathLine.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    representativeHullLayer.PathPoint.PointColor = Color.FromArgb(255, 128, 0);
                    representativeHullLayer.PathPoint.IsDrawPointBorder = true;
                    representativeHullLayer.PathPoint.PointRadius = 2;
                    representativeHullLayer.Active = true;
                    layers.Add(representativeHullLayer);
                }
                representativeHullLayer.SpringLayerMaxRectChangedEvent();
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        Layer_M2MPartSetInSpecificLevel linePartSetLayer = null;
        Layer_PositionSet_Point BottonLevelPositionSetLayer = null;
        PositionSetSet positionSetSet;
        private void OnGetLinePositionSetInSpecificLevel(ILevel level, int levelSequence, IPositionSet positionSet)
        {
            lock (layers)
            {
                if (linePartSetLayer == null)
                {
                    linePartSetLayer = new Layer_M2MPartSetInSpecificLevel(level, positionSet);
                    linePartSetLayer.MainColor = Settings.Default.LinePositionSetInSpecificLevelColor;
                    linePartSetLayer.Alpha = 50;
                    //linePartSetLayer.LineColor = Color.Red;
                    linePartSetLayer.Active = true;
                    layers.Add(linePartSetLayer);

                    positionSetSet = new PositionSetSet();

                    BottonLevelPositionSetLayer = new Layer_PositionSet_Point(positionSetSet);
                    BottonLevelPositionSetLayer.Point.IsDrawPointBorder = true;
                    BottonLevelPositionSetLayer.Point.PointRadius = 2;
                    BottonLevelPositionSetLayer.Point.PointColor = Settings.Default.BottonLevelPositionSetColor;
                    layers.Add(BottonLevelPositionSetLayer);
                }
                else
                {
                    linePartSetLayer.SpringLayerRepresentationChangedEvent(linePartSetLayer);                    
                }

                positionSetSet.Clear();
                positionSet.InitToTraverseSet();
                while (positionSet.NextPosition())
                {
                    IPart tempPart = level.GetPartRefByPartIndex((int)positionSet.GetPosition().GetX(), (int)positionSet.GetPosition().GetY());
                    if (tempPart != null)
                    {
                        positionSetSet.AddPositionSet(m2mStructure.GetBottonLevelPositionSetByAncestorPart((
                            tempPart), levelSequence));
                    }
                }

                BottonLevelPositionSetLayer.SpringLayerRepresentationChangedEvent(BottonLevelPositionSetLayer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnTheEndOfGetLinePositionSetInSpecificLevel()
        {
            linePartSetLayer = null;
        }

        Layer_PositionSet_Point childPositionSetOfConvexHullLayer;
        private void OnGetChildPositionSetInSpecificLevelOfConvexHull(ILevel level, int levelSequence, IPositionSet positionSet)
        {
            lock (layers)
            {
                childPositionSetOfConvexHullLayer = new Layer_PositionSet_Point(new PositionSet_Cloned(positionSet));
                childPositionSetOfConvexHullLayer.SetPositionSetTransformByM2MLevel(level);
                childPositionSetOfConvexHullLayer.MainColor = Settings.Default.ChildPositionSetInSpecificLevelOfConvexHullColor;
                //layer.MainColor = GenerateColor.GetSimilarColor(GenerateColor.GetSimilarColor(GenerateColor.GetSimilarColor(aryMainColor[levelSequence])));
                childPositionSetOfConvexHullLayer.Point.PointRadius = 2;
                childPositionSetOfConvexHullLayer.Point.IsDrawPointBorder = true;
                childPositionSetOfConvexHullLayer.Active = true;
                layers.Add(childPositionSetOfConvexHullLayer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        private void OnGetRealConvexHull(IPositionSet positionSet)
        {
            lock (layers)
            {
                Layer_PositionSet_ConvexHull layer = new Layer_PositionSet_ConvexHull(new PositionSet_Cloned(positionSet));
                layer.MainColor = Settings.Default.RealConvexHullColor;
                layer.ConvexHull.LineWidth = 2;
                layer.Active = true;
                layers.Add(layer);
            }

            flowControlerForm.BeginInvoke(Update);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }
    }
}
