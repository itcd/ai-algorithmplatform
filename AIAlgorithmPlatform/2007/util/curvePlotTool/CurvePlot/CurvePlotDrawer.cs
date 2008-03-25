using System;
using System.Collections.Generic;
using System.Text;
using CountTimeTool;
using Position_Interface;
using System.IO;
using Position_Implement;

namespace CurvePlotDrawer
{
    //---------------------------------------------
    // 类：CurvePlot
    // 根据点序列来作图
    //---------------------------------------------
    public class CurvePlot
    {
        List<IPositionSet>  psList = new List<IPositionSet>();
        List<string>        nameList = new List<string>();
        CurvePlotForm       form = new CurvePlotForm();
        bool                bLogX;

        //报表设置
        string reportFileName =
            DateTime.Now.Year.ToString() + "-" +
            DateTime.Now.Month.ToString() + "-" +
            DateTime.Now.Day.ToString() + " " +
            DateTime.Now.Hour.ToString() + "." +
            DateTime.Now.Minute.ToString() + "." +
            DateTime.Now.Second.ToString() + " " +
            DateTime.Now.Ticks.ToString() + " " +
            "report.xls";
        string reportTable = null;
        public void setReportFileName(string filename) { reportFileName = filename; }

        //构造函数
        public CurvePlot(string title, string xLabel, string yLabel, bool bLogX)
        {
            form.setTitle(title);
            form.setXLabel(xLabel);
            form.setYLabel(yLabel);
            this.bLogX = bLogX;
        }

        //添加一个点序列
        public void addCurve(string name, IPositionSet ps)
        {
            nameList.Add(name);
            psList.Add(ps);
        }

        //根据点序列来作图
        public void draw()
        {
            //生成报表第一行
            reportTable = "\t";
            IPositionSet ps0 = psList[0];
            ps0.InitToTraverseSet();
            while (ps0.NextPosition())
            {
                IPosition p = ps0.GetPosition();
                reportTable += p.GetX() + "\t";
            }
            reportTable += "\r\n";

            //作图并生成报表
            for (int i = 0; i < nameList.Count; i++)
            {
                reportTable += nameList[i] + "\t";
                List<double> xData = new List<double>();
                List<double> yData = new List<double>();
                IPositionSet ps = psList[i];
                ps.InitToTraverseSet();
                while (ps.NextPosition())
                {
                    if (bLogX)
                        xData.Add(Math.Log10(ps.GetPosition().GetX()));
                    else
                        xData.Add(ps.GetPosition().GetX());
                    yData.Add(ps.GetPosition().GetY());
                    reportTable += ps.GetPosition().GetY() + "\t";
                }
                reportTable += "\r\n";
                form.addCurve(nameList[i], xData.ToArray(), yData.ToArray());
            }
            FileStream fs = new FileStream(reportFileName, System.IO.FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, System.Text.UnicodeEncoding.Unicode);
            sw.Write(reportTable);
            sw.Close();
            fs.Close();
            form.Show();
        }

    }

    public delegate Object GetPublicObjectFunc(double x);

    //---------------------------------------
    // 类： CurvePlot_Function
    // 根据函数作图
    //---------------------------------------
    public class CurvePlot_Function
    {
        public delegate double Func(double x);
        public delegate double FuncWithObject(double x, Object o);
        CurvePlot curvePlot;
        List<Func> funcList = new List<Func>();
        List<FuncWithObject> funcWithObjectList = new List<FuncWithObject>();
        List<string> nameList = new List<string>();
        GetPublicObjectFunc getPublicObjectFunc = null;

        public void setReportFileName(string filename)
        {
            curvePlot.setReportFileName(filename);
        }
        public CurvePlot_Function(string title, string xLabel, string yLabel, bool bLogX)
        {
            curvePlot = new CurvePlot(title, xLabel, yLabel, bLogX);
            getPublicObjectFunc = null;
        }

        public CurvePlot_Function(string title, string xLabel, string yLabel, bool bLogX, GetPublicObjectFunc getPublicObjectFunc)
        {
            curvePlot = new CurvePlot(title, xLabel, yLabel, bLogX);
            this.getPublicObjectFunc = getPublicObjectFunc;
        }

        //添加一个函数
        public void add(string name, Func func)
        {
            if (getPublicObjectFunc != null)
            {
                throw new Exception("getPublicObjectFunc != null");
            }

            nameList.Add(name);
            funcList.Add(func);
        }

        //添加一个函数
        public void add(string name, FuncWithObject funcWithObject)
        {
            if (getPublicObjectFunc == null)
            {
                throw new Exception("getPublicObjectFunc == null");
            }

            nameList.Add(name);
            funcWithObjectList.Add(funcWithObject);
        }

