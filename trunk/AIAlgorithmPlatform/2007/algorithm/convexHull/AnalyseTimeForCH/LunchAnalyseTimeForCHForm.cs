using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using CurvePlotDrawer;
using Position_Interface;
using GrahamScanAlgorithm;
using QuickHullAlgorithm;
using JarvisMatchAlgorithm;
using M2M;
using ConvexHullEngine;
using Configuration;
using Position_Implement;

namespace AnalyseTimeForCH
{
    public partial class LunchAnalyseTimeForCHForm : Form
    {
        class CHConfiguration
        {
            int theDensityOfBottomLevel = 20;

            public int TheDensityOfBottomLevel
            {
                get { return theDensityOfBottomLevel; }
                set { theDensityOfBottomLevel = value; }
            }

            distributionStyle dStyle;
            public distributionStyle DistributionStyle
            {
                get { return dStyle; }
                set { dStyle = value; }
            }
        }

        CHConfiguration chc = new CHConfiguration();
                
        public LunchAnalyseTimeForCHForm()
        {
            InitializeComponent();
        }
        
        void GSCH(double n, Object o)
        {
            GrahamScan GS = new GrahamScan();
            GS.ConvexHull((IPositionSet)o);
        }

        void QHCH(double n, Object o)
        {
            QuickHull QH = new QuickHull();
            QH.ConvexHull((IPositionSet)o);
        }

        void JMCH(double n, Object o)
        {
            JarvisMatch JM = new JarvisMatch();
            JM.ConvexHull((IPositionSet)o);
        }

        void M2MCH(double n, Object o)
        {
            M2M_CH MCH = new M2M_CH();
            MCH.TheDensityOfBottomLevel = chc.TheDensityOfBottomLevel; 
            MCH.ConvexHull((IPositionSet)o);
        }

        void M2MPP(double n, Object o)
        {
            M2M_CH MCH = new M2M_CH();
            MCH.TheDensityOfBottomLevel = chc.TheDensityOfBottomLevel;
            MCH.PreProcess((IPositionSet)o);
        }

        Object GetRandomPositionSet(double d)
        {
            return new RandomPositionSet_InFixedDistribution((int)d, chc.DistributionStyle);
            //return new RandomPositionSet_Square((int)d, 0f, 1000f);
        }
        
        M2M_CH m2mch = new M2M_CH();

        Object RunPreProcess(double d)
        {
            IPositionSet temp = new RandomPositionSet_Square((int)d, 0f, 1000f);
            m2mch.PreProcess(temp);
            return temp;
        }

        void M2MQCH(double n, Object o)
        {
            M2M_CH MCH = new M2M_CH();
            m2mch.QueryConvexHull();
        }

        private void LunchAnalyseTimeForCHForm_Load(object sender, EventArgs e)
        {
           
        }

        private void btnAnalyse_Click(object sender, EventArgs e)
        {
            new ConfiguratedByForm(chc);

            CurvePlot_Function_Time curvePlot = new CurvePlot_Function_Time("Comparsion Among Convex Hull Algorithm", "Point Number", "time(ms)", false, GetRandomPositionSet);
            curvePlot.addProc("M2MCH", M2MCH);
            curvePlot.addProc("M2MPP", M2MPP);
            curvePlot.addProc("GSCH", GSCH);
            curvePlot.addProc("QHCH", QHCH);
            curvePlot.addProc("JMCH", JMCH);

            //CurvePlot_Function_Time curvePlot = new CurvePlot_Function_Time("Comparsion Among Convex Hull Algorithm", "Point Number", "time(ms)", false, RunPreProcess);
            //curvePlot.addProc("M2MQCH", M2MQCH);

            //curvePlot.draw(SequenceMaker.genXData_Exp(2, 0, 10));

            //curvePlot.draw(SequenceMaker.GenerateData_Linear(double.Parse(textIncreaseInterval.Text), double.Parse(textMaxPointNum.Text), double.Parse(textIncreaseInterval.Text)));
            curvePlot.draw(SequenceMaker.GenerateData_Multiple());
        }
    }
}