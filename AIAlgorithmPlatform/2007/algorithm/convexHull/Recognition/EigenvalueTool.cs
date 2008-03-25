using System;
using System.Collections.Generic;
using System.Text;

namespace Recognition
{
    public class EigenvalueTool
    {
         double TOL ;
        //误差容限   
        int NMAX;
        //方阵阶数   
        int M ;
        //最大叠带次数   
        public EigenvalueTool(int N)
        {
            NMAX = N;
            TOL = 0.001;
            M = 20000;
        }
        public EigenvalueTool(int N,double err)
        {
            NMAX =N;
            TOL = err;
            M = 20000;
        }

        public EigenvalueTool(int N, double err, int iters)
        {
            NMAX = N;
            TOL = err;
            M = iters;
        }

       double Max(double[] u)
        {
            double j = u[0];
            for (int i = 1; i < NMAX; i++)
            {
                if (j < u[i])
                    j = u[i];
            }
            return j;
        }

         void Evaluate(double[] u, double[] u0)
        {
            for (int i = 0; i < NMAX; i++)
                u[i] = u0[i];
        }

        void Multi(double[] v, double[][] a, double[] u)
        {
            for (int i = 0; i < NMAX; i++)
            {
                v[i] = 0;
                for (int j = 0; j < NMAX; j++)
                {
                    v[i] += a[i][j] * u[j];
                }
            }
        }

        public double MaxEigenValue(double[][] a)
        {
        

            double[] u0 = new double[NMAX];//非零初始向量，略去初始化  
            for (int l = 0; l < NMAX; l++)
            {
                u0[l] = 1;
            }
            
            double b;//方阵主特征值的近似值   
            double[] u = new double[NMAX];//近似特征向量   

            double k, ERR;
            int i;
            double[] w, v;
            w = new double[NMAX];
            v = new double[NMAX];
            Evaluate(u, u0);

            b = Max(u);

            for (i = 0; i < NMAX; i++)
            {
                u[i] = u[i] / b;
            }

            for (k = 1; k < M; k++)
            {
                Multi(v, a, u);
                b = Max(v);

                if (0 == b)
                    return 0;//   0是特征值，另选初值u0重新计算   

                for (i = 0; i < NMAX; i++)
                {
                    w[i] = v[i] / b;
                    u[i] = Math.Abs(u[i] - w[i]);
                }
                ERR = Max(u);
                Evaluate(u, w);

                if (ERR < TOL)
                    return b;   //   b是特征值，u是特征向量   
            }

            return 0;   //迭代次数溢出   
        }

        public double[] MaxEigenVector(double[][] a)
        {


            double[] u0 = new double[NMAX];//非零初始向量，略去初始化  
            for (int l = 0; l < NMAX; l++)
            {
                u0[l] = 1;
            }

            double b;//方阵主特征值的近似值   
            double[] u = new double[NMAX];//近似特征向量   

            double k, ERR;
            int i;
            double[] w, v;
            w = new double[NMAX];
            v = new double[NMAX];
            Evaluate(u, u0);

            b = Max(u);

            for (i = 0; i < NMAX; i++)
            {
                u[i] = u[i] / b;
            }

            for (k = 1; k < M; k++)
            {
                Multi(v, a, u);
                b = Max(v);

                if (0 == b)
                    return null;//   0是特征值，另选初值u0重新计算   

                for (i = 0; i < NMAX; i++)
                {
                    w[i] = v[i] / b;
                    u[i] = Math.Abs(u[i] - w[i]);
                }
                ERR = Max(u);
                Evaluate(u, w);

                if (ERR < TOL)
                    return u;   //   b是特征值，u是特征向量   
            }

            return null;   //迭代次数溢出   
        } 

    }
}