        //根据函数来作图
        public void draw(double[] xData)
        {
            List<IPosition>[] pl = new List<IPosition>[nameList.Count]; //new List<IPosition>()[xData.Length];

            for (int i = 0; i < nameList.Count; i++)
            {
                pl[i] = new List<IPosition>();
            }

            for (int j = 0; j < xData.Length; j++)
            {
                //对每个函数分发同一个object
                Object temp = null;

                if (getPublicObjectFunc != null)
                {
                    temp = getPublicObjectFunc(xData[j]);
                }

                //先对每一个函数计算同一个规模的输入的函数值
                for (int i = 0; i < nameList.Count; i++)
                {
                    Position_Point p = new Position_Point();
                    p.SetX((float)(xData[j]));

                    if (getPublicObjectFunc == null)
                    {
                        p.SetY((float)(funcList[i](xData[j])));
                    }
                    else
                    {
                        p.SetY((float)(funcWithObjectList[i](xData[j], temp)));
                    }

                    pl[i].Add(p);
                }
            }

            for (int i = 0; i < nameList.Count; i++)
            {
                curvePlot.addCurve(nameList[i], new PositionSet_ImplementByIEnumerableTemplate(pl[i]));
            }

            curvePlot.draw();
        }

        public void draw_Old(double[] xData)
        {
            for (int i = 0; i < nameList.Count; i++)
            {
                List<IPosition> pl = new List<IPosition>();
                double[] yData = new double[xData.Length];
                string name = nameList[i];
                Func func = funcList[i];
                for (int j = 0; j < xData.Length; j++)
                {
                    Position_Point p = new Position_Point();
                    p.SetX((float)(xData[j]));
                    p.SetY((float)(func(xData[j])));
                    pl.Add(p);
                }
                curvePlot.addCurve(name, new PositionSet_ImplementByIEnumerableTemplate(pl));
            }
            curvePlot.draw();
        }
    }


    //---------------------------------------
    // 类：CurvePlot_Function_Time
    // 做函数计时并作图
    //---------------------------------------
    public class CurvePlot_Function_Time
    {
        //--------------------------------------------
        // 类：ProcWrapper
        // 把一个无返回值函数封装成一个计时函数
        //--------------------------------------------
        class ProcWrapper
        {
            int askCount = 1;  //计算次数
            public void setAskCount(int ac)
            {
                askCount = ac;
            }
            Proc proc;
            double x;
            TimeCounter timeCounter = new TimeCounter();

            public ProcWrapper(Proc proc)
            {
                this.proc = proc;
            }            

            public double func(double x)
            {
                this.x = x;
                return timeCounter.CountTimeForRepeatableMethod(action);
            }
            void action()
            {
                proc(x);
            }
        }

        class ProcWithObjectWrapper
        {
            int askCount = 1;  //计算次数
            ProcWithObject procWithObject;
            double x;
            Object o;
            TimeCounter timeCounter = new TimeCounter();
            public void setAskCount(int ac)
            {
                askCount = ac;
            }

            public ProcWithObjectWrapper(ProcWithObject procWithObject)
            {
                this.procWithObject = procWithObject;
            }

            public double func(double x, Object o)
            {
                this.x = x;
                this.o = o;
                return timeCounter.CountTimeForRepeatableMethod(action) / askCount;
            }

            void action()
            {
                procWithObject(x,o);
            }
        }

        CurvePlot_Function curvePlot;
        public delegate void Proc(double x);
        public delegate void ProcWithObject(double x, Object o);
        List<Proc> procList = new List<Proc>();

        public void setReportFileName(string filename)
        {
            curvePlot.setReportFileName(filename);
        }
        
        private int askCount = 1;
        public void setAskCount(int ac)
        {
            askCount = ac;
        }
        //List<ProcWrapper> pwList = new List<ProcWrapper>();
        List<string> nameList = new List<string>();

        public CurvePlot_Function_Time (string title, string xLabel, string yLabel, bool bLogX)
        {
            curvePlot = new CurvePlot_Function(title, xLabel, yLabel, bLogX);
        }

        public CurvePlot_Function_Time(string title, string xLabel, string yLabel, bool bLogX, GetPublicObjectFunc getPublicObjectFunc)
        {
            curvePlot = new CurvePlot_Function(title, xLabel, yLabel, bLogX , getPublicObjectFunc);
        }

        //添加一个函数
        public void addProc(string name, Proc proc)
        {
            ProcWrapper pw = new ProcWrapper(proc);
            pw.setAskCount(askCount);
            curvePlot.add(name, pw.func);
        }

        //添加一个函数
        public void addProc(string name, ProcWithObject procWithObject)
        {
            ProcWithObjectWrapper pw = new ProcWithObjectWrapper(procWithObject);
            pw.setAskCount(askCount);
            curvePlot.add(name, pw.func);
        }

        //对函数计时，然后作图
        public void draw(double[] xData)
        {
            curvePlot.draw(xData);
        }
    }
}
