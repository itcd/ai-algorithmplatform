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
    public class AlgorithmDemo_M2M_CD
    {
        M2M_CD M2M_CD;
        Layers layers;
        FlowControlerForm flowControlerForm;
        dUpdate Update;
        IM2MStructure m2mStructure;
        
        
        Color[] aryMainColor = { Color.FromArgb(30, 249, 35), Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 235), Color.FromArgb(236, 19, 225) };

        Layer_PositionSet_Polygon objLayer=null;
        Layer_M2MPartSetInSpecificLevel linePartSetLayer = null;
        
        public AlgorithmDemo_M2M_CD(M2M_CD M2M_CD, Layers layers, FlowControlerForm flowControlerForm, dUpdate update)
        {
            this.M2M_CD = M2M_CD;
            this.layers = layers;
            this.flowControlerForm = flowControlerForm;
            this.Update = update;

            
            M2M_CD.GetCollision += delegate(IPositionSet positionSet)
            {
                System.Diagnostics.Debug.WriteLine("Collision!");

                if (objLayer != null)layers.Remove(objLayer);
                {
                    objLayer = new Layer_PositionSet_Polygon(new PositionSet_Cloned(positionSet));
                    objLayer.PolygonLine.LineColor = Color.Red;
                    objLayer.PolygonLine.LineWidth = 1;
                    layers.Add(objLayer);
                }

                flowControlerForm.BeginInvoke(Update);
                flowControlerForm.SuspendAndRecordWorkerThread();
            };

            M2M_CD.GetNoCollision += delegate(IPositionSet positionSet)
            {
                System.Diagnostics.Debug.WriteLine("NoCollision!");
                if (objLayer != null) layers.Remove(objLayer);
                { 
                    objLayer = new Layer_PositionSet_Polygon(new PositionSet_Cloned(positionSet));
                    objLayer.PolygonLine.LineColor = Color.Green;
                    objLayer.PolygonLine.LineWidth = 1;
                    layers.Add(objLayer);
                }
                flowControlerForm.BeginInvoke(Update);
                flowControlerForm.SuspendAndRecordWorkerThread();
            };

            M2M_CD.GetIntersectPart += delegate(ILevel level, IPositionSet positionSet)
            {
                if (linePartSetLayer == null)
                {
                    linePartSetLayer = new Layer_M2MPartSetInSpecificLevel(level, positionSet);
                    linePartSetLayer.MainColor = Settings.Default.LinePositionSetInSpecificLevelColor;
                    linePartSetLayer.Alpha = 50;
                    //linePartSetLayer.LineColor = Color.Red;
                    linePartSetLayer.Active = true;
                    layers.Add(linePartSetLayer);
                }
                else
                {
                    linePartSetLayer.SpringLayerRepresentationChangedEvent(linePartSetLayer);
                }
            };

            M2M_CD.GetM2MStructure += delegate(IM2MStructure m2mStructure)
            {
                this.m2mStructure = m2mStructure;
            };

            IsGetM2MStructure = true;
            IsGetPositionSetToGetConvexHull = true;

            flowControlerForm.SelectConfiguratoinObject(this);
            flowControlerForm.SuspendAndRecordWorkerThread();
        }

        public void EndDemo()
        {
            IsGetM2MStructure = false;
            IsGetPositionSetToGetConvexHull = false;

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
                        M2M_CD.GetPositionSetToGetConvexHull += OnGetPositionSetToGetConvexHull;
                    }
                    else
                    {
                        M2M_CD.GetPositionSetToGetConvexHull -= OnGetPositionSetToGetConvexHull;
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
                        M2M_CD.GetM2MStructure += OnGetM2MStructure;
                    }
                    else
                    {
                        M2M_CD.GetM2MStructure -= OnGetM2MStructure;
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

    }
}
