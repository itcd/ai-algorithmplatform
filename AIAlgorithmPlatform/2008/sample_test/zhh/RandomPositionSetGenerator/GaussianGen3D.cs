using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;
using Troschuetz;
//using M2M.Position.RandomGenerator;
using Configuration;
using Position_Implement;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;

namespace RandomPositionSetGenerator3D
{
    class GaussianGen3D : IRandomGenerator3D
    {
        int clusterPointNum=1;

        private int scale = 1;

        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }


        double X_Sigma = 8;

        public double X_Sigma1
        {
            get { return X_Sigma; }
            set { X_Sigma = value; }
        }
        double Y_Sigma = 8;

        public double Y_Sigma1
        {
            get { return Y_Sigma; }
            set { Y_Sigma = value; }
        }

        double Z_Sigma = 8;

        public double Z_Sigma1
        {
            get { return Z_Sigma; }
            set { Z_Sigma = value; }
        }

        float minMu;
        float maxMu;
        int pointNum;
        PositionSet3D positionSet3D = new PositionSet3D();

        public GaussianGen3D(float minMu, float maxMu, int pointNum)
        {
            this.maxMu = maxMu;
            this.minMu = minMu;
            this.pointNum = pointNum;
        }

        #region IRandomGenerator ≥…‘±

        public PositionSet3D getRandomPositionSet(int pointNum)
        {
            unchecked
            {
                int seed = (int) DateTime.Now.Ticks;
                this.pointNum = pointNum;
                NormalDistribution distributionX = new NormalDistribution(new StandardGenerator(seed++));
                distributionX.Sigma = X_Sigma;

                NormalDistribution distributionY = new NormalDistribution(new StandardGenerator(seed++));
                distributionY.Sigma = Y_Sigma;

                NormalDistribution distributionZ = new NormalDistribution(new StandardGenerator(seed++));
                distributionZ.Sigma = Z_Sigma;

                Random r = new Random();

                //int scale = 1;

                for (int i = 0; i < clusterPointNum; i++)
                {
                    distributionX.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    distributionY.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    distributionZ.Mu = minMu + (float)(r.NextDouble() * (maxMu - minMu));
                    RandomPositionSet3D randomPositionSet3D =
                        new RandomPositionSet3D((int) (pointNum/clusterPointNum), scale, distributionX, distributionY, distributionZ);
                    positionSet3D = randomPositionSet3D;
                }
            }
            return positionSet3D;
        }

        #endregion
    }
}
